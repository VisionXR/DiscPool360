using com.VisionXR.ModelClasses;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;

    [Header("Game Objects")]
    public GameObject line;
    public Transform checkTransform;
    public Renderer lineRenderer;

    [Header("Properties")]
    public LayerMask colliderMask;
    public float CutOffLength = 1f;
    public float LineThickness = 0.08f;
    public int NoofArrowsPerUnit = 5; // 5 arrows per 0.1m
    public float distanceFactor = 0.15f; // Adjust this based on your line's original length to get the correct scaling
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
        if (Physics.CheckSphere(checkTransform.position, boardData.StrikerRadius, colliderMask))
        {
            line.SetActive(false);
            return;
        }

        line.SetActive(true);

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        float currentDistance = CutOffLength;

        // 2. Perform SphereCast to find the hit point
        if (Physics.SphereCast(rayOrigin, boardData.StrikerRadius, rayDirection, out hit, CutOffLength, colliderMask))
        {
            currentDistance = hit.distance;
          
        }
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * currentDistance, Color.red);

        currentDistance = currentDistance / distanceFactor;


        // 3. Scale the Line (Assuming Y-axis is the "length" of your line object)
        // Adjust the scale to match the distance
        line.transform.localScale = new Vector3(LineThickness, currentDistance, LineThickness);

        // 4. Update Material Tiling
        // Tiling = Distance * ArrowsPerUnit. 
        // This ensures that as the line grows, more arrows appear rather than stretching the existing ones.
        if (lineRenderer != null)
        {
            float tilingValue = currentDistance * NoofArrowsPerUnit;

            // "_MainTex" is the standard property name, but if using URP/HDRP it might be "_BaseMap"
            lineRenderer.material.mainTextureScale = new Vector2(1, tilingValue);
        }
    }
}
