using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class InfoPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public AudioDataSO audioData;
        public UIDataSO uiData;


        [Header("Panel Objects")]
        public string currentState;


        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

        public void ReviewBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            // Replace 'com.YourCompany.YourGameName' with your actual package name
            string playStoreURL = "market://details?id=com.VisionXR.DiscPool360";

            // Fallback URL for testing in the Unity Editor or if the market link fails
            string browserURL = "https://play.google.com/store/apps/details?id=com.VisionXR.DiscPool360";

#if UNITY_ANDROID && !UNITY_EDITOR
        Application.OpenURL(playStoreURL);
#else
            Application.OpenURL(browserURL);
#endif
        }
    }
}
