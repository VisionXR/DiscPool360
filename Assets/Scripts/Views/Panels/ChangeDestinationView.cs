using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class ChangeDestinationView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public GameDataSO gameData;
        public UIDataSO uiData;

        [Header("Next And Previous Panels")]
        public string currentState;

        public void JoinBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();

            gameData.ExitGame();

        }

        public void ResumeBtnClicked()
        {
           
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.GoToState(uiData.uiManager.previousStateName);
        }

        public void SettingsBtnClicked()
        {

            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState("Settings", true);
        }

        public void RulesBtnClicked()
        {

            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState("Rules", true);
        }


    }
}
