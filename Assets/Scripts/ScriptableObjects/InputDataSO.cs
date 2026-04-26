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
        
        // Events

        public Action<float> FireStrikeEvent;
        public Action<float> StrikerForceChangedEvent;
        public Action<float> ZoomChangedEvent;
        public Action ZoomStartedEvent;

        public Action<bool> InputChangeEvent;

        public Action<float> RotateStrikerAbsoluteEvent;
        public Action<Vector2> RotationPinchStartedEvent;
        public Action<Vector2> RotationPinchContinuedEvent;
        public Action<Vector2>RotationPinchEndedEvent;


        public Action<bool> PlatformHiglightEvent;

        public Action<float> HorizontalSwipedEvent;
        public Action<float> VerticalSwipedEvent;

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

        public void HorizontalSwiped(float a)
        {
            HorizontalSwipedEvent?.Invoke(a);
        }


        public void VerticalSwiped(float a)
        {
            VerticalSwipedEvent?.Invoke(a);
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


        public void FireStrike(float power)
        {
            FireStrikeEvent?.Invoke(power);
        }

        public void RotateStrikerAbsolute(float angle)
        {
            RotateStrikerAbsoluteEvent?.Invoke(angle);
        }


        public void StrikerForceChanged(float force)
        {
            StrikerForceChangedEvent?.Invoke(force);
        }

        public void ZoomStarted()
        {
            ZoomStartedEvent?.Invoke();
        }

        public void ZoomChanged(float zoomLevel)
        {
            ZoomChangedEvent?.Invoke(zoomLevel);
        }

        public void BoardGrabbed()
        {
            isBoardGrabbed = true;
        }

        public void BoardReleased()
        {
            isBoardGrabbed = false;
        }


        public void PlatformHighlight(bool value)
        {
            PlatformHiglightEvent?.Invoke(value);
        }

    }
}
