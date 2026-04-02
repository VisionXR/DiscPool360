using UnityEngine;
using UnityEditor;
using com.VisionXR.ModelClasses;

public class SinglePlayerTestWindow : EditorWindow
{
    private GameDataSO gameDataSO;
    private int turnId;

    [MenuItem("Tests/Single Player Test")]
    public static void ShowWindow()
    {
        GetWindow<SinglePlayerTestWindow>("Single Player Test");
    }

    private void OnGUI()
    {
        GUILayout.Label("Single Player Test", EditorStyles.boldLabel);

        gameDataSO = (GameDataSO)EditorGUILayout.ObjectField("GameDataSO", gameDataSO, typeof(GameDataSO), false);
        turnId = EditorGUILayout.IntField("Turn ID", turnId);

        if (GUILayout.Button("ChangeTurn"))
        {
            if (gameDataSO != null)
            {
                gameDataSO.ChangeTurn(turnId);
                Debug.Log($"ChangeTurn called with turnId: {turnId}");
            }
            else
            {
                Debug.LogWarning("GameDataSO reference is missing.");
            }
        }
    }
}
