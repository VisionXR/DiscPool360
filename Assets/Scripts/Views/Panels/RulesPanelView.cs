using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class RulesPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public AudioDataSO audioData;
        public UIDataSO uiData;
      

        [Header("Panel Objects")]
        public string currentState;


        public void PoolRulesBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
        }

        public void SnookerRulesBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }
    }
}
