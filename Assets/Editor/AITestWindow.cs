using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEditor;
using UnityEngine;

public class AITestWindow : EditorWindow
{

    // Scriptable references
    private GameDataSO gameData;
    private CoinDataSO coinData;
    private PlayerDataSO playerData;
    private StrikerDataSO strikerData;
    public BoardDataSO boardData;

    // Turn state
    private int turnId = 1;

    [MenuItem("Tests/AI Test")]
    public static void ShowWindow()
    {
        GetWindow<AITestWindow>("AI Test");
    }

    private void OnGUI()
    {
       

        EditorGUILayout.LabelField("Game Data", EditorStyles.boldLabel);
        gameData = (GameDataSO)EditorGUILayout.ObjectField("GameDataSO", gameData, typeof(GameDataSO), false);
        coinData = (CoinDataSO)EditorGUILayout.ObjectField("CoinDataSO", coinData, typeof(CoinDataSO), false);
        playerData = (PlayerDataSO)EditorGUILayout.ObjectField("PlayerDataSO", playerData, typeof(PlayerDataSO), false);
        strikerData = (StrikerDataSO)EditorGUILayout.ObjectField("StrikerDataSO", strikerData, typeof(StrikerDataSO), false);
        boardData = (BoardDataSO)EditorGUILayout.ObjectField("BoardDataSO", boardData, typeof(BoardDataSO), false);
        turnId = EditorGUILayout.IntField("Turn Id", turnId);

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(!Application.isPlaying || playerData == null || strikerData == null || coinData == null))
        {
            if (GUILayout.Button("Start AI (Instantiate Player, Striker, Coins)"))
            {
                StartAI();
            }
        }

        using (new EditorGUI.DisabledScope(!Application.isPlaying || gameData == null))
        {
            if (GUILayout.Button("Change Turn"))
            {
                ChangeTurn();
            }

            if (GUILayout.Button("Reset Striker"))
            {
                strikerData.ResetStriker();
                strikerData.SetFoul(false);
            }
        }

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to run Start AI and Change Turn.", MessageType.Info);
        }
    }

    private void StartAI()
    {
        playerData.CreatePlayer(new PlayerProperties
        {
            myId = 2,
            myPlayerType = PlayerType.AI,
            myAiDifficulty = AIDifficulty.Hard,
            myPlayerControl = PlayerControl.Local,
            myCoin = PlayerCoin.AllPool
        });

        strikerData.CreateStriker(boardData.StrikerTransform);
        coinData.CreateCoins(GameMode.Pool, boardData.AllCoinsTransform);

    }

    private void ChangeTurn()
    {
        // Update current turn id and notify GameDataSO
        gameData.ChangeTurn(turnId);
        Debug.Log($"[AITestWindow] ChangeTurn called with turnId={turnId}");
    }
}