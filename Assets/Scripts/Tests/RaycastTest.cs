using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public BoardDataSO boardData;

    [Header("Board Data")]
    public GameObject striker;
    public GameObject hole;
    public GameObject coin;

    // Layer mask to use for the SphereCast (editable in inspector)
    public LayerMask layerMask = -1;

    private RaycastHit hitInfo;

    // Debug visualization state
    private Vector3 lastOrigin;
    private Vector3 lastDirection;
    private float lastDistance;
    private bool lastHit;
    private Vector3 lastHitPoint;
    public bool showDebug = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Toggle visualisation: perform a new cast when enabling, clear when disabling
            if (!showDebug)
            {
                PerformSphereCast();
                showDebug = true;
            }
            else
            {
                showDebug = false;
                lastDistance = 0f;
            }
        }
    }

    private void PerformSphereCast()
    {
        if (coin == null || hole == null || boardData == null)
        {
            Debug.LogWarning("[RaycastTest] Missing references (coin/hole/boardData).");
            showDebug = false;
            return;
        }

        Vector3 holeDir = (hole.transform.position - coin.transform.position).normalized;
        float maxDistance = 2f;

        lastOrigin = coin.transform.position;
        lastDirection = holeDir;
        lastDistance = maxDistance;

        if (Physics.SphereCast(lastOrigin, boardData.CoinRadius, lastDirection, out hitInfo, maxDistance, layerMask))
        {
            GameObject tmpObj = hitInfo.collider.gameObject;
            Debug.Log("[RaycastTest] Hit: " + tmpObj.name);
            lastHit = true;
            lastHitPoint = hitInfo.point;
        }
        else
        {
            Debug.Log("[RaycastTest] No hit");
            lastHit = false;
            lastHitPoint = lastOrigin + lastDirection * lastDistance;
        }
    }

    // Visualize the last cast in the Scene view / Game view (runtime)
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!showDebug) return;
        if (lastDistance <= 0f) return;
        if (boardData == null) return;

        // Draw sweep line
        Gizmos.color = lastHit ? Color.red : Color.green;
        Gizmos.DrawLine(lastOrigin, lastOrigin + lastDirection * lastDistance);

        // Draw wire spheres along the sweep to visualize the sphere sweep
        Gizmos.color = Color.cyan;
        float step = Mathf.Max(boardData.CoinRadius * 0.75f, 0.05f);
        int steps = Mathf.Max(1, Mathf.CeilToInt(lastDistance / step));
        for (int i = 0; i <= steps; i++)
        {
            float t = (i / (float)steps) * lastDistance;
            Vector3 pos = lastOrigin + lastDirection * t;
            Gizmos.DrawWireSphere(pos, boardData.CoinRadius);
        }

        // Draw origin indicator
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.8f);
        Gizmos.DrawSphere(lastOrigin, boardData.CoinRadius * 0.12f);

        // Draw hit point marker if hit
        if (lastHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHitPoint, 0.02f);
            // draw normal
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(lastHitPoint, lastHitPoint + hitInfo.normal * 0.2f);
        }
    }
}
