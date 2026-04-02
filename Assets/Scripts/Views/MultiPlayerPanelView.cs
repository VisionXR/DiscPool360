using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace com.VisionXR.Views
{
    public class MultiPlayerPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public GameDataSO gameData;
        public AppPropertiesDataSO appProperties;
        public BoardDataSO boardData;
        public NetworkInputDataSO networkInputData;
        public DestinationSO destinationData;
        public UserDataSO userData;
        public NetworkOutputDataSO networkOutputData;

        [Header("Board Objects")]
        public GameObject HomePanel;
        public GameObject BoardsPanel;
        public GameObject JoinRoomPanel;

        [Header("Bg Images")]
        public Image poolSelectionImage;
        public Image snookerSelectionImage;


        private void OnEnable()
        {
            Initialise();

        }

        private void Initialise()
        {
            ResetGameModeImages();

            if (uiData.currentGameMode == GameMode.Pool)
            {
                poolSelectionImage.gameObject.SetActive(true);
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {             
              snookerSelectionImage.gameObject.SetActive(true);
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


        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomePanel.SetActive(true);
            gameObject.SetActive(false);
        }


        public void CreateRoomBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            BoardsPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void JoinRoomBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            JoinRoomPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        private void ResetGameModeImages()
        {
            poolSelectionImage.gameObject.SetActive(false);
            snookerSelectionImage.gameObject.SetActive(false);
        }

    }
}
