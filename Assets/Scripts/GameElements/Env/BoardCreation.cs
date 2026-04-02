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


    [Header("Local Objects")]
    public Transform boardTransform;
    public GameObject currentBoard;
    public List<GameObject> allBoardPrefabs;

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
        }

        currentBoard = Instantiate(allBoardPrefabs[id], boardTransform);
        currentBoard.transform.parent = boardTransform;
    }

    private void CreateSameBoard()
    {
        if (currentBoard != null)
        {
            Destroy(currentBoard);
        }

        currentBoard = Instantiate(allBoardPrefabs[userData.myBoard], boardTransform);
        currentBoard.transform.parent = boardTransform;
    }

    public void StartTutorial()
    {
        if (currentBoard != null)
        {
            currentBoard.SetActive(false);
        }
    }
    public void EndTutorial()
    {
        if (currentBoard != null)
        {
            currentBoard.SetActive(true);
        }
    }
}
