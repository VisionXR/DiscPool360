using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.VisionXR.Views
{
    public class DestinationPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public DestinationSO destinationData;
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public GameDataSO gameData;
        public UserDataSO userData;

        [Header("Local Objects")]
        public TMP_Text connectionText;
        public GameObject rotationImage;
        public GameObject HomeBtn;
        public GameObject RetryBtn;

        [Header("Icons")]
        public Sprite EightPoolIcon;
        public Sprite FivePoolIcon;
        public Sprite TenSnookerIcon;
        public Sprite SixSnookerIcon;
        public Sprite ColorChallengeIcon;

        [Header("Rotation Settings")]
        public float rotationSpeed = 360f; // Degrees per second

        private Coroutine connectionRoutine = null;
        private Coroutine rotationRoutine = null;

        // local actions
        private Action DestinationSuccessEvent;
        private Action<string> DestinationFailureEvent;
        public Destination currentDestination;



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

            if(userData.myCoins == CoinsType.EightPool)
            {
                rotationImage.GetComponent<Image>().sprite = EightPoolIcon;
            }
            else if (userData.myCoins == CoinsType.FivePool)
            {
                rotationImage.GetComponent<Image>().sprite = FivePoolIcon;
            }
            else if (userData.myCoins == CoinsType.TenSnooker)
            {
                rotationImage.GetComponent<Image>().sprite = TenSnookerIcon;
            }

            else if (userData.myCoins == CoinsType.SixSnooker)
            {
                rotationImage.GetComponent<Image>().sprite = SixSnookerIcon;
            }
            else if (userData.myCoins == CoinsType.ColorChallenge)
            {
                rotationImage.GetComponent<Image>().sprite = ColorChallengeIcon;
            }


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
                StopCoroutine(rotationRoutine);
                rotationImage.transform.localRotation = Quaternion.identity;    
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
                StopCoroutine(rotationRoutine);
                rotationImage.transform.localRotation = Quaternion.identity;
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
                    rotationRoutine = StartCoroutine(RotateImage());
                }
               
                destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailureEvent);

            }

        }

        public void HomeBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomeBtn.SetActive(false);
            RetryBtn.SetActive(false);

            gameData.ExitGame();
            uiData.uiManager.ChangeState("GameType", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();
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

        private IEnumerator RotateImage()
        {
            while (true)
            {
                rotationImage.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}

