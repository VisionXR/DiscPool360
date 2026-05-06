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
