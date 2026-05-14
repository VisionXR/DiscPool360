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


        [Header("Selection Objects")]
        public GameObject SPSelectedImage;
        public GameObject MPSelectedImage;


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

        private void ResetImages()
        {
            SPSelectedImage.SetActive(false);
            MPSelectedImage.SetActive(false);
        }

    }
}
