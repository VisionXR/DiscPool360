using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



namespace com.VisionXR.Views
{
    public class GameTypePanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public UserDataSO userData;


        [Header("Selection Objects")]
        public List<GameObject> selectedImages;


        private void OnEnable()
        {
            ResetImages();
            if(uiData.currentGameType == GameType.SinglePlayer)
            {
                selectedImages[0].SetActive(true);
            }
            else if (uiData.currentGameType == GameType.MultiPlayer)
            {
                selectedImages[1].SetActive(true);
            }
            else if (uiData.currentGameType == GameType.Tutorial)
            {
                selectedImages[2].SetActive(true);
            }

        }


        public void GameTypeBtnClicked(int id)
        {
            ResetImages();
            selectedImages[id].SetActive(true);
            audioData.PlayAudio(AudioClipType.ButtonClick);
            if (id == 0)
            {
                uiData.SetGameType(GameType.SinglePlayer);
            }
            else if(id == 1)
            {
                uiData.SetGameType(GameType.MultiPlayer);
            }
            else if (id == 2)
            {
                uiData.SetGameType(GameType.Tutorial);
            }

        }



        private void ResetImages()
        {
            foreach (var image in selectedImages)
            {
                image.SetActive(false);
            }
        }

    }
}
