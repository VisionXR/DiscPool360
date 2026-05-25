
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class HomePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public UserDataSO userData;

        [Header("Selection Objects")]
        public List<GameObject> selectedImages;
        public PanelOnOff internetToastPanel;
        public GameObject tutorialManager;

        [Header("Next Objects")]
        public string singlePlayerState;
        public string multiPlayerState;
        public string tutorialState;
        


        private void OnEnable()
        {
            ResetImages();
            int id = (int)userData.myCoins;
            selectedImages[id].SetActive(true);

            if(id == 0 || id == 1)
            {
                uiData.SetGameMode(GameMode.Pool);
            }
            else
            {
                uiData.SetGameMode(GameMode.Snooker);
            }

            uiData.uiManager.ChangeState("Home", false);
        }

        public void GameModeBtnClicked(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            userData.SetMyCoins(id);
            ResetImages();
            selectedImages[id].SetActive(true);

            if (id == 0 || id == 1)
            {
                uiData.SetGameMode(GameMode.Pool);
            }
            else
            {
                uiData.SetGameMode(GameMode.Snooker);
            }
        }

        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (uiData.currentGameType == GameType.SinglePlayer)
            {
                uiData.uiManager.ChangeState(singlePlayerState, true);
            }
            else if (uiData.currentGameType == GameType.MultiPlayer)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {

                    StartCoroutine(CheckInternetAndProceed());
                }
                else
                {

                    uiData.uiManager.ChangeState(multiPlayerState, true);
                }
            }
            else if (uiData.currentGameType == GameType.Tutorial)
            {
                Debug.Log("Tutorial started");
                tutorialManager.SetActive(true);
                uiData.uiManager.ChangeState(tutorialState, true);
            }
        }

       private IEnumerator CheckInternetAndProceed()
        {
            internetToastPanel.TurnOnPanel();
            yield return new WaitForSeconds(2f);
            internetToastPanel.TurnOffPanel();
          
        }

        private void ResetImages()
        {
            foreach (var item in selectedImages)
            {
                item.SetActive(false);
            }
        }
    }
}
