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
        public UIDataSO uiData;
        public AppPropertiesDataSO appProperties;


        [Header("Next Panel bools")]
        public string settingsPanelState;
        public string leaderboardPanelState;
        public string achievementPanelState;
        public string purchasePanelState;
        public string rulePanelState;
    

        

        public void SettingsBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(settingsPanelState,true);
           
        }

        public void LeaderBoardBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(leaderboardPanelState, true);

        }

        public void AchievementBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(achievementPanelState, true);

        }

        public void PurchaseBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(purchasePanelState, true);

        }

        public void RulesBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(rulePanelState, true);

        }

    }

}