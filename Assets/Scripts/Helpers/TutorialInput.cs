using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class TutorialInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public TutorialDataSO tutorialData;
        public BoardDataSO boardData;
        public StrikerDataSO strikerData;

        [Header("Game Objects")]
        public GameObject striker;
        public StrikerShooting strikerShooting;
        public StrikerMovement strikerMovement;


        // Foul
        [Header("Foul Variables")]
        public LineRenderer lineRenderer;
        public LayerMask placementLayerMask; // Layer mask to specify which layers to check for placement
        public float raycastDistance = 2.0f;
        public float boardLift = 0.01f;
        public float placeRadius = 0.25f;


        // local state
        private bool isPlacingStriker = false;
        private bool _isHeld = false; // pinch/trigger held state
        public Action<StrikerSnapshot> StrikerFoulPlacedEvent;

        private void OnEnable()
        {
            inputData.StartStrikeEvent += StartStrike;
            inputData.EndStrikeEvent += EndStrike;
       
            inputData.RotateStrikerRelativeEvent += RotateRelative;
            inputData.RotateStrikerTowardsEvent += RotateTo;


            Initialise();

        }


        private void OnDisable()
        {

            inputData.StartStrikeEvent -= StartStrike;
            inputData.EndStrikeEvent -= EndStrike;

            inputData.RotateStrikerRelativeEvent -= RotateRelative;
            inputData.RotateStrikerTowardsEvent -= RotateTo;



        }

        private void Initialise()
        {

            lineRenderer.startWidth = 0.03f;
            lineRenderer.endWidth = 0.03f;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        private void RotateTo(Vector3 direction)
        {
            if (strikerMovement != null && tutorialData.canIAim)
            {
                strikerMovement.RotateTo(direction);
            }
        }

        private void RotateRelative(float angle)
        {
            if (strikerMovement != null && tutorialData.canIAim)
            {
                strikerMovement.RotateRelative(angle);
            }
        }

        private void StartStrike(float value)
        {
            if (strikerShooting != null && tutorialData.canIFire)
            {
                striker.GetComponent<Rigidbody>().isKinematic = false;
                strikerShooting.StartStrike(value);
            }
        }

        private void EndStrike(float value)
        {

            if (strikerShooting != null && tutorialData.canIFire)
            {
                strikerShooting.EndStrike(value);
            }
        }

        private void PinchStarted(Vector3 origin)
        {
            if (tutorialData.canIPlaceStriker)
            {
                // Handle the pinch start while placing the striker
                _isHeld = true;
                TryPlaceWhileHeld(origin);

            }
        }

        private void PinchContinued(Vector3 origin)
        {
            if (_isHeld)
            {

                TryPlaceWhileHeld(origin);
            }
        }

        private void PinchEnded()
        {
            if (_isHeld && !isPlacingStriker)
            {
                FinalizePlacement();

            }
        }


        private void TryPlaceWhileHeld(Vector3 pointerWorldPosition)
        {


            // Raycast vertically down from pointer
            Ray ray = new Ray(pointerWorldPosition, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
            {
                if (hit.collider != null && hit.collider.CompareTag("Board"))
                {
                    float r = boardData.StrikerRadius;
                    Vector3 target = hit.point + Vector3.up * (boardLift);

                    if (CanPlaceAt(target, r))
                    {
                        lineRenderer.positionCount = 2;
                        lineRenderer.SetPosition(0, pointerWorldPosition);
                        lineRenderer.SetPosition(1, target);
                        striker.transform.position = target;
                        isPlacingStriker = false;

                        StrikerFoulPlacedEvent?.Invoke(new StrikerSnapshot
                        {
                            Position = target,
                            Rotation = striker.transform.eulerAngles,
                        });
                    }
                }

            }
        }
        private void FinalizePlacement()
        {
            // Re-enable physics and complete foul
            if (striker != null)
            {
                var rb = striker.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            strikerData.SetFoul(false);

            isPlacingStriker = false;
            _isHeld = false;
            lineRenderer.positionCount = 0;
            
            strikerData.FoulComplete();
        }

        private bool CanPlaceAt(Vector3 targetPosition, float strikerRadius)
        {
            // Check for overlapping colliders at the target position (ignore board and striker)
            Collider[] overlaps = Physics.OverlapSphere(targetPosition, strikerRadius * 1.1f, placementLayerMask);
            for (int i = 0; i < overlaps.Length; i++)
            {
                var col = overlaps[i];
                if (col == null) continue;

                // Any other collider blocks placement
                return false;
            }

            return true;
        }

        private void Reset()
        {
            isPlacingStriker = false;
            _isHeld = false;
        }
    }
}
