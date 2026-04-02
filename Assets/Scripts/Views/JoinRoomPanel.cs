using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class JoinRoomPanel : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public DestinationSO destinationData;
        public UIDataSO uiData;

        [Header("game Objects")]
        public Destination multiPlayerDestination;
        public TMP_InputField roomCodeInputField;
        public DestinationPanel destinationPanel;
        public GameObject multiPlayerPanel;


        public void JoinRoomBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            multiPlayerDestination.roomName = roomCodeInputField.text;
            destinationPanel.gameObject.SetActive(true);
            destinationPanel.ConnectToDestination(multiPlayerDestination);
            gameObject.SetActive(false);
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            multiPlayerPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
