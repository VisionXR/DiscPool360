using com.VisionXR.ModelClasses;
using UnityEngine;

public class ReflectionTest : MonoBehaviour
{
    public BoardDataSO boardData;
    public PlayerDataSO playerData;
    public LayerMask layerMask;

    public LineRenderer actualLine;
    public LineRenderer expectedLine;

    public GameObject striker;
    public StrikerShooting strikerShooting;



    // sampling/config
    public int sampleFrameIndex = 10;     // frame at which we take the 3rd point

    

    private Rigidbody strikerRb;
    private bool sampling;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            striker = GameObject.FindWithTag("Striker");
            strikerShooting = striker.GetComponent<StrikerShooting>();
            strikerShooting.Fire(2);
            StartSampling();
        }
    }

    private void StartSampling()
    {
        strikerRb = striker.GetComponent<Rigidbody>();
        sampling = true;

        // Reset lines
        if (actualLine != null) actualLine.positionCount = 0;
        if (expectedLine != null) expectedLine.positionCount = 0;

        // Start sampling sequence
        StopAllCoroutines();
        StartCoroutine(SampleActualAndExpected());
    }

    private System.Collections.IEnumerator SampleActualAndExpected()
    {
        // Collect actual points:
        // 1) Starting point (world)
        // 2) First hit point on edge (world)
        // 3) Position at the given sample frame (world)
        Vector3 startWorld = strikerRb.position;

        // Detect first hit point on edge along current forward motion using spherecast
        Vector3 hitWorld = startWorld; // fallback
        bool gotHit = false;

        // Compute one-step ray parameters
        Vector3 worldVelocity = striker.transform.forward*2;
        Vector3 worldDirection = striker.transform.forward;

        // Try a spherecast forward for a reasonable distance (use platform scale if needed)
        float testDistance = Mathf.Max(2f, worldVelocity.magnitude);
        if (Physics.SphereCast(startWorld, boardData.StrikerRadius, worldDirection, out RaycastHit hitInfo, testDistance, LayerMask.GetMask("Edge")))
        {
            if (hitInfo.collider.CompareTag("Edge"))
            {
                hitWorld = hitInfo.point;
                Debug.Log("Hit edge at: " + hitWorld);
                gotHit = true;
            }
        }

        // Wait up to sampleFrameIndex fixed frames to get frame-10 position
        int frames = 0;
        while (frames < sampleFrameIndex)
        {
            yield return new WaitForFixedUpdate();
            frames++;
        }

        Vector3 frame10World = strikerRb.position;

        // Draw actual line with 3 points
        if (actualLine != null)
        {
            actualLine.positionCount = 3;
            actualLine.SetPosition(0, startWorld);
            actualLine.SetPosition(1, hitWorld);
            actualLine.SetPosition(2, frame10World);
        }

        // Compute expected points using local-space estimator:
        // Inputs to estimator must be platform-local
        Vector3 localStart = startWorld;
        Vector3 localVel = worldVelocity;

        float dt = Time.fixedDeltaTime  * sampleFrameIndex;

        // Estimate the 10th frame point in local, using bounce off the first edge if encountered along direction
        Vector3 estimatedLocal = GetEstimatedReflectedPoint(
            localStart,
            localVel,
            playerData.strikerK,
            dt,
            boardData.StrikerRadius
            
        );

        Vector3 estimatedWorld = estimatedLocal;

        // For expected we also want a mid-point on the edge along path.
        // If we didn't hit with the initial testDistance above, try again with the correct total distance used in estimator:


        // Draw expected line with 3 points: start -> edge-hit (if any) -> estimated 10th frame
        if (expectedLine != null)
        {
            expectedLine.positionCount = 3;
            expectedLine.SetPosition(0, startWorld);
            expectedLine.SetPosition(1, hitWorld);
            expectedLine.SetPosition(2, estimatedWorld);
        }

        sampling = false;
    }

    public Vector3 GetEstimatedReflectedPoint(Vector3 startPos, Vector3 velocity, float k, float dt, float radius)
    {
        Vector3 direction = velocity.normalized;
        float decayFactor = 1 - Mathf.Exp(-k * dt);
        float totalDistance = (velocity.magnitude / k) * decayFactor;

        // First raycast
        if (Physics.SphereCast(startPos, radius, direction, out RaycastHit hit1, totalDistance, LayerMask.GetMask("Edge")))
        {
            if (hit1.collider.CompareTag("Edge"))
            {
                float distToHit1 = hit1.distance - radius;
                float remainingDist1 = totalDistance - distToHit1;

                Vector3 reflectedDir1 = Vector3.Reflect(direction, hit1.normal);

                // Second raycast from hit point in reflected direction
                if (Physics.SphereCast(hit1.point - direction * radius, boardData.StrikerRadius, reflectedDir1, out RaycastHit hit2, remainingDist1, LayerMask.GetMask("Edge")))
                {
                    if (hit2.collider.CompareTag("Edge"))
                    {
                        float distToHit2 = hit2.distance - radius;
                        float remainingDist2 = remainingDist1 - distToHit2;

                        Vector3 reflectedDir2 = Vector3.Reflect(reflectedDir1, hit2.normal);
                        return hit2.point - reflectedDir1 * radius + reflectedDir2 * remainingDist2;
                    }
                    else
                    {
                        return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                    }
                }
                else
                {
                    return hit1.point - direction * radius + reflectedDir1 * remainingDist1;
                }
            }
        }

        // No bounce
        return startPos + direction * totalDistance;
    }
}
