using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CloudDataSO", menuName = "ScriptableObjects/CloudDataSO", order = 1)]    
    public class CloudDataSO : ScriptableObject   
    {
        // variables
       public bool isPlayerDataLoaded = false;


        public Action PlayFabLoginSuccessEvent;
        public Action PlayFabLoginFailureEvent;


        public Action<Action,Action> LoadPlayerDataEvent;
        public Action SavePlayerDataEvent;

        public Action FetchSuccessEvent;
        public Action FetchFailureEvent;



        // Methods

        private void OnEnable()
        {
            isPlayerDataLoaded = false;
        }

        public void LoadPlayerData(Action OnSuccess,Action OnFailure)
        {
            LoadPlayerDataEvent?.Invoke(OnSuccess,OnFailure);
        }

        public void SavePlayerData()
        {
            if (isPlayerDataLoaded)
            {
                SavePlayerDataEvent?.Invoke();
            }
        }

        public void PlayFabLoginSuccess()
        {
            PlayFabLoginSuccessEvent?.Invoke();
        }

        public void PlayFabLoginFailure()
        {
            PlayFabLoginFailureEvent?.Invoke();
        }

        public void FetchSuccess()
        {
            FetchSuccessEvent?.Invoke();
        }

        public void FetchFailure()
        {
            FetchFailureEvent?.Invoke();
        }


    }
}
        