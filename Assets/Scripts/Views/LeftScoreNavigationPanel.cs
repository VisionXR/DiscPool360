using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
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
        public GameDataSO gameData;

        [Header("Came View Objects")]
        public CamPositionManager camPositionManager;
        public Image LeftCameraViewImage;
        public Image RightCameraViewImage;
        public Sprite FrontView;
        public Sprite TopView;
        private bool isFrontView = false;

        [Header("Next And Previous Panels")]
        public GameObject RotationObject;
        public GameObject AimObject;
        public string PauseState;

        // local
        private Coroutine controlsRoutine;
        public void ExitBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(PauseState, true);
            gameData.GamePaused();
        }

        public void CameraBtnClicked()
        {
                  
            if(isFrontView)
            {
                LeftCameraViewImage.sprite = FrontView;
                RightCameraViewImage.sprite = FrontView;
                TopViewBtnClicked();
                isFrontView = false;
            }
            else
            {
                LeftCameraViewImage.sprite = TopView;
                RightCameraViewImage.sprite = TopView;
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

        public void ControlsBtnClicked()
        {         
            if(controlsRoutine == null)
            {
                audioData.PlayAudio(AudioClipType.ButtonClick);
                controlsRoutine = StartCoroutine(ShowControls());
            }
        }


        private IEnumerator ShowControls()
        {
            RotationObject.SetActive(true);
            AimObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            RotationObject.SetActive(false);
            AimObject.SetActive(false);
            controlsRoutine = null;
        }
    }
}
