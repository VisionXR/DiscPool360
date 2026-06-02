using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic; // Added for Dictionary
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

        private void OnEnable()
        {
            cloudData.LoadPlayerDataEvent += LoadPlayerData;
            cloudData.SavePlayerDataEvent += SaveUserData;
        }

        private void OnDisable()
        {
            cloudData.LoadPlayerDataEvent -= LoadPlayerData;
            cloudData.SavePlayerDataEvent -= SaveUserData;
        }

        /// <summary>
        /// Fetches both Inventory (Coins) and Custom User Data (DiscPoolUserData)
        /// </summary>
        public void LoadPlayerData(Action OnSuccess, Action OnFailure)
        {
            OnDataFetchSuccessEvent = OnSuccess;
            OnDataFetchFailureEvent = OnFailure;

            // 2. Immediately after Inventory, Get Custom User Data
            LoadUserData();

        }

        // --- SAVE DATA ---
        public void SaveUserData()
        {
            try
            {

                // Convert your UserData class to JSON string
                string jsonString = JsonUtility.ToJson(achievementsData.defaultBoardWinsData);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> {
                    { defaultBoardsWinsKey, jsonString }
                },

                };

                PlayFabClientAPI.UpdateUserData(request,
                    result => { },
                    OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error saving user data: " + e.Message); 
            }

            try
            {

                // Convert your UserData class to JSON string
                string jsonString = JsonUtility.ToJson(achievementsData.specialBoardWinsStats);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> {
                    { specialBoardWinsKey, jsonString }
                },

                };

                PlayFabClientAPI.UpdateUserData(request,
                    result => { },
                    OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error saving board stats data: " + e.Message);
            }
        }

        // --- LOAD DATA ---
        public void LoadUserData()
        {
            Debug.Log("Trying to load");
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
                        // Convert the JSON string back into your UserData object
                        string json = result.Data[defaultBoardsWinsKey].Value;
                        achievementsData.defaultBoardWinsData = JsonUtility.FromJson<DefaultBoardWinsData>(json);

                        Debug.Log("user Data Loaded Successfully"+json);
                    }

                    if (result.Data != null && result.Data.ContainsKey(specialBoardWinsKey))
                    {
                        // Convert the JSON string back into your BoardWinsStats object
                        string json = result.Data[specialBoardWinsKey].Value;
                        achievementsData.specialBoardWinsStats = JsonUtility.FromJson<SpecialBoardWinsStats>(json);

                        Debug.Log("board stats Data Loaded Successfully"+json);
                    }


                    achievementsData.UserLoggedIn();
                    OnDataFetchSuccessEvent?.Invoke();

                }, OnDataFetchError);
            }
            catch (Exception e)
            {
                Debug.Log("Error loading data " + e.Message);
            }
        }

        private void OnDataFetchError(PlayFabError error)
        {
            Debug.Log($"[CloudManager] PlayFab Error: {error.GenerateErrorReport()}");
            OnDataFetchFailureEvent?.Invoke();
        }
    }
}