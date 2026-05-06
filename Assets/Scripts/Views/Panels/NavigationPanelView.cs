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


        [Header("Next Panel bools")]
        public string settingsPanelstring;
        public string leaderboardPanelstring;
        public string achievementPanelstring;
        public string purchasePanelstring;
        public string rulePanelstring;
    

        

        public void SettingsBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
           
           
        }

        public void LeaderBoardBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
         
        }

        public void AchievementBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
        }

        public void PurchaseBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
        

        }

        public void RulesBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);


        }

    }

}