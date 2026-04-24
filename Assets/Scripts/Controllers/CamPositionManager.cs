using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CamPositionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public StrikerDataSO strikerData;
        public InputDataSO inputData;
        public TableDataSO tableData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;
        

        [Header("Game Objects")]
        public GameObject cameraRig;
      
         

        [Header("Local Variables")]
        public float MovementSpeed = 0.005f;
        private Transform StartTransform;

      
        private void OnEnable()
        {
            uiData.HomeEvent += ResetCamPosition;
  
        }

        private void OnDisable()
        {
            uiData.HomeEvent -= ResetCamPosition;
        }

        private void ResetCamPosition()
        {
            tableData.SetTableRotation(1);
        }

        public void MoveCamForward(float value)
        {
            int id = 1;

            Player mp = playerData.GetMainPlayer();

            if(mp != null)
            {
                id = mp.playerProperties.myId;
            }

            StartTransform = tableData.GetPlayerTransform(id);
        }


    }
}
