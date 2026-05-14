using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class MultiPlayerPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;


        [Header("Selection Objects")]
        public GameObject CreateSelectedImage;
        public GameObject JoinSelectedImage;


        [Header("Next And Previous Panels")]
        public string currentState;
        public string createRoomState;
        public string joinRoomState;

        private void OnEnable()
        {
            ResteImages();
            if (uiData.currentLobbyType == LobbyType.Create)
            {
                
                CreateSelectedImage.SetActive(true);
            }
            else if (uiData.currentLobbyType == LobbyType.Join)
            {
            
                JoinSelectedImage.SetActive(true);
            }
        }

        public void CreateBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResteImages();
            CreateSelectedImage.SetActive(true);
            uiData.SetLobbyType(LobbyType.Create);
        }

        public void JoinBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResteImages();
            JoinSelectedImage.SetActive(true);
            uiData.SetLobbyType(LobbyType.Join);
        }



        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            
        }

        private void ResteImages()
        {
                       CreateSelectedImage.SetActive(false);
            JoinSelectedImage.SetActive(false);

        }

    }
}
