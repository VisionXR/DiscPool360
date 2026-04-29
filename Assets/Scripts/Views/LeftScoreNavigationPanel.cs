using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class LeftScoreNavigationPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;

        [Header("Came View Objects")]
        public CamPositionManager camPositionManager;
        public Image CameraViewImage;
        public Sprite FrontView;
        public Sprite TopView;


        private bool isFrontView = true;
        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.ExitButtonClicked();
        }

        public void CameraBtnClicked()
        {
          
            
            if(isFrontView)
            {
                CameraViewImage.sprite = FrontView;
                TopViewBtnClicked();
                isFrontView = false;
            }
            else
            {
                CameraViewImage.sprite = TopView;
                FrontViewBtnClicked();
                isFrontView = true;
            }
        }


        public void TopViewBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            Player  mp = playerData.GetMainPlayer();
            camPositionManager.SetTopCamProperties(mp.playerProperties.myId);

        }

        public void FrontViewBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            Player mp = playerData.GetMainPlayer();
            camPositionManager.SetFrontCamProperties(mp.playerProperties.myId);
        }


    }
}
