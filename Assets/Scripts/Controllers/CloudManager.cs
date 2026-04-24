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
        public string userDataKey = "DiscPoolUserData";
      

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
                string jsonString = JsonUtility.ToJson(achievementsData.userData);

                var request = new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string> {
                    { userDataKey, jsonString }
                },

                };

                PlayFabClientAPI.UpdateUserData(request,
                    result => Debug.Log("Cloud Save Successful"),
                    OnDataFetchError);
            }
            catch (Exception e)
            {
            }
        }

        // --- LOAD DATA ---
        public void LoadUserData()
        {
            var request = new GetUserDataRequest
            {
                Keys = new List<string> { userDataKey }
            };

            try
            {
                PlayFabClientAPI.GetUserData(request, result =>
                {
                    if (result.Data != null && result.Data.ContainsKey(userDataKey))
                    {
                        // Convert the JSON string back into your UserData object
                        string json = result.Data[userDataKey].Value;
                        achievementsData.userData = JsonUtility.FromJson<UserData>(json);

                        Debug.Log("Cloud Data Loaded Successfully" + json);
                    }
                    else
                    {
                        Debug.Log("No existing cloud data found for this key. Starting fresh.");
                    }

                    OnDataFetchSuccessEvent?.Invoke();

                }, OnDataFetchError);
            }
            catch (Exception e)
            {

            }
        }

        private void OnDataFetchError(PlayFabError error)
        {
            Debug.Log($"[CloudManager] PlayFab Error: {error.GenerateErrorReport()}");
            OnDataFetchFailureEvent?.Invoke();
        }
    }
}