using com.VisionXR.ModelClasses;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;

    [Header("Game Objects")]
    public GameObject line;
    public Transform startTransform;
    public Renderer lineRenderer;

    [Header("Properties")]
    public LayerMask hitMask;
    public float CutOffLength = 1f;
    public  float LineThickness = 0.08f;

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
        if (Physics.SphereCast(startTransform.position,boardData.StrikerRadius,transform.up, out hit,2,hitMask))
        {
            Vector3 hitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            float d = Vector3.Distance(hit.point, transform.position);
            d = d / 0.15f;
            float scaleY = d > CutOffLength ? CutOffLength : d;
            line.transform.localScale = new Vector3(LineThickness, scaleY, 1);

            // Set tiling for lineRenderer's material
            if (lineRenderer != null && lineRenderer.material != null)
            {
                lineRenderer.material.mainTextureScale = new Vector2(1, scaleY*10);
            }
        }
        else
        {
            line.transform.localScale = new Vector3(LineThickness, CutOffLength, 1);

            // Set tiling for lineRenderer's material
            if (lineRenderer != null && lineRenderer.material != null)
            {
                lineRenderer.material.mainTextureScale = new Vector2(1, CutOffLength*10);
            }
        }
    }
}
