using System;
using UnityEngine;
using GoogleMobileAds.Api;
using com.VisionXR.ModelClasses;

namespace com.VisionXR.Controllers
{
    public class AdManager : MonoBehaviour
    {
        [Header("Scriptable Object References")]
        public ADDataSO adDataSO;

        // Test Ad Unit IDs (Replace these with your actual IDs from AdMob dashboard)
#if UNITY_ANDROID
        private string _interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        private string _rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
    private string _interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string _rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _interstitialAdUnitId = "unused";
    private string _rewardedAdUnitId = "unused";
#endif

        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;

        private void OnEnable()
        {
            // Subscribe to events from the Scriptable Object
            adDataSO.LoadInterstitialAdEvent += LoadInterstitialAd;
            adDataSO.LoadRewardedAdEvent += LoadRewardedAd;
            adDataSO.ShowInterstitialAdEvent += ShowInterstitialAd;
            adDataSO.ShowRewardedAdEvent += ShowRewardedAd;
        }

        private void OnDisable()
        {
            // Unsubscribe from events to prevent memory leaks
            adDataSO.LoadInterstitialAdEvent -= LoadInterstitialAd;
            adDataSO.LoadRewardedAdEvent -= LoadRewardedAd;
            adDataSO.ShowInterstitialAdEvent -= ShowInterstitialAd;
            adDataSO.ShowRewardedAdEvent -= ShowRewardedAd;
        }


        private void Start()
        {
            // Initialize the Mobile Ads SDK
            MobileAds.Initialize((InitializationStatus status) =>
            {
                Debug.Log("Mobile Ads SDK Initialized.");
                // Load ads as soon as the SDK is ready
                LoadInterstitialAd();
                LoadRewardedAd();
            });
        }

        #region Interstitial Ad Methods

        public void LoadInterstitialAd()
        {
            // Clean up the old ad before loading a new one
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

           
            var adRequest = new AdRequest();

            InterstitialAd.Load(_interstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load: " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded successfully.");
                _interstitialAd = ad;

                // Register to handle ad closure so we can preload the next one
                _interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    adDataSO.OnInterstitialAdSuccess(); // Notify the Scriptable Object about the success
                    //LoadInterstitialAd();
                };
            });
        }

        public void ShowInterstitialAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
                LoadInterstitialAd();
            }
            else
            {
                LoadInterstitialAd(); // Try loading again
            }
        }

        #endregion

        #region Rewarded Ad Methods

        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }


            var adRequest = new AdRequest();

            RewardedAd.Load(_rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded successfully.");
                _rewardedAd = ad;

                // Register to handle ad closure so we can preload the next one
                _rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    adDataSO.OnRewardedAdSuccess(); // Notify the Scriptable Object about the reward
                };
            });
        }

        public void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                Debug.Log("Showing rewarded ad.");
                _rewardedAd.Show((Reward reward) =>
                {
                    Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
                  
                    LoadRewardedAd();
                });
            }
            else
            {
                Debug.LogWarning("Rewarded ad is not ready yet.");
                LoadRewardedAd(); // Try loading again
            }
        }

        #endregion
    }
}