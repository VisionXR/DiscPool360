using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
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


        [Header("Bg Images")]
        public Image FivePoolSelectionImage;
        public Image EightPoolSelectionImage;
        public Image Snooker6SelectionImage;
        public Image Snooker10SelectionImage;
        public Image ColorSelectionImage;

        [Header("This Objects")]
        public HomePanelView homePanelView;
        public BoardsPanelView boardsPanelView;
        public JoinRoomPanel joinRoomPanelView;
        public List<PanelOnOff> panelsToOff;


        private void OnEnable()
        {
            Initialise();

        }

        private void Initialise()
        {
            ResetGameModeImages();

            if (userData.myCoins == 0)
            {
                EightPoolSelectionImage.gameObject.SetActive(true);
            }
            else if (userData.myCoins == 1)
            {
                FivePoolSelectionImage.gameObject.SetActive(true);
            }

            else if (userData.myCoins == 2)
            {
                Snooker6SelectionImage.gameObject.SetActive(true);
            }
            else if (userData.myCoins == 4)
            {
                Snooker10SelectionImage.gameObject.SetActive(true);
            }
            else if (userData.myCoins == 3)
            {
                ColorSelectionImage.gameObject.SetActive(true);
            }

        }
        public void FivePoolBtnClicked()
        {

            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Pool);
            userData.SetMyCoins(1);
            FivePoolSelectionImage.gameObject.SetActive(true);
        }

        public void EightPoolBtnClicked()
        {

            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Pool);
            userData.SetMyCoins(0);
            EightPoolSelectionImage.gameObject.SetActive(true);
        }
        public void Snooker6BtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Snooker);
            userData.SetMyCoins(2);
            Snooker6SelectionImage.gameObject.SetActive(true);
        }

        public void Snooker10BtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Snooker);
            userData.SetMyCoins(4);
            Snooker10SelectionImage.gameObject.SetActive(true);
        }

        public void ColorBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetGameModeImages();
            uiData.SetGameMode(GameMode.Snooker);
            userData.SetMyCoins(3);
            ColorSelectionImage.gameObject.SetActive(true);
        }


        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            boardsPanelView.TurnOn();
            TurnOff();
        }

        public void JoinRoomBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            joinRoomPanelView.TurnOn();
            TurnOff();

        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            TurnOff();
            homePanelView.TurnOn();
        }

        public void TurnOff()
        {
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOffPanel();
            }
            StartCoroutine(WaitAndTurnOff());
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }


        public void TurnOn()
        {
            gameObject.SetActive(true);
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOnPanel();
            }

        }

        private void ResetGameModeImages()
        {
            FivePoolSelectionImage.gameObject.SetActive(false);
            EightPoolSelectionImage.gameObject.SetActive(false);
            Snooker6SelectionImage.gameObject.SetActive(false);
            Snooker10SelectionImage.gameObject.SetActive(false);
            ColorSelectionImage.gameObject.SetActive(false);
        }

    }
}
