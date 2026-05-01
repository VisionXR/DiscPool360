using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class ClientDisconnectionPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public GameDataSO gameData;

        public void HomeBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            gameData.ExitGame();
            gameObject.SetActive(false);
        }
    }
}
