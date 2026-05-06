using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class SPDifficultyPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public UserDataSO userData;


        [Header("Next And Previous Panels")]
        public string boardsState;
        public string previousState;

        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);           
            uiData.uiManager.ChangeState(boardsState, true);

        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(previousState, false);

        }

        public void EasyBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetAIDifficulty(AIDifficulty.Easy);

        }
        public void MediumBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetAIDifficulty(AIDifficulty.Medium);


        }
        public void HardBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetAIDifficulty(AIDifficulty.Hard);
        }
    }
}
