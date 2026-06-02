using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections; // Required for Coroutines
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CloudManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public CloudDataSO cloudData;
        public AchievementsDataSO achievementsData;

        [Header("Keys")]
        public string defaultBoardsWinsKey = "CarromPoolDefaultBoardsData";
        public string specialBoardWinsKey = "CarromPoolSpecialBoardsData";

        // Actions
        private Action OnDataFetchSuccessEvent;
        private Action OnDataFetchFailureEvent;

        private Coroutine loadTimeoutCoroutine;

        private void OnEnable()
        {
            cloudData.LoadPlayerDataEvent += LoadPlayerData;
            cloudData.SavePlayerDataEvent += SaveUserData;
        }

        private void OnDisable()
        {
            cloudData.LoadPlayerDataEvent -= LoadPlayerData;
            cloudData.SavePlayerDataEvent -= SaveUserData;

            // Safety check to prevent memory leaks if destroyed during loading
            if (loadTimeoutCoroutine != null) StopCoroutine(loadTimeoutCoroutine);
        }

        /// <summary>
        /// Fetches both Inventory (Coins) and Custom User Data (DiscPoolUserData)
        /// </summary>
        public void LoadPlayerData(Action OnSuccess, Action OnFailure)
        {
            OnDataFetchSuccessEvent = OnSuccess;
            OnDataFetchFailureEvent = OnFailure;

            // Reset the load state before starting
            cloudData.isPlayerDataLoaded = false;

            // Start the 5-second timeout coroutine
            if (loadTimeoutCoroutine != null) StopCoroutine(loadTimeoutCoroutine);
            loadTimeoutCoroutine = StartCoroutine(LoadPlayerDataRoutine(5f));
        }

        /// <summary>
        /// Coroutine that waits for data to load or times out after specified seconds.
        /// </summary>
        private IEnumerator LoadPlayerDataRoutine(float timeoutDuration)
        {
            float elapsed = 0f;

            // Loop until the data is loaded OR we hit the timeout limit
            while (!cloudData.isPlayerDataLoaded)
            {
                LoadUserData();
                elapsed += Time.deltaTime;
                yield return new WaitForSeconds(timeoutDuration); // Wait for the next frame
            }


         
            OnDataFetchSuccessEvent?.Invoke();
            loadTimeoutCoroutine = null;
        }

        // --- SAVE DATA ---
        public void SaveUserData()
        {
            try
            {
                string jsonString = JsonUtility.ToJson(achievementsData.defaultBoardWinsData);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> {
                        { defaultBoardsWinsKey, jsonString }
                    }
                };

                PlayFabClientAPI.UpdateUserData(request, result => { }, OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error saving user data: " + e.Message);
            }

            try
            {
                string jsonString = JsonUtility.ToJson(achievementsData.specialBoardWinsStats);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> {
                        { specialBoardWinsKey, jsonString }
                    }
                };

                PlayFabClientAPI.UpdateUserData(request, result => { }, OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error saving board stats data: " + e.Message);
            }
        }

        // --- LOAD DATA ---
        public void LoadUserData()
        {
           
            var request = new GetUserDataRequest
            {
                Keys = new List<string> { defaultBoardsWinsKey, specialBoardWinsKey }
            };

            try
            {
                PlayFabClientAPI.GetUserData(request, result =>
                {
                    if (result.Data != null && result.Data.ContainsKey(defaultBoardsWinsKey))
                    {
                        string json = result.Data[defaultBoardsWinsKey].Value;
                        achievementsData.defaultBoardWinsData = JsonUtility.FromJson<DefaultBoardWinsData>(json);
                        
                    }

                    if (result.Data != null && result.Data.ContainsKey(specialBoardWinsKey))
                    {
                        string json = result.Data[specialBoardWinsKey].Value;
                        achievementsData.specialBoardWinsStats = JsonUtility.FromJson<SpecialBoardWinsStats>(json);
                        
                    }

                    achievementsData.UserLoggedIn();

                    // Critical: Setting this triggers the coroutine while-loop to stop early!
                    cloudData.isPlayerDataLoaded = true;

                }, OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error loading data " + e.Message);
            }
        }

        private void OnDataFetchError(PlayFabError error)
        {
            // If PlayFab explicitly returns an error, stop the coroutine immediately
            if (loadTimeoutCoroutine != null)
            {
                StopCoroutine(loadTimeoutCoroutine);
                loadTimeoutCoroutine = null;
            }

            OnDataFetchFailureEvent?.Invoke();
        }
    }
}