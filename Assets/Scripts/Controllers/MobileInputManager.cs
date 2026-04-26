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

        private bool _isTouching = false;
        private Vector2 _lastTouchPosition;
        private Vector2 _swipeStartPosition;
        private float _swipeStartTime;
        public float cutoffValue = 0.1f;
        private float _initialPinchDistance;
        private bool isTouching = false;


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

            bool isZooming = false;
    
            isZooming = HandleZoomInput();
            

            if (inputData.isInputEnabled && !isZooming)
            {
                HandleTouchInput();
            }      

        }



        private bool HandleZoomInput()
        {

            // Check active touches using the new API
            var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
              
                if (activeTouches.Count == 2)
                {
                    var touch0 = activeTouches[0];
                    var touch1 = activeTouches[1];

                    if (touch1.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        _initialPinchDistance = Vector2.Distance(touch0.screenPosition, touch1.screenPosition);
                        inputData.ZoomStarted();
                       

                    }
                    else if (touch0.phase == UnityEngine.InputSystem.TouchPhase.Moved || touch1.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                    {
                        float currentDistance = Vector2.Distance(touch0.screenPosition, touch1.screenPosition);
                        float delta = (currentDistance - _initialPinchDistance) * 0.005f;

                        // Send this delta to InputDataSO (Normalized for sensitivity)


                        delta = Mathf.Clamp(delta, -1.0f, 1.0f);
                        inputData.ZoomChanged(delta);
                        _initialPinchDistance = currentDistance;
                       

                    }
                    else
                    {
                        _isTouching = false;
                    }

                    return true;

                }
              
            }
            return false; // Prioritize pinch over single touch rotation

        }

        private void HandleTouchInput()
        {
            // Check active touches using the new API
            var activeTouches = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                if (activeTouches.Count == 1)
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
                
            }
            else
            {
                isTouching = false;
            }

            // Fallback for Editor testing with mouse
            if (!Application.isEditor)
            {
                return;
            }

            // Fallback for Pointer (Mouse/Pen) using the New Input System
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
                    // Only process Moved if the pointer actually changed position to save performance
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
            

            _isTouching = true;
            _lastTouchPosition = touch;
            _swipeStartPosition = touch;
            _swipeStartTime = Time.time;

           
        }

        private void HandleTouchUpdate(Vector2 touch)
        {
            // Only process if position has actually changed
            if (_lastTouchPosition == touch)
            {
                return;
            }

            _lastTouchPosition = touch;

        
        }

        private void HandleTouchEnded(Vector2 touch)
        {

            if (_isTouching)
            {
                _isTouching = false;
                // Check for swipe gesture
                DetectSwipe(touch);
            }
        }

        private void DetectSwipe(Vector2 touchEndPosition)
        {
            Vector2 swipeDelta = touchEndPosition - _swipeStartPosition;
            float swipeDuration = Time.time - _swipeStartTime;

            Debug.Log("Swipe duration: " + swipeDuration);

            // Check if swipe duration is within threshold
            if (swipeDuration > swipemaxTimeThreshold || swipeDuration < swipeminTimeThreshold)
            {
                return; // Too slow to be a swipe
            }

            // Calculate absolute distances
            float horizontalDistance = Mathf.Abs(swipeDelta.x);
            float verticalDistance = Mathf.Abs(swipeDelta.y);

            // Check if minimum swipe distance is met for either direction
            float maxDistance = Mathf.Max(horizontalDistance, verticalDistance);

            Debug.Log("max distance: " + maxDistance);
            if (maxDistance < swipeminDistanceThreshold)
            {
                return; // Swipe distance too short
            }

            // Determine which direction is dominant
            if (horizontalDistance > verticalDistance)
            {
                // Horizontal swipe is dominant
                float normalizedValue = NormalizeSwipeDistance(horizontalDistance);
                float directedValue = Mathf.Sign(swipeDelta.x) * normalizedValue;
                inputData.HorizontalSwiped(directedValue);
                Debug.Log("Horizontal Swipe Detected: " + directedValue);
            }
            else if (verticalDistance > horizontalDistance)
            {
                // Vertical swipe is dominant
                float normalizedValue = NormalizeSwipeDistance(verticalDistance);
                float directedValue = Mathf.Sign(swipeDelta.y) * normalizedValue;
                inputData.VerticalSwiped(directedValue);
                Debug.Log("Vertical Swipe Detected: " + directedValue);
            }
            // If equal (very unlikely), no swipe is fired
        }

        private float NormalizeSwipeDistance(float distance)
        {
            // Clamp distance between min and max thresholds
            float clamped = Mathf.Clamp(distance, swipeminDistanceThreshold, swipemaxDistanceThreshold);

            // Normalize to 0-1 range
            float normalized = (clamped - swipeminDistanceThreshold) / (swipemaxDistanceThreshold - swipeminDistanceThreshold);

            // Map to -1 to 1 range (0 maps to -1, 0.5 maps to 0, 1 maps to 1)
            return normalized * 2f - 1f;
        }
    }
}