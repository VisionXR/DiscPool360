using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class PlayerFoul : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public StrikerDataSO strikerData;
        public BoardDataSO boardData;
        public InputDataSO inputData;
        public UIDataSO uiData;
        public UserDataSO userData;


        [Header("Game Objects")]
        public Player currentPlayer;
        public LineRenderer lineRenderer;
        public GameObject currentStriker;
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
            strikerData.HandleFoulEvent += StartFoulHandling;
            inputData.PinchStartedEvent += PinchStarted;
            inputData.PinchContinuedEvent += PinchContinued;
            inputData.PinchEndedEvent += PinchEnded;

            Initialise();
        }

        private void Initialise()
        {
            currentStriker = strikerData.currentStriker;
            lineRenderer.startWidth = 0.03f;
            lineRenderer.endWidth = 0.03f;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        private void OnDisable()
        {
            strikerData.HandleFoulEvent -= StartFoulHandling;
            inputData.PinchStartedEvent -= PinchStarted;
            inputData.PinchContinuedEvent -= PinchContinued;
            inputData.PinchEndedEvent -= PinchEnded;

            Reset();
        }

        private void Reset()
        {
            isPlacingStriker = false;
            _isHeld = false;
        }

        private void PinchStarted(Vector3 origin)
        {
            if (isPlacingStriker)
            {
                // Handle the pinch start while placing the striker
                _isHeld = true;
                TryPlaceWhileHeld(origin);

            }
        }

        private void PinchContinued(Vector3 origin)
        {
            if ( _isHeld)
            {
               
                TryPlaceWhileHeld(origin);
            }
        }

        private void PinchEnded()
        {
            if (_isHeld && !isPlacingStriker)
            {
                FinalizePlacement();

                boardData.TurnOffInteractable();
            }
        }

        private void StartFoulHandling(int id)
        {
            if (currentPlayer.playerProperties.myPlayerType == PlayerType.AI && currentPlayer.playerProperties.myId == id)
            {
                // Auto-place for AI
                PlaceStrikerOnBoard();
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
                        currentStriker.transform.position = target;
                        isPlacingStriker = false;

                        StrikerFoulPlacedEvent?.Invoke(new StrikerSnapshot
                        {
                            Position = target,
                            Rotation = currentStriker.transform.eulerAngles,
                        });
                    }
                }

            }
        }
        private void FinalizePlacement()
        {
            // Re-enable physics and complete foul
            if (currentStriker != null)
            {
                var rb = currentStriker.GetComponent<Rigidbody>();
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

        public void PlaceStrikerOnBoard()
        {
            if (currentStriker == null)
            {
                currentStriker = strikerData.currentStriker;
            }


            Rigidbody rb = currentStriker.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            GameObject board = boardData.Board;
            float strikerRadius = boardData.StrikerRadius;

            int steps = 16;

            if (board != null)
            {
                Vector3 boardPosition = board.transform.position;
                bool placed = false;

                for (int i = 0; i < steps; i++)
                {
                    float angle = i * Mathf.PI * 2f / steps;
                    Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * placeRadius;
                    Vector3 candidatePos = boardPosition + offset + Vector3.up *(boardLift);

                    if (CanPlaceAt(candidatePos, strikerRadius))
                    {
                        currentStriker.transform.position = candidatePos;
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    currentStriker.transform.position = boardPosition + Vector3.up * (boardLift);
                }

                if (rb != null) rb.isKinematic = false;
                currentStriker.transform.rotation = Quaternion.identity;

                strikerData.SetFoul(false);
                strikerData.FoulComplete();
            }
        }

        private bool CanPlaceAt(Vector3 targetPosition, float strikerRadius)
        {
            // Check for overlapping colliders at the target position (ignore board and striker)
            Collider[] overlaps = Physics.OverlapSphere(targetPosition, strikerRadius*1.1f,placementLayerMask);
            for (int i = 0; i < overlaps.Length; i++)
            {
                var col = overlaps[i];
                if (col == null) continue;

                // Any other collider blocks placement
                return false;
            }

            return true;
        }
    }
}