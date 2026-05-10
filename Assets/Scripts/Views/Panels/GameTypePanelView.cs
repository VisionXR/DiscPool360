using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;



namespace com.VisionXR.Views
{
    public class GameTypePanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public UserDataSO userData;

        [Header("UI Objects")]
        public TMP_Text headingText;

        [Header("Selection Objects")]
        public GameObject SPSelectedImage;
        public GameObject MPSelectedImage;

        [Header("Next Objects")]
        public string singlePlayerState;
        public string multiPlayerState;
        public string currentState;


        private void OnEnable()
        {
            ResetImages();
            if(uiData.currentGameType == GameType.SinglePlayer)
            {
                SPSelectedImage.SetActive(true);
            }
            else if (uiData.currentGameType == GameType.MultiPlayer)
            {
                MPSelectedImage.SetActive(true);
            }

            if (userData.myCoins == 0)
            {
                headingText.text = "EightPool";
            }
            else if (userData.myCoins == 1)
            {
                headingText.text = "FivePool";
            }
            else if (userData.myCoins == 2)
            {
                headingText.text = "TenSnooker";
            }

            else if (userData.myCoins == 3)
            {
                headingText.text = "SixSnooker";
            }
            else if (userData.myCoins == 4)
            {
                headingText.text = "ColorChallenge";
            }
        }


        public void SinglePlayerBtnClicked()
        {
            ResetImages();
            SPSelectedImage.SetActive(true);
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameType(GameType.SinglePlayer);
           

        }

        public void MultiPlayerBtnClicked()
        {

            ResetImages();
            MPSelectedImage.SetActive(true);
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameType(GameType.MultiPlayer);
        }


        public void NextBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (uiData.currentGameType == GameType.SinglePlayer)
            {
                uiData.uiManager.ChangeState(singlePlayerState, true);
            }
            else if (uiData.currentGameType == GameType.MultiPlayer)
            {
                uiData.uiManager.ChangeState(multiPlayerState, true);
            }
        }

        public void BackBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }


        private void ResetImages()
        {
            SPSelectedImage.SetActive(false);
            MPSelectedImage.SetActive(false);
        }

    }
}
