using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace com.VisionXR.Views
{
    public class SinglePlayerView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public GameDataSO gameData;
        public AppPropertiesDataSO appProperties;
        public BoardDataSO boardData;
        public DestinationSO destinationData;
        public UserDataSO userData;

        [Header("Board Objects")]
        public GameObject HomePanel;
        public GameObject BoardsPanel;

        [Header("Bg Images")]
        public Image poolSelectionImage;
        public Image snookerSelectionImage;

        public Image easyAISelectionImage;
        public Image mediumAISelectionImage;
        public Image hardAISelectionImage;


        private void OnEnable()
        {
            Initialise();
       
        }

        private void Initialise()
        {
            ResetGameModeImages();
            ResetAIImages();
           

            if (uiData.currentGameMode == GameMode.Pool)
            {

                poolSelectionImage.gameObject.SetActive(true);
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
              
               snookerSelectionImage.gameObject.SetActive(true);
            }


           if(uiData.currentAIDifficulty == AIDifficulty.Easy)
            {
               
               easyAISelectionImage.gameObject.SetActive(true);
            }
            else if (uiData.currentAIDifficulty == AIDifficulty.Medium)
            {
              
                mediumAISelectionImage.gameObject.SetActive(true);
            }
            else if (uiData.currentAIDifficulty == AIDifficulty.Hard)
            {
              
               hardAISelectionImage.gameObject.SetActive(true);
            }     

        }

        public void PoolBtnClicked()
        {
            
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Pool);
            poolSelectionImage.gameObject.SetActive(true);

        }

        public void SnookerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Snooker);
            snookerSelectionImage.gameObject.SetActive(true);
        }

        public void EasyAIBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetAIImages();
            uiData.SetAIDifficulty(AIDifficulty.Easy);
            easyAISelectionImage.gameObject.SetActive(true);
        }

        public void MediumAIBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetAIImages();
            uiData.SetAIDifficulty(AIDifficulty.Medium);
            mediumAISelectionImage.gameObject.SetActive(true);
        }

        public void HardAIBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetAIImages();
            uiData.SetAIDifficulty(AIDifficulty.Hard);
           hardAISelectionImage.gameObject.SetActive(true);
        }


        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            BoardsPanel.SetActive(true);
            gameObject.SetActive(false);
        }



        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomePanel.SetActive(true);
            gameObject.SetActive(false);
        }


        private void ResetGameModeImages()
        {

           poolSelectionImage.gameObject.SetActive(false);
            snookerSelectionImage.gameObject.SetActive(false);
      
        }

        private void ResetAIImages()
        {
            easyAISelectionImage.gameObject.SetActive(false);
            mediumAISelectionImage.gameObject.SetActive(false);
            hardAISelectionImage.gameObject.SetActive(false);
        }

    }
}
