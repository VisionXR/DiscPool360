using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CamPositionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]

        public InputDataSO inputData;
        public TableDataSO tableData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;
        public GameDataSO gameData;

        [Header("Game Objects")]
        public GameObject cameraRig;
        public GameObject MainCanvas;
         

        [Header("Local Variables")]
        public float MovementSpeed = 0.005f;
        public float LeftEndPoint = -0.5f;
        public float RightEndPoint = 0.5f;
        public float TopEndPoint = 0.3f;
        public float BottomEndPoint = -0.2f;
      

        private void OnEnable()
        {
          
            inputData.MovePlayerYEvent += MoveCamUpDown;
            inputData.MovePlayerXEvent += MoveCamLeftRight;

            uiData.HomeEvent += ResetCamPosition;
   

        }

        private void OnDisable()
        {
            inputData.MovePlayerYEvent -= MoveCamUpDown;
            inputData.MovePlayerXEvent -= MoveCamLeftRight;

            gameData.ExitGameEvent -= ResetCamPosition;
        }

        private void ResetCamPosition()
        {
            tableData.SetTableRotation(1);
        }

        public void MoveCamLeftRight(float value)
        {
            int id = 1;

            Player mp = playerData.GetMainPlayer();

            if(mp != null)
            {
                id = mp.playerProperties.myId;
            }


            Vector3 centerPosition = tableData.GetCamTransform(id).position;

            centerPosition = new Vector3(centerPosition.x, cameraRig.transform.position.y, centerPosition.z);

            // Calculate the potential new position
            Vector3 potentialNewPosition = cameraRig.transform.position + cameraRig.transform.right * value * MovementSpeed;

            // Calculate the displacement from the center position
            float displacementFromCenter = Vector3.Dot(potentialNewPosition - centerPosition, cameraRig.transform.right);

            // Clamp the displacement
            displacementFromCenter = Mathf.Clamp(displacementFromCenter, LeftEndPoint, RightEndPoint);

            // Set the new position based on the clamped displacement
            cameraRig.transform.position = centerPosition + cameraRig.transform.right * displacementFromCenter;

          
        }



        public void MoveCamUpDown(float value)
        {
            int id = 1;

            Player mp = playerData.GetMainPlayer();

            if (mp != null)
            {
                id = mp.playerProperties.myId;
            }

            Vector3 topLimit = tableData.GetCamTransform(id).position + new Vector3(0, TopEndPoint, 0);
            Vector3 bottomLimit = tableData.GetCamTransform(id).position + new Vector3(0, BottomEndPoint, 0);
            float newY = cameraRig.transform.position.y + value * MovementSpeed;
            if (newY < topLimit.y && newY > bottomLimit.y)
            {
                cameraRig.transform.position = new Vector3(cameraRig.transform.position.x, newY, cameraRig.transform.position.z);
            }
        }

    }
}
