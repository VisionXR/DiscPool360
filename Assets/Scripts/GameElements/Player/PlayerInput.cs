using com.VisionXR.ModelClasses;
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

            inputData.FireStrikeEvent += FireStriker;
            inputData.RotateStrikerAbsoluteEvent += RotateStriker;
            gameData.TurnChangeEvent += SetStriker;

            inputData.StrikerForceChangedEvent += StrikerForceChanged;

        }

        private void OnDisable()
        {

            inputData.FireStrikeEvent -= FireStriker;
            inputData.RotateStrikerAbsoluteEvent -= RotateStriker;
            gameData.TurnChangeEvent -= SetStriker;

            inputData.StrikerForceChangedEvent -= StrikerForceChanged;

        }

        private void StrikerForceChanged(float obj)
        {
           strikerShooting.SetStrikerForce(obj);
        }

        private void RotateStriker(float angle)
        {
            strikerMovement.RotateAbsolute(angle);
        }

        private void FireStriker(float val)
        {
            strikerShooting.FireStriker(val);
        }


        private void SetStriker(int id)
        {
            // Fetching references from TableData/StrikerData
            striker = strikerData.currentStriker;

            if (tableData.platform != null)
                Platform = tableData.platform.gameObject;

            AllAssets = tableData.allAssets;

            if (striker != null)
            {
                strikerShooting = striker.GetComponent<StrikerShooting>();
                strikerMovement = striker.GetComponent<StrikerMovement>();
            }
        }

        private bool CheckTag(string tag)
        {
            if (tag == "Board" || tag == "Solid" || tag == "Stripe" || tag == "BlacK" || tag == "Red" || tag == "Color")
            {
                return true;
            }
            return false;
        }


    }
}