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
//    public GameObject NormalLine;
    

    [Header("Properties")]
    public LayerMask colliderMask;
    public float CutOffLength = 1f;
    public float LineThickness = 0.08f;
    public int NoofArrowsPerUnit = 5; // 5 arrows per 0.1m
    public float distanceFactor = 0.15f; // Adjust this based on your line's original length to get the correct scaling
    public float checkFactor = 1.5f;


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
        if (Physics.CheckSphere(checkTransform.position, boardData.StrikerRadius*checkFactor, colliderMask))
        {
               line.SetActive(false);
               quadCircle.SetActive(false);
             //  NormalLine.SetActive(false);
              
            return;
        }

        line.SetActive(true);
        quadCircle.SetActive(true);
     //   NormalLine.SetActive(true);

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        float currentDistance = CutOffLength;

        // 2. Perform SphereCast to find the hit point
        if (Physics.SphereCast(rayOrigin, boardData.StrikerRadius, rayDirection, out hit, CutOffLength, colliderMask))
        {
            currentDistance = hit.distance;
            quadCircle.transform.position = hit.point + hit.normal * (boardData.StrikerRadius-strikerOffset); // Slightly above the surface to avoid z-fighting


            Transform coin = hit.collider.transform;

     //       NormalLine.transform.position = hit.collider.transform.position; // Position at the hit point
           

     //       NormalLine.transform.rotation = Quaternion.LookRotation(-Vector3.up,-hit.normal);
           
        }
     

        currentDistance = currentDistance / distanceFactor;


        // 3. Scale the Line (Assuming Y-axis is the "length" of your line object)
        // Adjust the scale to match the distance
        line.transform.localScale = new Vector3(LineThickness, currentDistance-lineOffset, LineThickness);

       

    }
}
