using com.VisionXR.ModelClasses;
using GoogleMobileAds.Api;
using UnityEngine;


public class AdManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public ADDataSO adData;

    // 1. The ID strings can stay platform-dependent
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

    // 2. MOVE THESE HERE (Outside the #if blocks)
    // This ensures the compiler always sees them, even in the Editor!
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;


    private void OnEnable()
    {
        adData.LoadInterstitialAdEvent += LoadInterstitialAd;
        adData.ShowInterstitialAdEvent += ShowInterstitialAd;

        adData.LoadRewardAdEvent += LoadRewardedAd;
        adData.ShowRewardAdEvent += ShowRewardedAd;
    }

    private void OnDisable()
    {
        adData.LoadInterstitialAdEvent -= LoadInterstitialAd;
        adData.ShowInterstitialAdEvent -= ShowInterstitialAd;

        adData.LoadRewardAdEvent -= LoadRewardedAd;
        adData.ShowRewardAdEvent -= ShowRewardedAd;
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
                
                LoadInterstitialAd();
            };
        });
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
            LoadInterstitialAd();
        }
        else
        {
            Debug.LogWarning("Interstitial ad is not ready yet.");
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

        Debug.Log("Loading rewarded ad...");
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
                Debug.Log("Rewarded ad closed.");
                LoadRewardedAd();
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

                // Execute the reward callback on the main thread safely
             //   onRewardEarned?.Invoke();

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