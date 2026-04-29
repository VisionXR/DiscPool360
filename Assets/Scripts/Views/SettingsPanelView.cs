using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
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
        public SaveAndLoadManager saveAndLoadManager;

        [Header("Game Objects")]
        public AudioSource BGAudioSource;
        public TMP_Text playerNameText;
        public Image playerImage;
        public Slider SoundsSlider;
        public Slider BGMusicSlider;

        [Header("This Objects")]
        public HomePanelView homePanelView;
        public List<PanelOnOff> panelsToOff;


        private void OnEnable()
        {
            playerNameText.text = userData.MyName;
            playerImage.sprite = userData.MyProfileImage;

            PlayerSettings settings = saveAndLoadManager.LoadSettings();
            if (settings != null)
            {
                
                BGMusicSlider.value = settings.musicVolume;      
               
            }
        }

        public void BGMusicChanged(float val)
        {
            BGAudioSource.volume = val;

            PlayerSettings settings = new PlayerSettings
            {
                musicVolume = val,
                dominantHand = userData.myDominantHand,
            };

            saveAndLoadManager.SaveSettings(settings);
        }

        public void DominantHandChanged(float val)
        {
            userData.SetDominantHand((DominantHand)val);

            PlayerSettings settings = new PlayerSettings
            {
                musicVolume = BGAudioSource.volume,
                dominantHand = userData.myDominantHand,
            };

            saveAndLoadManager.SaveSettings(settings);
        }

        public void BackBtnClicked()
        {
           audioData.PlayAudio(AudioClipType.ButtonClick);
           TurnOff();
            homePanelView.TurnOn();
        }

        public void TurnOff()
        {
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOffPanel();
            }
            StartCoroutine(WaitAndTurnOff());
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }


        public void TurnOn()
        {
            gameObject.SetActive(true);
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOnPanel();
            }
        }
    }

}