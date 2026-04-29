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
        public HomePanelView homePanelView;
        public PanelOnOff SettingsPanel;
        public PanelOnOff LeaderBoardPanel;
        public PanelOnOff PurchasePanel;
        public PanelOnOff AchievementPanel;
    

        

        public void SettingsBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
           
            SettingsPanel.TurnOnPanel();
            homePanelView.TurnOff();
           
        }

        public void LeaderBoardBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
            LeaderBoardPanel.TurnOnPanel();
            homePanelView.TurnOff();

        }

        public void AchievementBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          

            AchievementPanel.TurnOnPanel();
            homePanelView.TurnOff();

        }

        public void PurchaseBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
         

            PurchasePanel.TurnOnPanel();
            homePanelView.TurnOff();

        }

    }

}