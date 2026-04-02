using UnityEditor;
using UnityEngine;
using com.VisionXR.GameElements;

public class StrikerMovementTestWindow : EditorWindow
{
    private StrikerMovement strikerMovement;
    private float absoluteAngle = 0f;
    private float relativeDelta = 0f;

    [MenuItem("Tests/Striker Movement Test")]
    public static void ShowWindow()
    {
        GetWindow<StrikerMovementTestWindow>("Striker Movement Test");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Assign StrikerMovement Reference", EditorStyles.boldLabel);
        strikerMovement = (StrikerMovement)EditorGUILayout.ObjectField("StrikerMovement", strikerMovement, typeof(StrikerMovement), true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Absolute Rotation", EditorStyles.boldLabel);
        absoluteAngle = EditorGUILayout.Slider("Absolute Angle (Y)", absoluteAngle, 0f, 360f);
        if (strikerMovement != null)
        {
            if (GUILayout.Button("Apply Absolute Rotation"))
            {
                strikerMovement.RotateAbsolute(absoluteAngle);
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Relative Rotation", EditorStyles.boldLabel);
        relativeDelta = EditorGUILayout.Slider("Relative Delta (Y)", relativeDelta, -180f, 180f);
        if (strikerMovement != null)
        {
            if (GUILayout.Button("Apply Relative Rotation"))
            {
                strikerMovement.RotateRelative(relativeDelta);
            }
        }
    }
}
