using com.VisionXR.ModelClasses;
using Photon.Realtime;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public GameDataSO gameData;
        public StrikerDataSO strikerData;
        public TableDataSO tableData;
        public BoardDataSO boardData;
        public PlayerDataSO playerData;

        [Header("Game Objects")]
        public GameObject AllAssets;
        public GameObject Platform;
        public GameObject striker;
        public StrikerShooting strikerShooting;
        public StrikerMovement strikerMovement;


        // Actions
        public Action PlatformRotatedEvent;



        private void OnEnable()
        {       

            inputData.RotateStrikerEvent += RotateStriker;
            inputData.FireStrikeEvent += FireStriker;
      
        }

        private void OnDisable()
        {
            
            inputData.RotateStrikerEvent -= RotateStriker;
            inputData.FireStrikeEvent -= FireStriker;
        }

        private void FireStriker(float val)
        {
            strikerShooting.FireStriker(val);
        }

        private void RotateStriker(Vector2 dir)
        {
            SetStriker();
            int id = playerData.GetMainPlayer().playerProperties.myId;
            Vector3 forward = tableData.GetCanvasTransform(id).forward;
            Vector3 right = tableData.GetCanvasTransform(id).right;
            Vector3 viewDirection = strikerData.currentStriker.transform.position + forward * dir.y + right * dir.x;

            strikerMovement.RotateTo(viewDirection);
            strikerShooting.SetStrikerForce(dir.magnitude); // use joystick magnitude as normalized force (0..1)

        }


        private void SetStriker()
        {
            // Fetching references from TableData/StrikerData
            striker = strikerData.currentStriker;

            if (tableData.platform != null)
                Platform = tableData.platform.gameObject;

            AllAssets = tableData.AllAssets;

            if (striker != null)
            {
                strikerShooting = striker.GetComponent<StrikerShooting>();
                strikerMovement = striker.GetComponent<StrikerMovement>();
            }
        }

   

        // Keep these for external/legacy calls if needed
        public void RotateTo(Vector3 direction) => strikerMovement?.RotateTo(direction);
        public void RotateRelative(float angle) => strikerMovement?.RotateRelative(angle);
        public void StartStrike(float value) => strikerShooting?.StartStrike(value);
        public void EndStrike(float value) => strikerShooting?.EndStrike(value);
    }
}