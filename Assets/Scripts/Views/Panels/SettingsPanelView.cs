using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
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

        [Header("Game Objects")]
        public AudioSource BGAudioSource;
        public TMP_Text playerNameText;
        public Image playerImage;
        public Slider SoundsSlider;
        public Slider BGMusicSlider;

        [Header("Panel Objects")]
        public string currentState;


        private void OnEnable()
        {
            //playerNameText.text = userData.MyName;
            //playerImage.sprite = userData.MyProfileImage;

            //PlayerSettings settings = saveAndLoadManager.LoadSettings();
            //if (settings != null)
            //{
                
            //    BGMusicSlider.value = settings.musicVolume;      
               
            //}
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

        public void BGMusicChanged(float val)
        {
            BGAudioSource.volume = val;

            PlayerSettings settings = new PlayerSettings
            {
                musicVolume = val,
               
            };

           saveData.SaveSettings(settings);
        }


        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

    }

}