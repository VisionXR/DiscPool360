using System;
using UnityEngine;
using UnityEngine.Video;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "TutorialDataSO", menuName = "ScriptableObjects/TutorialDataSO", order = 1)]
    public class TutorialDataSO : ScriptableObject
    {
        // variables
        public bool canIMoveTable;
        public bool canIRotatePlatform;
        public bool canIAim;
        public bool canIFire;
        public bool canIPlaceStriker;
        public int totalSteps = 8;

        // Events
        public Action CheckTableMoveEvent;
        public Action CheckRotatePlatformEvent;
        public Action CheckAimEvent;
        public Action CheckStrikeEvent;


        public Action<int, string, AudioClip, VideoClip,InteractiveStepType> ShowTutorialStepEvent;
        public Action<string, AudioClip> ShowTutorialStepSuccessEvent;
        public Action<string, AudioClip> ShowTutorialStepFailedEvent;
        public Action NextBtnClcikedEvent;
        public Action SkipBtnClcikedEvent;
        public Action PlayBtnClickedEvent;

        public Action ShowNextBtnEvent;

        // Methods

        public void ResetVariables()
        {
            canIMoveTable = false;
            canIRotatePlatform = false;
            canIAim = false;
            canIFire = false;
        }

        public void ShowTutorialStep(int stepNumber, string contentText, AudioClip audioClip, VideoClip videoClip,InteractiveStepType stepType,float time)
        {
            ShowTutorialStepEvent?.Invoke(stepNumber, contentText, audioClip, videoClip,stepType);
        }

        public void ShowTutorialStepSuccess(string contentText, AudioClip audioClip)
        {
            ShowTutorialStepSuccessEvent?.Invoke(contentText, audioClip);
        }

        public void ShowTutorialStepFailed(string contentText, AudioClip audioClip)
        {
            ShowTutorialStepFailedEvent?.Invoke(contentText, audioClip);
        }

        public void SetCanIMoveTable(bool value)
        {
            canIMoveTable = value;
        }

        public void SetCanIRotatePlatform(bool value)
        {
            canIRotatePlatform = value;
        }

        public void SetCanIAim(bool value)
        {
            canIAim = value;
        }

        public void SetCanIFire(bool value)
        {
            canIFire = value;
        }

        public void CheckTableMovement()
        {
            if (canIMoveTable)
            {
                CheckTableMoveEvent?.Invoke();
            }
        }

        public void CheckPlatformRotation()
        {
            if (canIRotatePlatform)
            {
                CheckRotatePlatformEvent?.Invoke();
            }
        }

        public void CheckAim()
        {
            if (canIAim)
            {
                CheckAimEvent?.Invoke();
            }
        }

        public void CheckStrike()
        {
            if (canIFire)
            {
                CheckStrikeEvent?.Invoke();
            }
        }

        public void ShowNextButton()
        {
            ShowNextBtnEvent?.Invoke();
        }
    }
}
