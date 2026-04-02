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

        [Header("Table Elements")]
        public List<Transform> PlayerTransforms;
        public List<Transform> CanvasTransforms;
        public List<Transform> CamTransforms;

        public Platform platform;
        public GameObject Camera;
        public GameObject AllAssets;

        // Actions

        public Action TablePlacedEvent;

        public Action<int> SetTableRotationEvent;

        public Action TurnOnGlowEvent;
        public Action TurnOffGlowEvent;


        public Action TurnOnInteractableEvent;
        public Action TurnOffInteractableEvent;


        public Action TableMovementStartedEvent;
        public Action TableMovementEndedEvent;

        public Action PlatformRotationStartedEvent;
        public Action PlatformRotationEndedEvent;

        public Action ResetPlatformEvent;


        public Action<Vector3> PlatformRotationChangedEvent;


        //Methods
        public Transform GetBoardTransform() => BoardTransform;

        public Transform GetCanvasTransform(int playerID) => CanvasTransforms[playerID - 1];

        public Transform GetPlayerTransform(int playerId) => PlayerTransforms[playerId - 1];
        public Transform GetCamTransform(int playerId) => CamTransforms[playerId - 1];


        public void SetBoardPosition(Transform boardTransform)
        {
            BoardTransform = boardTransform;
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

        public void TurnOnGlow()
        {
            TurnOnGlowEvent?.Invoke();
        }

        public void TurnOffGlow()
        {
            TurnOffGlowEvent?.Invoke();
        }

        public void TurnOnInteractable()
        {
            TurnOnInteractableEvent?.Invoke();
        }

        public void TurnOffInteractable()
        {
            TurnOffInteractableEvent?.Invoke();
        }


        public void PlaceTable()
        {
            TablePlacedEvent?.Invoke();
        }

        public void TableMovementStarted()
        {
            TableMovementStartedEvent?.Invoke();
        }
        public void TableMovementEnded()
        {
            TableMovementEndedEvent?.Invoke();
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

        public void SetPlatform(Platform platform)
        {
            this.platform = platform;
        }

        public void SetCamera(GameObject cam)
        {
            this.Camera = cam;
        }
        public void SetAllAssets(GameObject allAssets)
        {
                this.AllAssets = allAssets;
        }
    }
}
