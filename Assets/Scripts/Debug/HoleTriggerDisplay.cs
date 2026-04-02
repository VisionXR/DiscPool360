using UnityEngine;

[ExecuteInEditMode]
public class HoleTriggerDisplay : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public Color gizmoColor = Color.blue;
    public float gizmoRadius = 0.05f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, Vector3.one * gizmoRadius);
    }
}
