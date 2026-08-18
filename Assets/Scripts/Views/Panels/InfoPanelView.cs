using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Google.Play.Review;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


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
        public ScrollRect generalScrollRect;


        private void OnEnable()
        {
            StartCoroutine(ResetScroll());

        }

        private IEnumerator ResetScroll()
        {
            yield return new WaitForSeconds(uiData.disableTime+0.1f);
            generalScrollRect.verticalNormalizedPosition = 1f;
        }

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

            OpenPlayStorePage();
        }


   
        public void OpenPlayStorePage()
        {
            string packageName = Application.identifier; // Auto-fetches your bundle ID (e.g. com.company.game)
            Application.OpenURL($"market://details?id={packageName}");
        }
    }
}
