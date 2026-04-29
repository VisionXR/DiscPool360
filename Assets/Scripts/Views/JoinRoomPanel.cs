using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
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

        [Header("This Objects")]
        public MultiPlayerPanelView multiPlayerPanelView;
        public List<PanelOnOff> panelsToOff;
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
            multiPlayerPanelView.TurnOn();
            TurnOff();
        }

        public void TurnOff()
        {
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOffPanel();
            }
            StartCoroutine(WaitAndTurnOff());
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }

        public void TurnOn()
        {
            gameObject.SetActive(true);
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOnPanel();
            }
        }
    }
}
