using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class RightScoreNavigationPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;

        [Header("Local Objects")]
        public GameObject SettingsPanel;

        [Header("Sound Sprites")]
        public Image SpeakerImage;
        public Sprite SpeakerOn;
        public Sprite SpeakerOff;


        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.ExitButtonClicked();
        }


        public void SpeakerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (SpeakerImage != null && SpeakerImage.sprite == SpeakerOn)
            {
                SpeakerImage.sprite = SpeakerOff;
                uiData.TurnOffSpeakerEvent?.Invoke();
            }
            else if (SpeakerImage != null && SpeakerImage.sprite == SpeakerOff)
            {
                SpeakerImage.sprite = SpeakerOn;
                uiData.TurnOnSpeakerEvent?.Invoke();
            }
        }


        public void SettingsBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (SettingsPanel != null)
            {
                SettingsPanel.SetActive(true);
            }
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            SettingsPanel.SetActive(false);
        }
    }
}
