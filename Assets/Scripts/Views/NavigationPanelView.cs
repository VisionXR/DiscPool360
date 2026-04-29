using com.VisionXR.Controllers;
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
        public PanelOnOff GamesPanel;
        public PanelOnOff SettingsPanel;
        public PanelOnOff RulesPanel;
        public PanelOnOff LeaderBoardPanel;
        public PanelOnOff PurchasePanel;
        public PanelOnOff AchievementPanel;
    

        [Header("BGImages")]
        public Image HomeSelectionImage;
        public Image SettingsSelectionImage;
        public Image RulesSelectionImage;
        public Image LeaderBoardSelectionImage;
        public Image PurchaseSelectionImage;
        public Image AchievementSelectionImage;
        public Image RatingSelectionImage;


        private void OnEnable()
        {
            ResetImages();
            ResetPanels();
            GamesPanel.TurnOnPanel();
            HomeSelectionImage.gameObject.SetActive(true);
        }


        public void HomePanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            GamesPanel.TurnOnPanel();
            HomeSelectionImage.gameObject.SetActive(true);
        }

        public void SettingsPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            SettingsPanel.TurnOnPanel();
            SettingsSelectionImage.gameObject.SetActive(true);
        }

        public void RulesPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            RulesPanel.TurnOnPanel();
            RulesSelectionImage.gameObject.SetActive(true);
        }

        public void LeaderBoardPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            LeaderBoardPanel.TurnOnPanel();
            LeaderBoardSelectionImage.gameObject.SetActive(true);
        }

        public void AchievementPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            AchievementPanel.TurnOnPanel();
            AchievementSelectionImage.gameObject.SetActive(true);
        }




        public void PurchasePanelClicked()
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
            RulesPanel.TurnOffPanel();
            LeaderBoardPanel.TurnOffPanel();
            PurchasePanel.TurnOffPanel();
            AchievementPanel.TurnOffPanel();
            GamesPanel.TurnOffPanel();


        }

        private void ResetImages()
        {
            HomeSelectionImage.gameObject.SetActive(false);
            SettingsSelectionImage.gameObject.SetActive(false);
            RulesSelectionImage.gameObject.SetActive(false);
            LeaderBoardSelectionImage.gameObject.SetActive(false);
            PurchaseSelectionImage.gameObject.SetActive(false);
            AchievementSelectionImage.gameObject.SetActive(false);
            RatingSelectionImage.gameObject.SetActive(false);
        }
    }

}