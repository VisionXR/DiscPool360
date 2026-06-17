using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class LoginPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;



        public void GuestLoginBtnClicked()
        {
            audioData.PlayAudio(HelperClasses.AudioClipType.ButtonClick);
        }

        public void GoogleLoginBtnClicked()
        {
            audioData.PlayAudio(HelperClasses.AudioClipType.ButtonClick);
        }

    }
}
