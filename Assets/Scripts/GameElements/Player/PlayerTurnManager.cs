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
        public UIDataSO uiData;

        // local
      
        public Player player;
        public PlayerFoul playerFoul;

        private void OnEnable()
        {
            gameData.TurnChangeEvent += ChangeTurn;
            strikerData.FoulCompleteEvent += FoulComplete;

        }

        private void OnDisable()
        {
            gameData.TurnChangeEvent -= ChangeTurn;
            strikerData.FoulCompleteEvent -= FoulComplete;
        }

        private void ChangeTurn(int id)
        {

            if (strikerData.isFoul)
            {
                if (player.playerProperties.myId == id)
                {
                    uiData.ShowTurn(id);
                    if (player.playerProperties.myPlayerControl == PlayerControl.Local)
                    {
                        playerFoul.StartFoulHandling(player.playerProperties.myId);
                        boardData.TurnOnInteractable();
                        inputData.EnableInput();
                    }

                }

                return;
            }

            if (player.playerProperties.myId == id)
            {

                strikerData.TurnOnRigidBody();
                strikerData.TurnOnArrow();
                uiData.ShowTurn(id);
        
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


        public void FoulComplete()
        {


            if (player.playerProperties.myId == gameData.currentTurnId)
            {
                strikerData.TurnOnRigidBody();
                strikerData.TurnOnArrow();
                strikerData.SetFoul(false);

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
