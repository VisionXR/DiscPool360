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

        [Header("Next And Previous Panels")]
        public string currentState;

        public void HomeBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            gameData.ExitGame();
            uiData.uiManager.ChangeState("GameType", false);
            uiData.uiManager.ChangeState(currentState, false);
            uiData.uiManager.ResetAllBools();
        }
    }
}
