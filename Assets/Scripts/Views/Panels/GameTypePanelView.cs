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

        [Header("Next Objects")]
        public string singlePlayerState;
        public string multiPlayerState;
        public string previousState;


        private void OnEnable()
        {
            Initialise();

        }

        private void Initialise()
        {
          
         
          
        }

        public void SinglePlayerBtnClicked()
        {

            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(singlePlayerState,true);
         
        }

        public void MultiPlayerBtnClicked()
        {

            audioData.PlayAudio(AudioClipType.ButtonClick);
           

        }


        public void NextBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
        }

        public void BackBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(previousState, false);
        }

    }
}
