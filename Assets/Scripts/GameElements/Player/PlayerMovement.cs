using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public InputDataSO inputData;
     

        [Header(" local Objects")]
        public Player currentPlayer;
       

        void OnEnable()
        {
            //inputData.MovePlayerXEvent += MovePlayerLeftRight;
            //inputData.MovePlayerYEvent += MovePlayerUpDown;
        }

        private void OnDisable()
        {
            //inputData.MovePlayerXEvent -= MovePlayerLeftRight;
            //inputData.MovePlayerYEvent -= MovePlayerUpDown;
        }

        private void MovePlayerUpDown(float value)
        {
            if(currentPlayer == null)
            {
               // camPositionData.MoveCamUpDown(1, value);
            }

            //else if ( currentPlayer.myPlayerRole == PlayerRole.Human && currentPlayer.myPlayerControl == PlayerControl.Local)
            //{
            //  //  camPositionData.MoveCamUpDown(currentPlayer.myId, value);
            //}
        }

        private void MovePlayerLeftRight(float value)
        {

            if(currentPlayer == null)
            {
              //  camPositionData.MoveCamLeftRight(1, value);
            }


            //else if (currentPlayer.myPlayerRole == PlayerRole.Human && currentPlayer.myPlayerControl == PlayerControl.Local)
            //{
            //    camPositionData.MoveCamLeftRight(currentPlayer.myId, value);
            //}
        }
    }
}
