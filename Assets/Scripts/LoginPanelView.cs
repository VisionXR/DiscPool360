using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class LoginPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;

        public AuthManager authManager;

        public void GuestLoginBtnClicked()
        {
            audioData.PlayAudio(HelperClasses.AudioClipType.ButtonClick);
            authManager.GuestLogin();
        }

        public void GoogleLoginBtnClicked()
        {
            audioData.PlayAudio(HelperClasses.AudioClipType.ButtonClick);
            authManager.Login();
            
        }

    }
}
