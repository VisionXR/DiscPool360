
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class HomePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
     

        private void OnEnable()
        {

          
            
        }


        public void SinglePlayerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameType(GameType.SinglePlayer);        
           
            
           
        }

        public void MultiPlayerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                uiData.SetGameType(GameType.MultiPlayer);           
              
            }
            else
            {
                // No internet check
            }
        }


    }
}
