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
        public GameObject HomePanel;
        public GameObject SettingsPanel;
        public GameObject RulesPanel;
        public GameObject LeaderBoardPanel;
        public GameObject PurchasePanel;
        public GameObject AchievementPanel;
        public GameObject RatingPanel;

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
            HomePanel.SetActive(true);
            HomeSelectionImage.gameObject.SetActive(true);
        }


        public void HomePanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            HomePanel.SetActive(true);
            HomeSelectionImage.gameObject.SetActive(true);
        }

        public void SettingsPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            SettingsPanel.SetActive(true);
            SettingsSelectionImage.gameObject.SetActive(true);
        }

        public void RulesPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            RulesPanel.SetActive(true);
            RulesSelectionImage.gameObject.SetActive(true);
        }

        public void LeaderBoardPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            LeaderBoardPanel.SetActive(true);
            LeaderBoardSelectionImage.gameObject.SetActive(true);
        }

        public void AchievementPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            AchievementPanel.SetActive(true);
            AchievementSelectionImage.gameObject.SetActive(true);
        }

        public void RatingPanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            RatingPanel.SetActive(true);
            RatingSelectionImage.gameObject.SetActive(true);
        }


        public void PurchasePanelClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetImages();
            ResetPanels();
            PurchasePanel.SetActive(true);
            PurchaseSelectionImage.gameObject.SetActive(true);
        }


        private void ResetPanels()
        {

            HomePanel.SetActive(false);
            SettingsPanel.SetActive(false);
            RulesPanel.SetActive(false);
            LeaderBoardPanel.SetActive(false);
            PurchasePanel.SetActive(false);
            AchievementPanel.SetActive(false);
            RatingPanel.SetActive(false);
   
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