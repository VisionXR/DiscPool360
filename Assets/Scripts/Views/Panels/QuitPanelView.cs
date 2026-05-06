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

        [Header("Next And Previous Panels")]
        public string quitAppState;

        public void QuitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            Application.Quit();
        }

        public void ResumeBtnClicked()
        {
           
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(quitAppState, false);
         
        }

        
    }
}
