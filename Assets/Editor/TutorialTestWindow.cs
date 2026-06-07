using UnityEditor;
using UnityEngine;
using System.Reflection;
using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;

public class TutorialTestWindow : EditorWindow
{
    private TableDataSO tableData;
    private TutorialDataSO tutorialData;
    private BoardDataSO boardData;
    private TutorialManager tutorialManager;

    [MenuItem("Tests/Tutorial Test")]
    public static void ShowWindow()
    {
        GetWindow<TutorialTestWindow>("Tutorial Test");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tutorial Test", EditorStyles.boldLabel);

        tableData = (TableDataSO)EditorGUILayout.ObjectField("TableDataSO", tableData, typeof(TableDataSO), false);
        tutorialData = (TutorialDataSO)EditorGUILayout.ObjectField("TutorialDataSO", tutorialData, typeof(TutorialDataSO), false);
        boardData = (BoardDataSO)EditorGUILayout.ObjectField("BoardDataSO", boardData, typeof(BoardDataSO), false);
        tutorialManager = (TutorialManager)EditorGUILayout.ObjectField("TutorialManager", tutorialManager, typeof(TutorialManager), true);

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))
        {
            if (GUILayout.Button("Find Tutorial Manager"))
            {
                tutorialManager = FindObjectOfType<TutorialManager>();
            }

            if (GUILayout.Button("Complete Board Rotation - Step 2"))
            {
                tableData.PlatformRotationEnded();
            }

            if (GUILayout.Button("Complete Aim - Step 3"))
            {
                CompleteAimStep();
            }

            if (GUILayout.Button("Complete Strike - Step 4"))
            {
                CompleteStrikeStep();
            }
        }

        EditorGUILayout.HelpBox("Enter Play Mode, assign SOs and TutorialManager, then test steps.", MessageType.Info);
    }

    private void CompleteAimStep()
    {
        if (tutorialManager == null)
        {
            Debug.LogWarning("TutorialManager is missing.");
            return;
        }


        MethodInfo tappedMethod = typeof(TutorialManager).GetMethod(
            "Tapped",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (tappedMethod != null)
        {
            tappedMethod.Invoke(tutorialManager, new object[] { 1f });
        }
        else
        {
            Debug.LogWarning("Tapped method not found.");
        }
    }

    private void CompleteStrikeStep()
    {
        if (tutorialManager == null)
        {
            Debug.LogWarning("TutorialManager is missing.");
            return;
        }

        FieldInfo coinPocketedField = typeof(TutorialManager).GetField(
            "isCoinPocketed",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (coinPocketedField != null)
        {
            coinPocketedField.SetValue(tutorialManager, true);
        }

        MethodInfo strikeCompletedMethod = typeof(TutorialManager).GetMethod(
            "StrikeCompleted",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (strikeCompletedMethod != null)
        {
            strikeCompletedMethod.Invoke(tutorialManager, null);
        }
        else
        {
            Debug.LogWarning("StrikeCompleted method not found.");
        }
    }
}