using com.VisionXR.ModelClasses;
using Photon.Realtime;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public GameDataSO gameData;
        public StrikerDataSO strikerData;
        public TableDataSO tableData;
        public BoardDataSO boardData;
        public PlayerDataSO playerData;

        [Header("Game Objects")]
        public GameObject AllAssets;
        public GameObject Platform;
        public GameObject striker;
        public StrikerShooting strikerShooting;
        public StrikerMovement strikerMovement;

        // Drag state (striker)
        private bool isTouchingStriker;
        private Vector2 initialScreenPoint;
        private Vector3 initialWorldPoint;
        private Transform touchedStrikerTransform;

        // Coin drag state
        private bool isTouchingCoins;
        private Vector2 initialCoinScreenPoint;
        private Vector3 initialCoinWorldPoint;
        private Transform touchedCoinTransform;
        private float _lastCoinScreenX;
        private Camera _mainCam;
        [Tooltip("Max distance for the raycast.")]
        public float maxRayDistance = 10f;

        [Header("Drag / Swipe Settings")]
        [Tooltip("Multiplier for how far dragging translates to normalized force (higher = less force for same drag).")]
        public float dragSensitivity = 5f;

        [Tooltip("Sensitivity mapping from horizontal swipe pixels -> rotation degrees for coins.")]
        public float coinRotateSensitivity = 0.2f;

        // Actions
        public Action PlatformRotatedEvent;

        private void Awake()
        {
            _mainCam = Camera.main;
        }

        private void OnEnable()
        {
            // Subscribe to InputData events
            inputData.RotationPinchStartedEvent += OnPinchStarted;
            inputData.RotationPinchContinuedEvent += OnPinchContinued;
            inputData.RotationPinchEndedEvent += OnPinchEnded;

            inputData.RotateStrikerEvent += RotateStriker;
            inputData.FireStrikeEvent += FireStriker;

      
        }

        private void OnDisable()
        {
            // Unsubscribe to prevent memory leaks
            inputData.RotationPinchStartedEvent -= OnPinchStarted;
            inputData.RotationPinchContinuedEvent -= OnPinchContinued;
            inputData.RotationPinchEndedEvent -= OnPinchEnded;

            inputData.RotateStrikerEvent -= RotateStriker;
            inputData.FireStrikeEvent -= FireStriker;
        }

        private void FireStriker(float val)
        {
            strikerShooting.FireStriker(val);
        }

        private void RotateStriker(Vector2 dir)
        {
            SetStriker();
            int id = playerData.GetMainPlayer().playerProperties.myId;
            Vector3 forward = tableData.GetCanvasTransform(id).forward;
            Vector3 right = tableData.GetCanvasTransform(id).right;
            Vector3 viewDirection = strikerData.currentStriker.transform.position + forward * dir.y + right * dir.x;

            strikerMovement.RotateTo(viewDirection);
            strikerShooting.SetStrikerForce(dir.magnitude); // use joystick magnitude as normalized force (0..1)

        }

        private void OnPinchStarted(Vector2 screenPosition)
        {
            if (inputData.isInputLocked) return;

            SetStriker();
            // 1) Try striker first
            if (TryRaycastForStriker(screenPosition, out RaycastHit hit, out Transform striker))
            {
                // Begin touch on striker
                CancelCurrentTouch(); // reset safety
                isTouchingStriker = true;
                initialScreenPoint = screenPosition;
                initialWorldPoint = hit.point;
                touchedStrikerTransform = striker;

          
                return;
            }
        }
        private void CancelCurrentTouch()
        {
            ResetTouchState();
        }

        private void ResetTouchState()
        {
            isTouchingStriker = false;
            touchedStrikerTransform = null;
            initialScreenPoint = Vector2.zero;
            initialWorldPoint = Vector3.zero;

            isTouchingCoins = false;
            touchedCoinTransform = null;
            initialCoinScreenPoint = Vector2.zero;
            initialCoinWorldPoint = Vector3.zero;
            _lastCoinScreenX = 0f;
        }

        private void OnPinchContinued(Vector2 screenPosition)
        {

            // Striker handling
            if (isTouchingStriker)
            {
                // Project current screen point onto horizontal plane at initialWorldPoint.y
                Camera cam = Camera.main;
                if (cam == null) return;

                Plane plane = new Plane(Vector3.up, initialWorldPoint);
                Ray ray = cam.ScreenPointToRay(screenPosition);
                Vector3 currentWorldPoint;
                if (plane.Raycast(ray, out float enter) && enter > 0f)
                {
                    currentWorldPoint = ray.GetPoint(enter);
                }
                else
                {
                    currentWorldPoint = ray.GetPoint(maxRayDistance);
                }

                Vector3 delta = currentWorldPoint - initialWorldPoint;
                Vector3 direction = (initialWorldPoint - currentWorldPoint).normalized;

                // Publish aiming
                strikerMovement.AimStriker(direction);

       
                float strikerRadius = boardData.StrikerRadius;

                float maxDragDistance = Mathf.Max(0.001f, dragSensitivity * strikerRadius); // avoid div by zero
                float dragDistance = delta.magnitude;

                //Debug.Log("Max Drag Dist: "  + maxDragDistance + " | Drag Dist: " + dragDistance);
                float normalizedForce = Mathf.Clamp01(dragDistance / maxDragDistance);

                // publish normalized force (0..1)
                strikerShooting.SetStrikerForce(normalizedForce);
                return;
            }
        }

        private void OnPinchEnded(Vector2 screenPosition)
        {
            if (isTouchingStriker)
            {
                Camera cam = Camera.main;
                if (cam == null)
                {
                    ResetTouchState();
                    return;
                }

                Plane plane = new Plane(Vector3.up, initialWorldPoint);
                Ray ray = cam.ScreenPointToRay(screenPosition);
                Vector3 currentWorldPoint;
                if (plane.Raycast(ray, out float enter) && enter > 0f)
                {
                    currentWorldPoint = ray.GetPoint(enter);
                }
                else
                {
                    currentWorldPoint = ray.GetPoint(maxRayDistance);
                }

                Vector3 delta = currentWorldPoint - initialWorldPoint;

       
               float strikerRadius = boardData.StrikerRadius;

                float maxDragDistance = Mathf.Max(0.001f, dragSensitivity * strikerRadius); // avoid div by zero
                float dragDistance = delta.magnitude;
                float normalizedForce = Mathf.Clamp01(dragDistance / maxDragDistance);

      

                strikerShooting.FireStriker(normalizedForce);
               

                ResetTouchState();
                return;
            }
        }

        private void SetStriker()
        {
            // Fetching references from TableData/StrikerData
            striker = strikerData.currentStriker;

            if (tableData.platform != null)
                Platform = tableData.platform.gameObject;

            AllAssets = tableData.AllAssets;

            if (striker != null)
            {
                strikerShooting = striker.GetComponent<StrikerShooting>();
                strikerMovement = striker.GetComponent<StrikerMovement>();
            }
        }

        /// <summary>
        /// Raycast from screen point and determine if a striker was hit directly OR a board was hit
        /// and a striker is overlapping the hit point (using OverlapSphere).
        /// Returns true if a striker interaction should begin. Out parameters contain the striker transform (if any).
        /// </summary>
        private bool TryRaycastForStriker(Vector2 screenPoint, out RaycastHit hitInfo, out Transform strikerTransform)
        {
            strikerTransform = null;
            hitInfo = default;

            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out hitInfo, maxRayDistance))
            {
                GameObject hitObject = hitInfo.transform.gameObject;
                if (hitObject != null)
                {
                    // direct striker hit
                    if (hitObject.CompareTag("Striker"))
                    {
                        strikerTransform = hitInfo.transform;
                        return true;
                    }

                    // board hit -> check overlap sphere for any striker colliders near the hit point
                    if (hitObject.CompareTag("Board"))
                    {
                        float strikerRadius = boardData.StrikerRadius;

                        float checkRadius = strikerRadius * 3.5f;
                        Collider[] cols = Physics.OverlapSphere(hitInfo.point, checkRadius);
                        foreach (var c in cols)
                        {
                            if (c == null) continue;
                            if (c.gameObject.CompareTag("Board")) continue;

                            var striker = c.GetComponentInParent<StrikerMovement>();
                            if (striker != null)
                            {
                                strikerTransform = striker.transform;
                                return true;
                            }

                            if (c.gameObject.CompareTag("Striker"))
                            {
                                strikerTransform = c.transform;
                                return true;
                            }
                        }

                        return false;
                    }

                }
            }

            return false;
        }

        // Keep these for external/legacy calls if needed
        public void RotateTo(Vector3 direction) => strikerMovement?.RotateTo(direction);
        public void RotateRelative(float angle) => strikerMovement?.RotateRelative(angle);
        public void StartStrike(float value) => strikerShooting?.StartStrike(value);
        public void EndStrike(float value) => strikerShooting?.EndStrike(value);
    }
}