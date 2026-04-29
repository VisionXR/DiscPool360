using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class NavigationPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public AppPropertiesDataSO appProperties;


        [Header("Panels")] 
        public PanelOnOff SettingsPanel;
        public PanelOnOff LeaderBoardPanel;
        public PanelOnOff PurchasePanel;
        public PanelOnOff AchievementPanel;
    

        [Header("BGImages")]
        public Image SettingsSelectionImage;
        public Image LeaderBoardSelectionImage;
        public Image PurchaseSelectionImage;
        public Image AchievementSelectionImage;
        


        private void OnEnable()
        {
            ResetImages();
            ResetPanels();
        }


        public void SettingsBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            SettingsPanel.TurnOnPanel();
            SettingsSelectionImage.gameObject.SetActive(true);
        }

        public void LeaderBoardBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            LeaderBoardPanel.TurnOnPanel();
            LeaderBoardSelectionImage.gameObject.SetActive(true);
        }

        public void AchievementBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            AchievementPanel.TurnOnPanel();
            AchievementSelectionImage.gameObject.SetActive(true);
        }

        public void PurchaseBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            PurchasePanel.TurnOnPanel();
            PurchaseSelectionImage.gameObject.SetActive(true);
        }


        private void ResetPanels()
        {    
            SettingsPanel.TurnOffPanel();
            LeaderBoardPanel.TurnOffPanel();
            PurchasePanel.TurnOffPanel();
            AchievementPanel.TurnOffPanel();
        }

        private void ResetImages()
        {
            
            SettingsSelectionImage.gameObject.SetActive(false);
            LeaderBoardSelectionImage.gameObject.SetActive(false);
            PurchaseSelectionImage.gameObject.SetActive(false);
            AchievementSelectionImage.gameObject.SetActive(false);
        }
    }

}