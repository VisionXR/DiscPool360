using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class ExitPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public GameDataSO gameData;
        public UIDataSO uiData;

        [Header("Next And Previous Panels")]
        public string currentState;

        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            gameData.ExitGame();
            uiData.uiManager.ChangeState("GameType", false);
            uiData.uiManager.ChangeState(currentState, false);
            uiData.uiManager.ResetAllBools();
           
        }

        public void ResumeBtnClicked()
        {
           
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

        
    }
}
