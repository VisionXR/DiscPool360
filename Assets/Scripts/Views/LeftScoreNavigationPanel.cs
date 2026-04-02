using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class LeftScoreNavigationPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;

        [Header("Local Objects")]
        public GameObject RulesPanel;

        [Header("Sound Sprites")]
        public Image MicImage;
        public Sprite MicOn;
        public Sprite MicOff;



        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.ExitButtonClicked();
        }

        public void MicBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (MicImage != null && MicImage.sprite == MicOn)
            {
                MicImage.sprite = MicOff;
                uiData.TurnOffMicEvent?.Invoke();
            }
            else if (MicImage != null && MicImage.sprite == MicOff)
            {
                MicImage.sprite = MicOn;
                uiData.TurnOnMicEvent?.Invoke();
            }
        }

        public void RulesBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (RulesPanel != null)
            {
                RulesPanel.SetActive(true);
            }
        }

        public void RulesBackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            RulesPanel.SetActive(false);
        }
    }
}
