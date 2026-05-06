
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class HomePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public UserDataSO userData;


        [Header("Next And Previous Panels")]
        public string gameTypeState;
        public string quitAppState;


        public void QuitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);        
            uiData.uiManager.ChangeState(quitAppState, true);
        }

        public void EightPoolBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameMode(GameMode.Pool);
            userData.myCoins = 0;

            uiData.uiManager.ChangeState(gameTypeState, true);
            
        }

        public void FivePoolBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameMode(GameMode.Pool);
            userData.myCoins = 1;

            uiData.uiManager.ChangeState(gameTypeState, true);
        }

        public void SixSnookerBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameMode(GameMode.Pool);
            userData.myCoins = 2;

            uiData.uiManager.ChangeState(gameTypeState, true);
        }

        public void TenSnookerBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameMode(GameMode.Pool);
            userData.myCoins = 3;

            uiData.uiManager.ChangeState(gameTypeState, true);
        }

        public void CollorChallengeBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameMode(GameMode.Pool);
            userData.myCoins = 4;

            uiData.uiManager.ChangeState(gameTypeState, true);
        }


        public void TutorialBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
        }

        public void QuitBtnClciked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
        }
    }
}
