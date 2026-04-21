using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.TouchPhase;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace com.VisionXR.Controllers
{
    public class MobileInputManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public UserDataSO userData;
        

        [Header("Audio Trigger")]
        
        public AudioSource grapSelectedAudio;
        public AudioSource grapUnSelectedAudio;
        public AudioSource tapSelectedAudio;

        private bool _isTouching = false;
        // Store the position to send to the Platform script
        private Vector2 _lastTouchPosition;
        public float cutoffValue = 0.15f;
        private bool isJoystickActive = false;

        private void OnEnable()
        {
            // 3. You MUST enable EnhancedTouch once
            EnhancedTouchSupport.Enable();   
         
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();

        }



        private void Start()
        {
            if (!Input.touchSupported && !Application.isEditor)
            {
                this.enabled = false;
            }
        }

        private void Update()
        {
            if (inputData.isInputEnabled)
            {
                HandleTouchInput();
            }       

        }

        private void HandleTouchInput()
        {


            // Check active touches using the new API
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                var touch = activeTouches[0]; // Get the first finger

                switch (touch.phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        HandleTouchBegan(touch.screenPosition);
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Moved:
                    case UnityEngine.InputSystem.TouchPhase.Stationary:
                        HandleTouchUpdate(touch.screenPosition);
                        break;

                    case UnityEngine.InputSystem.TouchPhase.Ended:
                    case UnityEngine.InputSystem.TouchPhase.Canceled:
                        HandleTouchEnded(touch.screenPosition);
                        break;
                }
            }


            if (! Application.isEditor)
            {
                return;
            }

            // 2. Fallback for Pointer (Mouse/Pen) using the New Input System
            var pointer = Pointer.current;

            if (pointer != null)
            {
                Vector2 pointerPos = pointer.position.ReadValue();

                if (pointer.press.wasPressedThisFrame)
                {
                    ProcessInput(pointerPos, TouchPhase.Began);
                }
                else if (pointer.press.isPressed)
                {
                    // Only process Moved if the pointer actually changed position to save performance
                    ProcessInput(pointerPos, TouchPhase.Moved);
                }
                else if (pointer.press.wasReleasedThisFrame)
                {
                    ProcessInput(pointerPos, UnityEngine.TouchPhase.Ended);
                }
            }
        }

        // Move your switch logic into this helper method
        private void ProcessInput(Vector2 screenPos, TouchPhase phase)
        {
            DominantHand hand = userData.myDominantHand;
            switch (phase)
            {
                case TouchPhase.Began:
                    HandleTouchBegan(screenPos); // Updated to take Vector2
                    break;
                case TouchPhase.Moved:
                    HandleTouchUpdate(screenPos); // Updated to take Vector2
                    break;
                case TouchPhase.Ended:
                    HandleTouchEnded(screenPos);
                    break;
            }
        }


        private void HandleTouchBegan(Vector2 touch)
        {
            _isTouching = true;
            _lastTouchPosition = touch;

            // 1. Fire the event for the Platform script to check for "Edge" raycast
            inputData.RotationPinchStarted(touch);

        }

        private void HandleTouchUpdate(Vector2 touch)
        {
            // 1. Fire the event for Platform rotation (Lazy Susan)
            // The Platform script uses the distance from the start to rotate
            inputData.RotationPinchContinued(touch);

          
        }

        private void HandleTouchEnded(Vector2 touch)
        {
            _isTouching = false;

            // 1. Fire the event for Platform to stop rotating and unparent assets
            inputData.RotationPinchEnded(touch);


        }
    }
}