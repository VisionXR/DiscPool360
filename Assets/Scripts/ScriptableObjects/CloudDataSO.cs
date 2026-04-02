using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CloudDataSO", menuName = "ScriptableObjects/CloudDataSO", order = 1)]    
    public class CloudDataSO : ScriptableObject   
    {
        // variables
       


        public Action PlayFabLoginSuccessEvent;
        public Action PlayFabLoginFailureEvent;


        public Action<Action,Action> LoadPlayerDataEvent;
        public Action SavePlayerDataEvent;

        public Action FetchSuccessEvent;
        public Action FetchFailureEvent;



        // Methods

        private void OnEnable()
        {
            
        }

        public void LoadPlayerData(Action OnSuccess,Action OnFailure)
        {
            LoadPlayerDataEvent?.Invoke(OnSuccess,OnFailure);
        }

        public void SavePlayerData()
        {
            SavePlayerDataEvent?.Invoke();
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
        