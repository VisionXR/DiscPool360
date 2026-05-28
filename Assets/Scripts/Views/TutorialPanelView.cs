using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;



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

        [Header("Typewriter")]
        [SerializeField] private float typingSpeed = 0.03f;

        private Coroutine typingRoutine;
        private Coroutine buttonRoutine;

        [Header("Hand Swipe Animation")]
        [SerializeField] private float handStartScale = 1.2f;
        [SerializeField] private float handEndScale = 1f;
        [SerializeField] private float handMoveDistance = 80f;
        [SerializeField] private float handPressDuration = 0.2f;
        [SerializeField] private float handMoveDuration = 0.35f;
        [SerializeField] private float handPauseDuration = 0.2f;
        [SerializeField] private RectTransform handSwipeIcon;
        [SerializeField] private RectTransform leftHandSwipeIcon;
        [SerializeField] private RectTransform rightHandSwipeIcon;

        private Coroutine leftHandSwipeRoutine;
        private Coroutine rightHandSwipeRoutine;
        private Coroutine handSwipeRoutine;

        private Vector2 handInitialPosition;
        private Vector2 leftHandInitialPosition;
        private Vector2 rightHandInitialPosition;

        private void OnEnable()
        {
            if (handSwipeIcon != null)
            {
                handInitialPosition = handSwipeIcon.anchoredPosition;
                handSwipeIcon.gameObject.SetActive(false);
            }

            if (leftHandSwipeIcon != null)
            {
                leftHandInitialPosition = leftHandSwipeIcon.anchoredPosition;
                leftHandSwipeIcon.gameObject.SetActive(false);
            }

            if (rightHandSwipeIcon != null)
            {
                rightHandInitialPosition = rightHandSwipeIcon.anchoredPosition;
                rightHandSwipeIcon.gameObject.SetActive(false);
            }
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

            if (typingRoutine != null)
            {
                StopCoroutine(typingRoutine);
                typingRoutine = null;
            }

            if (buttonRoutine != null)
            {
                StopCoroutine(buttonRoutine);
                buttonRoutine = null;
            }
            if (leftHandSwipeRoutine != null)
            {
                StopCoroutine(leftHandSwipeRoutine);
                leftHandSwipeRoutine = null;
            }

            if (rightHandSwipeRoutine != null)
            {
                StopCoroutine(rightHandSwipeRoutine);
                rightHandSwipeRoutine = null;
            }


            if (handSwipeRoutine != null)
            {
                StopCoroutine(handSwipeRoutine);
                handSwipeRoutine = null;
            }

            if (leftHandSwipeIcon != null)
            {
                leftHandSwipeIcon.gameObject.SetActive(false);
                leftHandSwipeIcon.localScale = Vector3.one * handStartScale;
                leftHandSwipeIcon.anchoredPosition = leftHandInitialPosition;
            }

            if (rightHandSwipeIcon != null)
            {
                rightHandSwipeIcon.gameObject.SetActive(false);
                rightHandSwipeIcon.localScale = Vector3.one * handStartScale;
                rightHandSwipeIcon.anchoredPosition = rightHandInitialPosition;
            }

            if (handSwipeIcon != null)
            {
                handSwipeIcon.gameObject.SetActive(false);
                handSwipeIcon.localScale = Vector3.one * handStartScale;
                handSwipeIcon.anchoredPosition = handInitialPosition;
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
            if (stepNumber == 2)
            {
                StartHandSwipeAnimation(handSwipeIcon, handInitialPosition, ref handSwipeRoutine);
                StopHandSwipeAnimation(leftHandSwipeIcon, leftHandInitialPosition, ref leftHandSwipeRoutine);
                StopHandSwipeAnimation(rightHandSwipeIcon, rightHandInitialPosition, ref rightHandSwipeRoutine);
            }
            else if (stepNumber == 3)
            {
                StopHandSwipeAnimation(handSwipeIcon, handInitialPosition, ref handSwipeRoutine);
                StartHandSwipeAnimation(leftHandSwipeIcon, leftHandInitialPosition, ref leftHandSwipeRoutine);
                StartHandSwipeAnimation(rightHandSwipeIcon, rightHandInitialPosition, ref rightHandSwipeRoutine);
            }
            else
            {
                StopHandSwipeAnimation(handSwipeIcon, handInitialPosition, ref handSwipeRoutine);
                StopHandSwipeAnimation(leftHandSwipeIcon, leftHandInitialPosition, ref leftHandSwipeRoutine);
                StopHandSwipeAnimation(rightHandSwipeIcon, rightHandInitialPosition, ref rightHandSwipeRoutine);
            }
            SuccessFailurerText.text = "";
            NextBtn.SetActive(false);
            PlayBtn.SetActive(false);

            if (typingRoutine != null)
            {
                StopCoroutine(typingRoutine);
            }

            if (buttonRoutine != null)
            {
                StopCoroutine(buttonRoutine);
            }

            typingRoutine = StartCoroutine(TypeTutorialText(stepNumber, contentText, audioClip, stepType));
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

        private IEnumerator TypeTutorialText(int stepNumber, string fullText, AudioClip audioClip, InteractiveStepType stepType)
        {
            ContentText.text = "";

            if (audioSource != null)
            {
                audioSource.Stop();

                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
            }

            for (int i = 0; i < fullText.Length; i++)
            {
                ContentText.text += fullText[i];
                yield return new WaitForSeconds(typingSpeed);
            }

            if (stepType == InteractiveStepType.None)
            {
                float waitTime = 1f;

                if (audioClip != null)
                {
                    waitTime = Mathf.Max(audioClip.length - (fullText.Length * typingSpeed), 0f) + 1f;
                }

                if (stepNumber == tutorialData.totalSteps)
                {
                    buttonRoutine = StartCoroutine(WaitAndShowPlayBtn(waitTime));
                }
                else
                {
                    buttonRoutine = StartCoroutine(WaitAndShowNextBtn(waitTime));
                }
            }
        }

        private void StartHandSwipeAnimation(RectTransform handIcon, Vector2 initialPosition, ref Coroutine routine)
        {
            if (handIcon == null)
                return;

            if (routine != null)
            {
                StopCoroutine(routine);
            }

            handIcon.gameObject.SetActive(true);
            routine = StartCoroutine(PlayHandSwipeAnimation(handIcon, initialPosition));
        }

        private void StopHandSwipeAnimation(RectTransform handIcon, Vector2 initialPosition, ref Coroutine routine)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }

            if (handIcon != null)
            {
                handIcon.gameObject.SetActive(false);
                handIcon.localScale = Vector3.one * handStartScale;
                handIcon.anchoredPosition = initialPosition;
            }
        }

        private IEnumerator PlayHandSwipeAnimation(RectTransform handIcon, Vector2 initialPosition)
        {
            while (true)
            {
                handIcon.anchoredPosition = initialPosition;
                handIcon.localScale = Vector3.one * handStartScale;

                yield return AnimateScale(handIcon, handStartScale, handEndScale, handPressDuration);

                Vector2 leftPos = initialPosition + Vector2.left * handMoveDistance;
                Vector2 rightPos = initialPosition + Vector2.right * handMoveDistance;

                yield return AnimatePosition(handIcon, initialPosition, leftPos, handMoveDuration);
                yield return AnimatePosition(handIcon, leftPos, rightPos, handMoveDuration);
                yield return AnimatePosition(handIcon, rightPos, initialPosition, handMoveDuration);

                yield return new WaitForSeconds(handPauseDuration);
            }
        }

        private IEnumerator AnimateScale(RectTransform handIcon, float from, float to, float duration)
        {
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                float scale = Mathf.Lerp(from, to, t);
                handIcon.localScale = Vector3.one * scale;
                yield return null;
            }

            handIcon.localScale = Vector3.one * to;
        }

        private IEnumerator AnimatePosition(RectTransform handIcon, Vector2 from, Vector2 to, float duration)
        {
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                handIcon.anchoredPosition = Vector2.Lerp(from, to, t);
                yield return null;
            }

            handIcon.anchoredPosition = to;
        }
    }
}
