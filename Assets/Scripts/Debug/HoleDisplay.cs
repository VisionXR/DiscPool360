using UnityEngine;

[ExecuteInEditMode]
public class HoleDisplay : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color gizmoColor = Color.yellow;
    public float gizmoRadius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
