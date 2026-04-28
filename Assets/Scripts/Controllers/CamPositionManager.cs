using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public BoardDataSO boardData;


        [Header("Game Objects")]
        public List<Transform> camTransforms; // List of camera properties for different boards
        public GameObject cameraRig;




        private void OnEnable()
        {
            SetCamProperties(1);
            uiData.HomeEvent += ResetCamPosition;
            tableData.SetCamRotationEvent += SetCamProperties;
        }

        private void OnDisable()
        {
            uiData.HomeEvent -= ResetCamPosition;
            tableData.SetCamRotationEvent -= SetCamProperties;
        }

      
        private void ResetCamPosition()
        {
            SetCamProperties(1);

        }

        public void SetCamProperties(int id)
        {
            id = id - 1;
            Transform camTransform = camTransforms[id];
            cameraRig.transform.position = camTransform.position;
            cameraRig.transform.rotation = camTransform.rotation;

        }
      
    }


}
