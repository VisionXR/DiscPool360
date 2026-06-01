using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class QuitPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public GameDataSO gameData;

        [Header("Next And Previous Panels")]
        public string quitAppState;

        private void OnEnable()
        {
            gameData.GamePaused();
        }

        private void OnDisable()
        {
            gameData.GameResumed();
        }

        public void QuitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            Application.Quit();
        }

        public void ResumeBtnClicked()
        {
           
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.GoToState(uiData.uiManager.previousStateName);

        }

        
    }
}
