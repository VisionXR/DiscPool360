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
        public string currentState;

        [Header("Selection Objects")]
        public GameObject AIEasySelectedImage;
        public GameObject AIMediumSelectedImage;
        public GameObject AIHardSelectedImage;


        private void OnEnable()
        {
            ResetImages();  
            if(uiData.currentAIDifficulty == AIDifficulty.Easy)
            {
                AIEasySelectedImage.SetActive(true);
            }
            else if (uiData.currentAIDifficulty == AIDifficulty.Medium)
            {
                AIMediumSelectedImage.SetActive(true);
            }
            else if (uiData.currentAIDifficulty == AIDifficulty.Hard)
            {
                AIHardSelectedImage.SetActive(true);
            }
        }

        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);           
            uiData.uiManager.ChangeState(boardsState, true);

        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);

        }

        public void EasyBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            AIEasySelectedImage.SetActive(true);
            uiData.SetAIDifficulty(AIDifficulty.Easy);

        }
        public void MediumBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            AIMediumSelectedImage.SetActive(true);
            uiData.SetAIDifficulty(AIDifficulty.Medium);
        }
        public void HardBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            AIHardSelectedImage.SetActive(true);
            uiData.SetAIDifficulty(AIDifficulty.Hard);
        }


        private void ResetImages()
        {
            AIEasySelectedImage.SetActive(false);
            AIHardSelectedImage.SetActive(false);
            AIMediumSelectedImage.SetActive(false);
        }
    }
}
