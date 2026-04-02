using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class SettingsPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public SaveAndLoadManager saveAndLoadManager;

        [Header("Game Objects")]
        public AudioSource BGAudioSource;
        public TMP_Text playerNameText;
        public Image playerImage;
        public Slider DominantHandSlider;
        public Slider BGMusicSlider;


        private void OnEnable()
        {
            playerNameText.text = userData.MyName;
            playerImage.sprite = userData.MyProfileImage;

            PlayerSettings settings = saveAndLoadManager.LoadSettings();
            if (settings != null)
            {
                
                BGMusicSlider.value = settings.musicVolume;      
                DominantHandSlider.value = (float)settings.dominantHand;
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
    }

}