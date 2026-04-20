using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreation : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public BoardDataSO boardData;
    public CoinDataSO coinData;
    public UserDataSO userData;
    public GameDataSO gameData;

    [Header("Local Settings")]
    public Transform boardTransform;
    public GameObject currentBoard;

    // The sub-folder inside your 'Resources' folder where boards are stored
    // e.g. "Assets/Resources/Boards/Board_0.prefab" -> use "Boards/Board_"
    [SerializeField] private string boardResourcePrefix = "Boards/Board_";

    private void OnEnable()
    {
        userData.BoardChangedEvent += CreateBoard;
        userData.CreateSameBoardEvent += CreateSameBoard;

        CreateBoard(userData.myBoard);
    }

    private void OnDisable()
    {
        userData.BoardChangedEvent -= CreateBoard;
        userData.CreateSameBoardEvent -= CreateSameBoard;
    }

    private void CreateBoard(int id)
    {
        if (currentBoard != null)
        {
            Destroy(currentBoard);
            // Optional: Unload unused assets to free up memory
            Resources.UnloadUnusedAssets();
        }

        // 1. Construct the path (e.g., "Boards/Board_0")
        string resourcePath = boardResourcePrefix + id;

        // 2. Load the prefab from the Resources folder
        GameObject boardPrefab = Resources.Load<GameObject>(resourcePath);

        if (boardPrefab != null)
        {
            // 3. Instantiate the loaded prefab
            currentBoard = Instantiate(boardPrefab, boardTransform);
            // currentBoard.transform.parent = boardTransform; // Redundant since it's in Instantiate
        }
        else
        {
            Debug.LogError($"[BoardCreation] Failed to find board at Resources/{resourcePath}");
        }
    }

    private void CreateSameBoard()
    {
        CreateBoard(userData.myBoard);
    }

    public void StartTutorial()
    {
        if (currentBoard != null) currentBoard.SetActive(false);
    }

    public void EndTutorial()
    {
        if (currentBoard != null) currentBoard.SetActive(true);
    }
}