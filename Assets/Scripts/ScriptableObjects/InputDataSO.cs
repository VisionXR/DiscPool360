using com.VisionXR.HelperClasses;
using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "InputDataSO", menuName = "ScriptableObjects/InputDataSO", order = 1)]
    public class InputDataSO : ScriptableObject
    {
        // User Data
        public bool isInputEnabled = true;
        public bool isBoardGrabbed = false;
        public bool isInputLocked = false;



        // Events
        public Action<float> RotateStrikerAbsoluteEvent;
        public Action<float> RotateStrikerRelativeEvent;
        public Action<Vector3> RotateStrikerTowardsEvent;
        public Action<Vector2> RotateStrikerEvent;

        public Action<float> StartStrikeEvent;
        public Action<float> EndStrikeEvent;
        public Action<float> FireStrikeEvent;

        public Action<bool> InputChangeEvent;
     

        public Action<Vector3> PinchStartedEvent;
        public Action<Vector3> PinchContinuedEvent;
        public Action PinchEndedEvent;

        public Action<Vector2> RotationPinchStartedEvent;
        public Action<Vector2> RotationPinchContinuedEvent;
        public Action<Vector2>RotationPinchEndedEvent;

        public Action<int> PlatformRotatedEvent;

        public Action<float> MovePlayerXEvent;
        public Action<float> MovePlayerYEvent;


        public Action<DominantHand, bool> GrabInputEvent;
        public Action<DominantHand, bool> TapInputEvent;

        public Action<bool> PlatformHiglightEvent;

        //Methods

        void OnEnable()
        {
            isInputEnabled = false;
        }

        public void EnableInput()
        {
            isInputEnabled = true;
            InputChangeEvent?.Invoke(true);
        }

        public void DisableInput()
        {
            isInputEnabled = false;
            InputChangeEvent?.Invoke(false);
        }


        public void PinchStarted(Vector3 origin)
        {
            PinchStartedEvent?.Invoke(origin);
        }

        public void PinchContinued(Vector3 origin)
        {
            PinchContinuedEvent?.Invoke(origin);
        }

        public void PinchEnded()
        {
            PinchEndedEvent?.Invoke();
        }

        public void RotationPinchStarted(Vector2 origin)
        {
            RotationPinchStartedEvent?.Invoke(origin);
        }

        public void RotationPinchContinued(Vector2 origin)
        {
            RotationPinchContinuedEvent?.Invoke(origin);
        }

        public void RotationPinchEnded(Vector2 pos)
        {
            RotationPinchEndedEvent?.Invoke(pos);
        }

        public void StartStrike()
        {
            StartStrikeEvent?.Invoke(1);
        }

        public void EndStrike()
        {
            EndStrikeEvent?.Invoke(0);
        }

        public void FireStrike(float power)
        {
            FireStrikeEvent?.Invoke(power);
        }

        public void RotateStrikerAbsolute(float angle)
        {
            RotateStrikerAbsoluteEvent?.Invoke(angle);
        }

        public void RotateStrikerRelative(float angle)
        {
            RotateStrikerRelativeEvent?.Invoke(angle);
        }

        public void RotateStriker(Vector2 direction)
        {
            RotateStrikerEvent?.Invoke(direction);
        }

        public void RotateStrikerTowards(Vector3 initialPosition)
        {
            RotateStrikerTowardsEvent?.Invoke(initialPosition);
        }

        public void PlatformRotated(int direction)
        {
            PlatformRotatedEvent?.Invoke(direction);
        }

        public void BoardGrabbed()
        {
            isBoardGrabbed = true;
        }

        public void BoardReleased()
        {
            isBoardGrabbed = false;
        }

        public void LockInput()
        {
            isInputLocked = true;
        }

        public void UnlockInput()
        {
            isInputLocked = false;
        }

        public void MovePlayerX(float value)
        {
            MovePlayerXEvent?.Invoke(value);
        }

        public void MovePlayerY(float value)
        {
            MovePlayerYEvent?.Invoke(value);
        }

        public void GrabInput(DominantHand hand,bool value)
        {
            GrabInputEvent?.Invoke(hand, value);
        }

        public void TapInput(DominantHand hand,bool value)
        {
            TapInputEvent?.Invoke(hand, value);
        }

        public void PlatformHighlight(bool value)
        {
            PlatformHiglightEvent?.Invoke(value);
        }

    }
}
