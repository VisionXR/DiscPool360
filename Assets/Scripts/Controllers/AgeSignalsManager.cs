using UnityEngine;
using System;

public class AgeSignalsManager : MonoBehaviour
{
    // Interface proxy matching the Java callback interface
    private class AgeSignalsCallbackProxy : AndroidJavaProxy
    {
        private Action<int, int, int> onSuccessAction;
        private Action<string> onFailureAction;

        public AgeSignalsCallbackProxy(Action<int, int, int> onSuccess, Action<string> onFailure)
            : base("com.visionxr.gamesignals.AgeSignalsBridge$AgeSignalsCallback")
        {
            this.onSuccessAction = onSuccess;
            this.onFailureAction = onFailure;
        }

        // Called by Java on success
        void onSuccess(int status, int lower, int upper)
        {
            // Move back to main thread via Unity Action if UI updates are needed
            onSuccessAction?.Invoke(status, lower, upper);
        }

        // Called by Java on failure
        void onFailure(string errorMessage)
        {
            onFailureAction?.Invoke(errorMessage);
        }
    }


    public void RequestAgeSignals()
    {
        try
        {
            // Get current Unity player activity context
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Create the C# proxy for the callback
                    AgeSignalsCallbackProxy callbackProxy = new AgeSignalsCallbackProxy(
                        onSuccess: (status, lower, upper) => {
                            Debug.Log($"[AgeSignals] Status: {status}, Lower Bound: {lower}, Upper Bound: {upper}");
                            ApplyAgeRestrictions(status, lower, upper);
                        },
                        onFailure: (error) => {
                            Debug.LogError($"[AgeSignals] Failed: {error}");
                            // Default to strict safety settings if API fails
                        }
                    );

                    // Call the static method in our Java file
                    using (AndroidJavaClass bridgeClass = new AndroidJavaClass("com.visionxr.gamesignals.AgeSignalsBridge"))
                    {
                        bridgeClass.CallStatic("fetchAgeSignals", currentActivity, callbackProxy);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[AgeSignals] Plugin error: {e.Message}");
        }
    }

    private void ApplyAgeRestrictions(int status, int lower, int upper)
    {
        // Example handling:
        // status flags: 0 = UNKNOWN, 1 = DECLARED, 2 = VERIFIED, 3 = SUPERVISED
        if (lower >= 18 || lower == -1)
        {
            Debug.Log("Adult user or unrestricted region. Enabling all features.");
            // Enable multiplayer chat, unrestricted microtransactions, etc.
        }
        else
        {
            Debug.Log("Minor user detected. Restricting social features.");
            // Disable native text chat, hide targeted ads, apply privacy modes
        }
    }
}