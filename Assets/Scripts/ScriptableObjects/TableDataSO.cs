using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "TableDataSO", menuName = "ScriptableObjects/TableDataSO")]
    public class TableDataSO : ScriptableObject
    {

        // variables
        [Header("Board Properties")]
        public Transform BoardTransform;
        public GameObject allAssets;
        public Platform platform;

        [Header("Table Elements")]
        public List<Transform> PlayerTransforms;
        public List<Transform> CanvasTransforms;
        public List<Transform> CamTransforms;


        // Actions

        public Action<int> SetTableRotationEvent;
        public Action PlatformRotationStartedEvent;
        public Action PlatformRotationEndedEvent;
        public Action ResetPlatformEvent;
        public Action<Vector3> PlatformRotationChangedEvent;


        //Methods
        public Transform GetBoardTransform() => BoardTransform;

        public GameObject GetAllAssets() => allAssets;

        public Platform GetPlatform() => platform;

        public Transform GetCanvasTransform(int playerID) => CanvasTransforms[playerID - 1];

        public Transform GetPlayerTransform(int playerId) => PlayerTransforms[playerId - 1];
        public Transform GetCamTransform(int playerId) => CamTransforms[playerId - 1];


        public void SetPlatform(Platform platform)
        {
            this.platform = platform;
        }

        public void SetBoardPosition(Transform boardTransform)
        {
            BoardTransform = boardTransform;
        }
        public void SetAllAssets(GameObject allAssets)
        {
            this.allAssets = allAssets;
        }

        public void SetMainCanvasPositions(List<Transform> mainCanvasTransforms)
        {
            CanvasTransforms = mainCanvasTransforms;
        }

        public void SetPlayerPositions(List<Transform> playerTransforms)
        {
            PlayerTransforms = playerTransforms;
        }

        public void SetCamPositions(List<Transform> camTransforms)
        {
            this.CamTransforms = camTransforms;
        }


        public void SetTableRotation(int index)
        {
            SetTableRotationEvent?.Invoke(index);
        }


        public void PlatformRotationStarted()
        {
            PlatformRotationStartedEvent?.Invoke();
        }

        public void PlatformRotationEnded()
        {
            PlatformRotationEndedEvent?.Invoke();
        }

        public void ResetPlatform()
        {
            ResetPlatformEvent?.Invoke();
        }

        public void PlatformRotationChanged(Vector3 rotationDelta)
        {
            PlatformRotationChangedEvent?.Invoke(rotationDelta);
        }

    }
}
