
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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


        [Header("Next Objects")]
        public string singlePlayerState;
        public string multiPlayerState;
        


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
            if(uiData.currentGameType == GameType.SinglePlayer)
            {
                uiData.uiManager.ChangeState(singlePlayerState, true);
            }
            else if (uiData.currentGameType == GameType.MultiPlayer)
            {
                uiData.uiManager.ChangeState(multiPlayerState, true);
            }

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
