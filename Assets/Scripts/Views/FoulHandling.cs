using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class FoulHandling : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIDataSO uiData;
        public AudioDataSO audioData;
        public StrikerDataSO strikerData;

        [Header("Local Objects")]
        public GameObject foulPanel;

        private void OnEnable()
        {
            uiData.ShowFoulHandlingEvent += ShowFoulPanel;
            strikerData.FoulCompleteEvent += HideFoulPanel;
        }

        private void OnDisable()
        {
            uiData.ShowFoulHandlingEvent -= ShowFoulPanel;
            strikerData.FoulCompleteEvent -= HideFoulPanel;
        }

        private void ShowFoulPanel()
        {
            foulPanel.SetActive(true);
        }

        private void HideFoulPanel()
        {
            foulPanel.SetActive(false); 
        }

        public void CorrectBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
        }
    }
}
