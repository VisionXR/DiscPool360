using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Globalization;
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
        public GameObject LinkExpiredBtn;
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
            LinkExpiredBtn.SetActive(false);

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
          
            yield return new WaitForSeconds(uiData.disableTime);
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
               
                string time = currentDestination.time;

                if (string.IsNullOrEmpty(time))
                {
                    if (connectionRoutine == null)
                    {
                        connectionRoutine = StartCoroutine(ShowConnectionStatus());
                        rotationRoutine = StartCoroutine(RotateImage());
                    }
                    destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailureEvent);
                }
                else
                {
                    try
                    {
                        // 1. Parse the string using the exact pattern you saved it with
                        // CultureInfo.InvariantCulture prevents issues with regional device formats
                        DateTime linkTime = DateTime.ParseExact(time, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                        // 2. Get the current UTC time to compare against
                        DateTime currentTime = DateTime.UtcNow;

                        // 3. Calculate the difference
                        TimeSpan timeDifference = currentTime - linkTime;

                        // 4. Check if the link is older than 15 minutes OR if the link time is somehow in the future
                        if (timeDifference.TotalMinutes > 15 || timeDifference.TotalMinutes < 0)
                        {
                            Debug.LogWarning($"Link expired! It was created {timeDifference.TotalMinutes:F1} minutes ago.");
                            connectionText.text = " Link Expired ...";
                            LinkExpiredBtn.SetActive(true);
                            // TODO: Call your UI manager here to show an "expired link" pop-up screen
                            // uiData.uiManager.ShowPopup("This invite link has expired. Please ask for a new one.");
                        }
                        else
                        {
                            if (connectionRoutine == null)
                            {
                                connectionRoutine = StartCoroutine(ShowConnectionStatus());
                                rotationRoutine = StartCoroutine(RotateImage());
                            }
                            // The link is valid and within the 15-minute window!
                            Debug.Log($"Link is valid. Only {timeDifference.TotalMinutes:F1} minutes old. Connecting...");
                            destinationData.ConnectToDestination(currentDestination, DestinationSuccessEvent, DestinationFailureEvent);
                        }
                    }
                    catch (FormatException)
                    {
                        // If someone tampered with the URL parameter and it's no longer a valid date string
                        Debug.LogError($"Invalid time format in URL: '{time}'. Failed to parse.");
                        // Treat as expired/invalid link
                    }
                }

            }

        }

        public void HomeBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomeBtn.SetActive(false);
            RetryBtn.SetActive(false);
            LinkExpiredBtn.SetActive(false);

            gameData.ExitGame();
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();
        }

        public void RetryBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            HomeBtn.SetActive(false);
            RetryBtn.SetActive(false);
            LinkExpiredBtn.SetActive(false);

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

