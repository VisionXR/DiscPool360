
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
        public string currentState;


        private void OnEnable()
        {
            ResetImages();
            int id = (int)userData.myCoins;
            selectedImages[id].SetActive(true);
        }

        public void GameModeBtnClicked(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            userData.SetMyCoins(id);
            ResetImages();
            selectedImages[id].SetActive(true);
        }

        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);


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
