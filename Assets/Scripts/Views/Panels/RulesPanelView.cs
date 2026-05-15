using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class RulesPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public AudioDataSO audioData;
        public UIDataSO uiData;

        [Header("Tab Objects")]
        public List<GameObject> SelectionImages;
        public List<GameObject> TabPanels;


        [Header("Panel Objects")]
        public string currentState;


        public void TabButtonClicked(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetTabs();
            TabPanels[id].SetActive(true);
            SelectionImages[id].SetActive(true);
        }

        private void ResetTabs()
        {
            foreach (var tab in TabPanels)
            {
                tab.SetActive(false);
            }

            foreach (var img in SelectionImages)
            {
                img.SetActive(false);
            }
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }
    }
}
