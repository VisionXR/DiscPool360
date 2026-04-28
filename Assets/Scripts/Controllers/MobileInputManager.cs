using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


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

        [Header("Swipe Settings")]
        public float swipeminDistanceThreshold = 100f; // Minimum pixels to register a swipe
        public float swipemaxDistanceThreshold = 400f; // Minimum pixels to register a swipe
        public float swipeminTimeThreshold = 0.05f; // Minimum time for a swipe (seconds)
        public float swipemaxTimeThreshold = 1; // Maximum time for a swipe (seconds)

        // Gesture type tracking
        private enum GestureType { None, Zoom, Swipe, Rotation }
        private GestureType _currentGestureType = GestureType.None;

        private Vector2 swipeStartPosition;
        private float swipeStartTime;
        public float cutoffValue = 0.1f;
        private float initialPinchDistance;



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

        private void LateUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;


            HandleTouchInput();
        }



        private void HandleTouchInput()
        {


            var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

            if (activeTouches.Count == 1)
            {
                var touch = activeTouches[0];
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

            // Fallback for Editor testing with mouse
            if (!Application.isEditor)
                return;

            var pointer = Pointer.current;
            if (pointer != null)
            {
                Vector2 pointerPos = pointer.position.ReadValue();

                if (pointer.press.wasPressedThisFrame)
                {
                    HandleTouchBegan(pointerPos);
                }
                else if (pointer.press.isPressed)
                {
                    HandleTouchUpdate(pointerPos);
                }
                else if (pointer.press.wasReleasedThisFrame)
                {
                    HandleTouchEnded(pointerPos);
                }
            }
        }

        private void HandleTouchBegan(Vector2 touch)
        {


            swipeStartPosition = touch;
            swipeStartTime = Time.time;

            Ray ray = Camera.main.ScreenPointToRay(touch);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Edge"))
                {

                    inputData.RotationPinchStarted(touch);
                    return;
                }
            }


            inputData.FoulPinchStarted(touch);

        }

        private void HandleTouchUpdate(Vector2 touch)
        {

            inputData.RotationPinchContinued(touch);
            inputData.FoulPinchContinued(touch);
        }

        private void HandleTouchEnded(Vector2 touch)
        {

            inputData.RotationPinchEnded(touch);
            inputData.FoulPinchEnded(touch);
        }

    }
}