using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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

        [Header("Bg Images Games")]
        public Image FivePoolSelectionImage;
        public Image EightPoolSelectionImage;
        public Image Snooker6SelectionImage;
        public Image Snooker10SelectionImage;
        public Image ColorSelectionImage;

        [Header("Bg Images AI")]
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
            else if (userData.myCoins == 3)
            {
                ColorSelectionImage.gameObject.SetActive(true);
            }
            else if (userData.myCoins == 4)
            {
                Snooker10SelectionImage.gameObject.SetActive(true);
            }



            if (uiData.currentAIDifficulty == AIDifficulty.Easy)
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
            FivePoolSelectionImage.gameObject.SetActive(false);
            EightPoolSelectionImage.gameObject.SetActive(false);
            Snooker6SelectionImage.gameObject.SetActive(false);
            Snooker10SelectionImage.gameObject.SetActive(false);
            ColorSelectionImage.gameObject.SetActive(false);
        }

        private void ResetAIImages()
        {
            easyAISelectionImage.gameObject.SetActive(false);
            mediumAISelectionImage.gameObject.SetActive(false);
            hardAISelectionImage.gameObject.SetActive(false);
        }

    }
}
