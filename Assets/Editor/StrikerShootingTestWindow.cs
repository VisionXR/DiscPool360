using UnityEditor;
using UnityEngine;

public class StrikerShootingTestWindow : EditorWindow
{
    private StrikerShooting strikerShooting;
    private float strikeStrength = 1.0f;

    [MenuItem("Tests/Striker Shooting Test")]
    public static void ShowWindow()
    {
        GetWindow<StrikerShootingTestWindow>("Striker Shooting Test");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Assign StrikerShooting Reference", EditorStyles.boldLabel);
        strikerShooting = (StrikerShooting)EditorGUILayout.ObjectField("StrikerShooting", strikerShooting, typeof(StrikerShooting), true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Strike Strength", EditorStyles.boldLabel);
        strikeStrength = EditorGUILayout.Slider("Strength", strikeStrength, 0f, 5f);

        EditorGUILayout.Space();

        if (strikerShooting != null)
        {
            if (GUILayout.Button("Start Strike"))
            {
                strikerShooting.Fire(strikeStrength);
            }

        }
    }
}