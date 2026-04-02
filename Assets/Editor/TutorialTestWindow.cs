using UnityEditor;

using UnityEngine;

using System.Reflection;

using com.VisionXR.ModelClasses;

public class TutorialTestWindow : EditorWindow

{

    private TableDataSO tableData;

    private TutorialDataSO tutorialData;

    public BoardDataSO boardData;

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

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))

        {

            if (GUILayout.Button("Complete Table Movement"))

            {

                tableData.TableMovementEnded();

            }

            if (GUILayout.Button("Complete Board Rotation"))

            {

                tableData.PlatformRotationEnded();

            }

        }

        EditorGUILayout.HelpBox("Enter Play Mode. Assign TutorialDataSO, then click 'Complete Step' to trigger the step-complete action.", MessageType.Info);

    }

    private void CompleteTableMovement()

    {

    }


}
