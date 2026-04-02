using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class ExitPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public GameDataSO gameData;
        
        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            gameData.ExitGame();
            gameObject.SetActive(false);
        }

        public void ResumeBtnClicked()
        {
           
            audioData.PlayAudio(AudioClipType.ButtonClick);           
            gameObject.SetActive(false);
        }

        
    }
}
