using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class PlayerTurnManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public StrikerDataSO strikerData;
        public InputDataSO inputData;
        public GameDataSO gameData;
        public BoardDataSO boardData;


        // local
        public Player player;

        private void OnEnable()
        {
            gameData.TurnChangeEvent += ChangeTurn;
        }

        private void OnDisable()
        {
            gameData.TurnChangeEvent -= ChangeTurn;
        }

        private void ChangeTurn(int id)
        {
            strikerData.TurnOnRigidBody();
            strikerData.TurnOnArrow();

            if (player.playerProperties.myId == id)
            {
               
                if (player.playerProperties.myPlayerType == PlayerType.Human && player.playerProperties.myPlayerControl == PlayerControl.Local)
                {                 
                        inputData.EnableInput();
                        boardData.TurnOnInteractable();
                      
                }
                else if (player.playerProperties.myPlayerType == PlayerType.AI && player.playerProperties.myPlayerControl == PlayerControl.Local)
                {
                    // Its AI
                       
                    StartCoroutine(WaitAndPlayAI());
    
                }

            }
      
        }


        private IEnumerator WaitAndPlayAI()
        {
            yield return new WaitForSeconds(0.2f);
            player.aIBehaviour.ExecuteShot(player.playerProperties.myCoin);
        }
    }
}
