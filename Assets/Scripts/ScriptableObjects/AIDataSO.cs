using System;
using UnityEngine;
using com.VisionXR.HelperClasses;
using System.Collections.Generic;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AIDataSO", menuName = "ScriptableObjects/AIDataSO", order = 1)]
    public class AIDataSO : ScriptableObject
    {
        // variables

        public float rotationSpeed = 45f;  // Adjust as needed
        public float positionSpeed = 4;
        public float calculatingShotTime = 2f;
        public float strikeWaitTime = 3;

        public Sprite AIEasy;
        public Sprite AIMedium;
        public Sprite AIHard;

        // Events

        public Action<int,List<CoinInfo>> CoinInformationReceivedEvent;
        public Action<string> PlayAnimationEvent;
        public Action<string, bool> PlayHandAnimationEvent;
        public Action<string, bool> PlayLeftHandAnimationEvent;
        public Action<bool> StartRightHandMoveEvent;
        public Action<Vector3> StartRightHandRotationEvent;


        public Action<bool> StartLeftHandRotationEvent;

        // Methods

        public void StartLeftHandRotation(bool isForward)
        {
            StartLeftHandRotationEvent?.Invoke(isForward);
        }
        public void StartRightHandRotation(Vector3 targetEulerAngles)
        {
            StartRightHandRotationEvent?.Invoke(targetEulerAngles);
        }

        public void StartRightHandMove(bool state)
        {
            StartRightHandMoveEvent?.Invoke(state);
        }

        public void CoinInformationReceived(int id, List<CoinInfo> info)
        {
            CoinInformationReceivedEvent?.Invoke(id, info);
        }

        public void PlayAnimation(string animationTrigger)
        {
            PlayAnimationEvent?.Invoke(animationTrigger);
        }

        public void PlayHandAnimation(string handAnimationTrigger, bool state)
        {
            PlayHandAnimationEvent?.Invoke(handAnimationTrigger, state);
        }

        public void PlayLeftHandAnimation(string handAnimationTrigger, bool state)
        {
            PlayLeftHandAnimationEvent?.Invoke(handAnimationTrigger, state);
        }

    }
}
