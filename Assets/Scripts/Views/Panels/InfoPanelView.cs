using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;
using Google.Play.Review;


namespace com.VisionXR.Views
{
    public class InfoPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public AudioDataSO audioData;
        public UIDataSO uiData;

        private ReviewManager _reviewManager;
        private PlayReviewInfo _playReviewInfo;

        [Header("Panel Objects")]
        public string currentState;


        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

        public void ReviewButtonClick()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if (Application.isEditor)
            {
                // Fallback URL for testing in the Unity Editor or if the market link fails
                string browserURL = "https://play.google.com/store/apps/details?id=com.VisionXR.DiscPool360";
                Application.OpenURL(browserURL);
                return;
            }

            StartCoroutine(RequestAndShowReview());
        }

        private IEnumerator RequestAndShowReview()
        {
            _reviewManager = new ReviewManager();


            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;

            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogError($"Review Flow Request Failed: {requestFlowOperation.Error.ToString()}");
   

           OpenPlayStorePage();
                yield break;
            }

            _playReviewInfo = requestFlowOperation.GetResult();

 
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;

            _playReviewInfo = null; // Clear the reference after execution

            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                Debug.LogError($"Launch Review Failed: {launchFlowOperation.Error.ToString()}");
                OpenPlayStorePage();
                yield break;
            }

            Debug.Log("In-App Review completed or dismissed by user.");
        }

   
        public void OpenPlayStorePage()
        {
            string packageName = Application.identifier; // Auto-fetches your bundle ID (e.g. com.company.game)
            Application.OpenURL($"market://details?id={packageName}");
        }
    }
}
