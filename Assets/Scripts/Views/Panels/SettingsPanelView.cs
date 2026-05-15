using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class SettingsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public SaveDataSO saveData;

        [Header("Tab Objects")]
        public List<GameObject> SelectionImages;
        public List<GameObject> TabPanels;

        [Header("Guide Selection Images")]
        public GameObject GuideSelectedImage;
        public GameObject NoGuideSelectedImage;

        [Header("Force Selection Images")]
        public GameObject LeftSideSelectedImage;
        public GameObject RightSideSelectedImage;


        [Header("Audio Objects")]
        public Slider bgSlider;
        public AudioSource BGAudioSource;


        [Header("Panel Objects")]
        public string currentState;


        private void OnEnable()
        {
            if(userData.myGuide == GuideType.Guide)
            {
                GuideSelectedImage.SetActive(true);
                NoGuideSelectedImage.SetActive(false);
            }
            else
            {
                GuideSelectedImage.SetActive(false);
                NoGuideSelectedImage.SetActive(true);
            }


            if(userData.myDominantHand == DominantHand.Right)
            {
                LeftSideSelectedImage.SetActive(false);
                RightSideSelectedImage.SetActive(true);
            }
            else
            {
                LeftSideSelectedImage.SetActive(true);
                RightSideSelectedImage.SetActive(false);
            }

            bgSlider.value = BGAudioSource.volume;
        }

        public void TabButtonClicked(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetTabs();
            TabPanels[id].SetActive(true);
            SelectionImages[id].SetActive(true);
        }

        private void ResetTabs()
        {
            foreach (var tab in TabPanels)
            {
                tab.SetActive(false);
            }

            foreach (var img in SelectionImages)
            {
                img.SetActive(false);
            }
        }

        public void GuideBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            GuideSelectedImage.SetActive(true);
            NoGuideSelectedImage.SetActive(false);
            userData.SetGuideType(GuideType.Guide);
        }

        public void NoGuideBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            GuideSelectedImage.SetActive(false);
            NoGuideSelectedImage.SetActive(true);
            userData.SetGuideType(GuideType.NoGuide);
        }

        public void RightBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            RightSideSelectedImage.SetActive(true);
            LeftSideSelectedImage.SetActive(false);
            userData.SetDominantHand(DominantHand.Right);
        }
        

        public void LeftBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            RightSideSelectedImage.SetActive(false);
            LeftSideSelectedImage.SetActive(true);
            userData.SetDominantHand(DominantHand.Left);
        }

        public void BGMusicChanged(float val)
        {
            BGAudioSource.volume = val;
        }


        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

    }

}