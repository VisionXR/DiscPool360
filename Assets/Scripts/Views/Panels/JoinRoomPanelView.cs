using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class JoinRoomPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public DestinationSO destinationData;
        public UIDataSO uiData;

        [Header("game Objects")]
        public Destination multiPlayerDestination;
        public TMP_InputField roomCodeInputField;
        public DestinationPanelView destinationPanelView;
       



        [Header("Next And Previous Panels")]
        public string destinationState;
        public string currentState;


        public void JoinRoomBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            multiPlayerDestination.roomName = roomCodeInputField.text;
            multiPlayerDestination.gameMode = uiData.currentGameMode;

            destinationPanelView.SetDestination(multiPlayerDestination);
            uiData.uiManager.ChangeState(destinationState, true);

        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

    }
}
