using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using TMPro;
using UnityEngine;




namespace com.VisionXR.Views
{
    public class TutorialPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public TutorialDataSO tutorialData;

        [Header("UI Objects")]
        public TMP_Text StepNumberText;
        public TMP_Text SuccessFailurerText;
        public TMP_Text ContentText;
        public AudioSource audioSource;

        public GameObject NextBtn;
        public GameObject PlayBtn;

        [Header("Animations Objects")]
        public GameObject CentralUI;



        private void OnEnable()
        {
           
         
            Reset();
           tutorialData.ShowTutorialStepEvent += ShowTutorialStep;
            tutorialData.ShowTutorialStepSuccessEvent += ShowTutorialStepSuccess;
            tutorialData.ShowTutorialStepFailedEvent += ShowTutorialStepFailed;
        }

        private void OnDisable()
        {
           tutorialData.ShowTutorialStepEvent -= ShowTutorialStep;
            tutorialData.ShowTutorialStepSuccessEvent -= ShowTutorialStepSuccess;
            tutorialData.ShowTutorialStepFailedEvent -= ShowTutorialStepFailed;
        }

        private void Reset()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            ContentText.text = "";
            SuccessFailurerText.text = "";
            NextBtn.SetActive(false);
            PlayBtn.SetActive(false);
        }

        private void ShowTutorialStepFailed(string content, AudioClip clip)
        {
            SuccessFailurerText.text = content;

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void ShowTutorialStepSuccess(string content, AudioClip clip)
        {
            SuccessFailurerText.text = content;

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }

            NextBtn.SetActive(true);
        }

        private void ShowTutorialStep(int stepNumber, string contentText, AudioClip audioClip,  InteractiveStepType stepType)
        {
            StepNumberText.text = "Step " + stepNumber + "/" + tutorialData.totalSteps;
            SuccessFailurerText.text = "";
            NextBtn.SetActive(false);
            PlayBtn.SetActive(false);

        }

        private IEnumerator WaitAndShowNextBtn(float time)
        {
            yield return new WaitForSeconds(time);
            NextBtn.SetActive(true);
        }

        private IEnumerator WaitAndShowPlayBtn(float time)
        {
            yield return new WaitForSeconds(time);
            PlayBtn.SetActive(true);
        }

        public void NextBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            tutorialData.NextBtnClcikedEvent?.Invoke();
            SuccessFailurerText.text = "";
            NextBtn.SetActive(false);
        }

        public void PlayBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            tutorialData.PlayBtnClickedEvent?.Invoke();
            PlayBtn.SetActive(false);
        }

        public void SkipBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            tutorialData.SkipBtnClcikedEvent?.Invoke();
        }

    
    }
}
