using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "ADDataSO", menuName = "ScriptableObjects/ADDataSO", order = 1)]
    public class ADDataSO : ScriptableObject
    {
        // variables


        // events
        public Action ShowInterstitialAdEvent;
        public Action LoadInterstitialAdEvent;

        public Action ShowRewardAdEvent;
        public Action LoadRewardAdEvent;


        //methods

        public void ShowInterstitialAd()
        {
            ShowInterstitialAdEvent?.Invoke();
        }

        public void LoadInterstitialAd()
        {
            LoadInterstitialAdEvent?.Invoke();
        }

        public void ShowRewardAd()
        {
            ShowRewardAdEvent?.Invoke();
        }

        public void LoadRewardAd()
        {
            LoadRewardAdEvent?.Invoke();
        }
    }
}
