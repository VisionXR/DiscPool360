using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class DeleteAccountPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UserDataSO userData;

        [Header("Loca Objects")]
        public PanelOnOff deleteAccountPanel;
        public void DeleteAccount()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            userData.DeleteAccount();
            deleteAccountPanel.TurnOffPanel();
        }
    }
}
