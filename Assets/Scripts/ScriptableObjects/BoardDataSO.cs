using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "BoardDataSO", menuName = "ScriptableObjects/BoardDataSO", order = 1)]
    public class BoardDataSO : ScriptableObject
    {
        [Header("Board Objects")]
        public List<GameObject> Holes;
        public List<Transform> StrikerFoulPositions;
        public List<Transform> SnookerCoinPositions;

        public GameObject Board;
        public GameObject SnookerObject;
        public GameObject EdgeHighlight;
        public Transform AllCoinsTransform;
        public Transform StrikerTransform;
     
        [Header("Board Properties")]
        public int BoardId = 1;
        public float StrikerRadius;
        public float CoinRadius;
        

        public  bool  isSinglePlayer = false;


        // Actions
        public Action<int> BoardChangedEvent;

        public Action TurnOnInteractableEvent;
        public Action TurnOffInteractableEvent;

        // Methods

        public void TurnOnInteractable()
        {
            TurnOnInteractableEvent?.Invoke();
        }

        public void TurnOffInteractable()
        {
            TurnOffInteractableEvent?.Invoke();
        }

        // Setters for the properties
        public void SetHoles(List<GameObject> holes)
        {
            Holes = holes;
        }

        public void SetSnookerCoinPositions(List<Transform> snookerCoinPositions)
        {
            SnookerCoinPositions = snookerCoinPositions;
        }

        public void SetSnookerObject(GameObject snookerObject)
        {
            SnookerObject = snookerObject;
        }

        public void SetBoard(GameObject board)
        {
            Board = board;
        }

        public void SetEdgeHighlight(GameObject edgeHighlight)
        {
            EdgeHighlight = edgeHighlight;
        }

        public void SetAllCoinsTransform(Transform allCoins)
        {
            AllCoinsTransform = allCoins;
        }

        public void SetStrikerTransform(Transform strikerTransform)
        {
            StrikerTransform = strikerTransform;
        }
        public void SetStrikerFoulPositions(List<Transform> strikerFoulPositions)
        {
            StrikerFoulPositions = strikerFoulPositions;
        }

        public void SetStrikerRadius(float strikerRadius)
        {
            StrikerRadius = strikerRadius;
        }

        public void SetCoinRadius(float coinRadius)
        {
            CoinRadius = coinRadius;
        }

        public void SetBoardId(int boardId)
        {
            BoardId = boardId;
            BoardChangedEvent?.Invoke(boardId);
        }



        public void SetPreviousPanel()
        {
            isSinglePlayer = true;
        }

    }
}
        