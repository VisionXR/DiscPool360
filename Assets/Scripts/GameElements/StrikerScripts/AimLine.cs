using com.VisionXR.ModelClasses;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;

    [Header("Game Objects")]
   
    public Transform checkTransform;
    public Renderer lineRenderer;
    public GameObject line;
    public GameObject quadCircle;
    

    [Header("Properties")]
    public LayerMask colliderMask;
    public float CutOffLength = 1f;
    public float arrowCutOffLength = 0.15f;
    public float LineThickness = 0.08f;
    public int NoofArrowsPerUnit = 5; // 5 arrows per 0.1m
    public float distanceFactor = 0.075f; // Adjust this based on your line's original length to get the correct scaling
    

    public float lineOffset = 0.01f;
    public float strikerOffset = 0.01f;
    // local variables
    private RaycastHit hit;

    public void SetCutOffLength(float d)
    {
        CutOffLength = d;
    }

    public void SetColor(Color color)
    {
        if (lineRenderer != null && lineRenderer.material != null)
        {
            lineRenderer.material.color = color;
        }
    }

    private void FixedUpdate()
    {
        // 1. Initial Overlap Check - Hide line if overlapping something
   
        float currentDistance = CutOffLength;
        float hitDistance;

        // 2. Perform SphereCast to find the hit point
        if (Physics.SphereCast(checkTransform.position, boardData.StrikerRadius, checkTransform.forward, out hit, CutOffLength, colliderMask))
        {
            

            currentDistance = hit.distance;
           
            hitDistance = currentDistance / distanceFactor;
            Debug.Log("hit Distance " + hitDistance);
            if (hitDistance < 1)
            {
                line.SetActive(false);
                quadCircle.SetActive(false);
            }
            else
            {
                line.SetActive(true);
                quadCircle.SetActive(true);
                quadCircle.transform.position = hit.point + hit.normal * (boardData.StrikerRadius - strikerOffset); // Slightly above the surface to avoid z-fighting
                                                                                                                    // 3. Scale the Line (Assuming Y-axis is the "length" of your line object)
                currentDistance = Vector3.Distance(line.transform.position, quadCircle.transform.position);
                Debug.Log("current Distance " + currentDistance);
                currentDistance = currentDistance / 0.75f;// Adjust the scale to match the distance
                line.transform.localScale = new Vector3(LineThickness, currentDistance - lineOffset, LineThickness);
            }
        }
        else
        {
            line.SetActive(false);
            quadCircle.SetActive(false);
        }

    }
}
