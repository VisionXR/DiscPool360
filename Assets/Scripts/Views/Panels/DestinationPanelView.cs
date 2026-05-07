using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class DestinationPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public DestinationSO destinationData;
        public AudioDataSO audioData;
        public UIDataSO uiData;

        [Header("Local Objects")]
        public TMP_Text connectionText;
        public GameObject rotationImage;
        public GameObject HomeBtn;
        public GameObject RetryBtn;

        // local actions
        private Action DestinationSuccessEvent;
        private Action<string> DestinationFailureEvent;
        public Destination currentDestination;

        private Coroutine connectionRoutine = null;


        [Header("Next And Previous Panels")]
        public string singlePlayerState;
        public string multiPlayerState;
        public string currentState;
        public string lobbyState;


        private void OnEnable()
        {
            DestinationSuccessEvent += OnSuccess;
            DestinationFailureEvent += OnFailure;

            HomeBtn.SetActive(false);
            RetryBtn.SetActive(false);

            StartCoroutine(WaitAndConnect());
        }

        private void OnDisable()
        {
            DestinationSuccessEvent -= OnSuccess;
            DestinationFailureEvent -= OnFailure;
        }


        private IEnumerator WaitAndConnect()
        {
          
            yield return new WaitForSeconds(0.5f);
            ConnectToDestination();
        }


        private void OnSuccess()
        {
            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                connectionRoutine = null;
            }

            connectionText.text = "Connected";
            uiData.uiManager.ChangeState(lobbyState, true);      
        }

        private void OnFailure(string msg)
        {
            if (connectionRoutine != null)
            {
                StopCoroutine(connectionRoutine);
                connectionRoutine = null;
            }

            HomeBtn.SetActive(true);
            RetryBtn.SetActive(true);
        }


        public void SetDestination(Destination destination)
        {
            
            currentDestination = destination;   
       
        }


        public void ConnectToDestination()
        {
            if (currentDestination != null)
            {
              
          
                if (connectionRoutine == null)
                {
                    connectionRoutine = StartCoroutine(ShowConnectionStatus());
                }
               
                destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailureEvent);

            }

        }

        public void HomeBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomeBtn.SetActive(false);
            RetryBtn.SetActive(false);

            uiData.TriggerHomeEvent();
        }

        public void RetryBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomeBtn.SetActive(false);
            RetryBtn.SetActive(false);

            ConnectToDestination();
        }


        private IEnumerator ShowConnectionStatus()
        {
            while (true)
            {
                yield return null;
                connectionText.text = "Connecting to destination";
                yield return new WaitForSeconds(0.2f);
                connectionText.text = "Connecting to destination..";
                yield return new WaitForSeconds(0.2f);
                connectionText.text = "Connecting to destination....";
                yield return new WaitForSeconds(0.2f);
                connectionText.text = "Connecting to destination..";
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
