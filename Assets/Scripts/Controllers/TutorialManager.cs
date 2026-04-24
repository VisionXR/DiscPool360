using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using Photon.Voice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public List<TutorialStep> handTutorialSteps;
        public List<TutorialStep> controllerTutorialSteps;
        public List<TutorialStep> tutorialSteps;
        public TutorialDataSO tutorialData;
        public TableDataSO tableData;
        public BoardDataSO boardData;
        public InputDataSO inputData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;
        public UIDataSO uiData;


        [Header("Game Objects")]
        public GameObject tutorialBoard;
        public GameObject tutorialStriker;
        public GameObject glowStriker;
        public GameObject tutorialCoin;


        [Header("Local Variables")]
        public Vector3 strikerInitPosition;
        public BoardCreation boardCreation;
        public Platform platform;
        public StrikerArrow strikerArrow;
        public TutorialStep currentStep;
        private Coroutine _tutorialRoutine;
        private bool _stepCompleted;
        private bool _tutorialSkipped;
        private int _currentStepIndex = -1;
        private bool isCoinPocketed = false;

        private void OnEnable()
        {
            Reset();
            tutorialData.NextBtnClcikedEvent += NextBtnClicked;
            tutorialData.SkipBtnClcikedEvent += SkipBtnClicked;
            tutorialData.PlayBtnClickedEvent += PlayBtnClicked;


            tableData.PlatformRotationStartedEvent += PlatformRotationStarted;
            tableData.PlatformRotationEndedEvent += PlatformRotationEnded;

            
            coinData.CoinPocketedEvent += CoinPocketed;

            strikerData.StrikerStartedEvent += StrikeStarted;
            strikerData.StrikerStoppedEvent += StrikeCompleted;
            strikerData.FoulCompleteEvent += FoulComplete;

            boardCreation.StartTutorial();
            tutorialBoard.SetActive(true);
            StartCoroutine(RunTutorialSteps());
        }

        private void OnDisable()
        {
            tutorialData.NextBtnClcikedEvent -= NextBtnClicked;
            tutorialData.SkipBtnClcikedEvent -= SkipBtnClicked;
            tutorialData.PlayBtnClickedEvent -= PlayBtnClicked;


            tableData.PlatformRotationStartedEvent -= PlatformRotationStarted;
            tableData.PlatformRotationEndedEvent -= PlatformRotationEnded;

            
            coinData.CoinPocketedEvent -= CoinPocketed;
            strikerData.StrikerStoppedEvent -= StrikeCompleted;
            strikerData.StrikerStartedEvent -= StrikeStarted;
            strikerData.FoulCompleteEvent -= FoulComplete;

            boardCreation.EndTutorial();
        }

        private void Reset()
        {
            if (_tutorialRoutine != null)
            {
                StopCoroutine(_tutorialRoutine);
                _tutorialRoutine = null;
            }
        
            tutorialBoard.SetActive(false);
            tutorialStriker.SetActive(false);
            glowStriker.SetActive(false);
            tutorialCoin.SetActive(false);
            inputData.DisableInput();
            tutorialData.canIAim = false;
            tutorialData.canIFire = false;
            isCoinPocketed = false;
            _tutorialSkipped = false;
            _stepCompleted = false;
            _currentStepIndex = -1;
        }
        
        private void CoinPocketed(GameObject coin)
        {
            tutorialCoin.GetComponent<Rigidbody>().isKinematic = true;
            isCoinPocketed = true;
        }

        // Call this to start running the steps
        private void PlayBtnClicked()
        {
            Reset();
            uiData.TriggerHomeEvent();
            gameObject.SetActive(false);
        }

        private void NextBtnClicked()
        {
            // Force-advance the current step
            _stepCompleted = true;
        }

        private void SkipBtnClicked()
        {

            Reset();
            uiData.TriggerHomeEvent();
            gameObject.SetActive(false);
        }

        private IEnumerator RunTutorialSteps()
        {
            yield return new WaitForSeconds(1);


            for (int i = 0; i < controllerTutorialSteps.Count; i++)
            {
                if (_tutorialSkipped)
                {
                    break;
                }

                _currentStepIndex = i;
                _stepCompleted = false;

             
                    tutorialSteps = controllerTutorialSteps;
                


                // Optional: if your TutorialStep has enter/exit hooks, you can call them here
                // tutorialSteps[i].BeginStep();

                tutorialData.ShowTutorialStep(
                    i + 1,
                    tutorialSteps[i].stepText,
                    tutorialSteps[i].stepAudio,
                    tutorialSteps[i].stepVideo,
                    tutorialSteps[i].interactiveStepType,
                    tutorialSteps[i].stepAudio.length
                );

                currentStep = tutorialSteps[i];


                if (currentStep.interactiveStepType == InteractiveStepType.BoardRotation)
                {
                    boardData.TurnOnInteractable();
                }

                else if (currentStep.interactiveStepType == InteractiveStepType.Aiming)
                {
                    platform.transform.localEulerAngles = Vector3.zero;
                    tutorialStriker.SetActive(true);
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;
                    glowStriker.SetActive(true);
               
                    strikerArrow.TurnOnDisplayArrow();
                    glowStriker.transform.localEulerAngles = currentStep.aimingStrikerRotation;
                    inputData.EnableInput();
                    tutorialData.canIAim = true;
                }


                else if (currentStep.interactiveStepType == InteractiveStepType.Foul)
                {
                    platform.transform.localEulerAngles = Vector3.zero;
                    tutorialStriker.SetActive(true);
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                    tutorialStriker.transform.position = boardData.StrikerFoulPositions[3].position;
                    tutorialStriker.transform.rotation = boardData.StrikerFoulPositions[3].rotation;
                    tutorialStriker.GetComponent<Rigidbody>().isKinematic = true;
                    strikerData.SetFoul(true);
                    tutorialData.canIPlaceStriker = true;
                    
                   
                }

                else if (currentStep.interactiveStepType == InteractiveStepType.Striking)
                {

                    platform.transform.localEulerAngles = Vector3.zero;
                    isCoinPocketed = false;
                    strikerArrow.TurnOnArrow();
                    tutorialStriker.SetActive(true);
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;
                    glowStriker.SetActive(true);
                    glowStriker.transform.localEulerAngles = currentStep.strikingStrikerRotation;
                    inputData.EnableInput();
                    tutorialData.canIAim = true;
                    tutorialData.canIFire = true;
                    tutorialCoin.SetActive(true);

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
                    strikerData.SetFoul(false);
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.transform.localEulerAngles = Vector3.zero;

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
                    tutorialCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialCoin.transform.localRotation = Quaternion.identity;
                    tutorialCoin.transform.localPosition = currentStep.coinPosition;
                }

                else
                {
                    tutorialStriker.SetActive(false);
                }


                // Wait until this step is marked as completed or tutorial is skipped
                yield return new WaitUntil(() => _stepCompleted || _tutorialSkipped);

                // tutorialSteps[i].EndStep();
            }

            _currentStepIndex = -1;
            _tutorialRoutine = null;


        }


        private void PlatformRotationStarted()
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.BoardRotation)
            {

            }
        }

        private void PlatformRotationEnded()
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.BoardRotation)
            {
                tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);
               
            }
        }

        private void FoulComplete()
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Foul)
            {
                tutorialData.canIPlaceStriker = false;
                tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);

            }
        }

        private void Tapped(float val)
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Aiming)
            {

                if (Mathf.Abs(tutorialStriker.transform.localEulerAngles.y - glowStriker.transform.localEulerAngles.y) <= 5f)
                {
                    // Aimed properly
                    tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);
                    tutorialStriker.SetActive(false);
                    glowStriker.SetActive(false);
                    inputData.DisableInput();
                    tutorialData.canIAim = false;
                }
                else
                {
                    tutorialData.ShowTutorialStepFailed(currentStep.failureText, currentStep.failureAudio);

                }
            }
        }

        private void StrikeStarted()
        {
            glowStriker.SetActive(false);
        }

        private void StrikeCompleted()
        {
            if (currentStep != null && currentStep.interactiveStepType == InteractiveStepType.Striking)
            {
                if (isCoinPocketed)
                {
                    // Aimed properly
                    tutorialData.ShowTutorialStepSuccess(currentStep.successText, currentStep.successAudio);

                    tutorialStriker.transform.localEulerAngles = Vector3.zero;
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                    tutorialStriker.SetActive(false);


                    glowStriker.SetActive(false);

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
                    strikerData.SetFoul(false);
                    tutorialCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialCoin.transform.localPosition = currentStep.coinPosition;
                    tutorialCoin.transform.localEulerAngles = Vector3.zero;
                    tutorialCoin.SetActive(false);


                    inputData.DisableInput();
                    tutorialData.canIAim = false;
                    tutorialData.canIFire = false;
                }
                else
                {
                    inputData.EnableInput();
                    strikerArrow.TurnOnArrow();
                    glowStriker.SetActive(true);
                    tutorialData.ShowTutorialStepFailed(currentStep.failureText, currentStep.failureAudio);

                    tutorialCoin.GetComponent<Rigidbody>().isKinematic = false;
                    strikerData.SetFoul(false);
                    tutorialCoin.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialCoin.transform.localPosition = currentStep.coinPosition;
                    tutorialCoin.transform.localEulerAngles = Vector3.zero;
                  

                    tutorialStriker.transform.localEulerAngles = Vector3.zero;
                    tutorialStriker.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                    tutorialStriker.transform.localPosition = strikerInitPosition;
                   
                }

            }

        }

    }
}
