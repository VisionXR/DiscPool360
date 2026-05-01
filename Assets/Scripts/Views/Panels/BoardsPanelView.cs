using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class BoardsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public BoardDataSO boardData;
        public AudioDataSO audioData;
        public AppPropertiesDataSO appProperties;
        public UserDataSO userData;
        public UIDataSO uiData;
        public PurchaseDataSO purchaseData;

        [Header("Board Images")]
        public List<Image> boardSelectionImages;
        public List<Image> boardLockImages;

        [Header("This Objects")]
        public List<PanelOnOff> panelsToOff;

        [Header("Panel Objects")]
        public PurchasePanelView PurchasePanel;
        public SinglePlayerView singlePlayerView;
        public MultiPlayerPanelView multiPlayerPanelView;
        public DestinationPanelView destinationPanel;

        void OnEnable()
        {
            boardSelectionImages[userData.myBoard].gameObject.SetActive(true);
            OpenLock();
        }


        private void OpenLock()
        {
            UnlockBoards(0, 2); 
            foreach (AssetData data in purchaseData.BoardsData)
            {
                if (data.isPurchased)
                {
                    int id = purchaseData.BoardsData.IndexOf(data);


                    // Unlock striker images based on purchased id
                    if (id == 0)
                    {
                        UnlockBoards(3, 5); // Unlock 1,2,3,4 (indices 0-4)
                    }
                    else if (id == 1)
                    {
                        UnlockBoards(6, 8); // Unlock 5,6,7,8,9 (indices 5-9)
                    }
                    else if (id == 2)
                    {
                        UnlockBoards(9, 11); // Unlock 10,11,12,13,14 (indices 10-14)
                    }
                    else if (id == 3)
                    {
                        UnlockBoards(12, 14); // Unlock 10,11,12,13,14 (indices 10-14)
                    }
                    else if (id == 4)
                    {
                        UnlockBoards(15, 17); // Unlock 10,11,12,13,14 (indices 10-14)
                    }
                    else if (id == 5)
                    {
                        UnlockBoards(18, 20); // Unlock 10,11,12,13,14 (indices 10-14)
                    }
                    else if (id == 6)
                    {
                        UnlockBoards(0, 20); // Unlock 10,11,12,13,14 (indices 10-14)
                    }

                }
            }

                UnlockBoards(0, 20); // for testing now remove later
            

        }

        private void UnlockBoards(int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                boardLockImages[i].gameObject.SetActive(false);
            }

        }


        public void BoardSelected(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if(!boardLockImages[id].gameObject.activeInHierarchy)
            {
                userData.SetBoard(id);
                ResetBoardImages();

                boardSelectionImages[userData.myBoard].gameObject.SetActive(true);
              
            }
            else
            {
                TurnOff();
                PurchasePanel.TurnOn();
                
            }
           
        }

        public void StartGameBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            Destination destination = new Destination();
            destination.region = ServerRegion.asia;
            destination.lobbyName = "asia";
            destination.gameType = uiData.currentGameType;
            destination.gameMode = uiData.currentGameMode;
            destination.aIDifficulty = uiData.currentAIDifficulty;
            
            destinationPanel.TurnOn();
            destinationPanel.ConnectToDestination(destination);

             TurnOff();
        }
        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if(uiData.currentGameType == GameType.SinglePlayer)
            {
                
            }
            else if(uiData.currentGameType == GameType.MultiPlayer)
            {
                
            }
            
        }

        private void ResetBoardImages()
        {

            foreach (Image boardImage in boardSelectionImages)
            {
                boardImage.gameObject.SetActive(false);
            }
        }

        public void TurnOff()
        {
         
            StartCoroutine(WaitAndTurnOff());
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            
        }


        public void TurnOn()
        {
           


        }
    }

}