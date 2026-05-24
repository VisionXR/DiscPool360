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
        public float heightLift = 0.25f;
        public float placeRadius = 0.25f;
        public float yOffset = 100f;

        // local state
        public bool isPlacingStriker = false;


        [Header("Pinch Settings")]
        public float pinchThreshold = 0.1f;         // meters
        public float swipeSensitivity = 0.2f;    // Adjusted lower since frame deltas are more responsive

        private bool isPinchStarted = false;
        private Vector2 pinchStartPosition;
        private Vector2 lastFramePosition;


        private void OnEnable()
        {

            inputData.FoulPinchStartedEvent += PinchStarted;
            inputData.FoulPinchContinuedEvent += PinchContinued;
            inputData.FoulPinchEndedEvent += PinchEnded;

            uiData.PlaceStrikerEvent += FinalisePlacement;
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

            inputData.FoulPinchStartedEvent -= PinchStarted;
            inputData.FoulPinchContinuedEvent -= PinchContinued;
            inputData.FoulPinchEndedEvent -= PinchEnded;

            uiData.PlaceStrikerEvent -= FinalisePlacement;

            Reset();
        }

        private void Reset()
        {
            isPlacingStriker = false;
            isPinchStarted = false;
        }

        public void StartFoulHandling(int id)
        {

            if (currentPlayer.playerProperties.myPlayerType == PlayerType.AI)
            {
                // Auto-place for AI
                PlaceStrikerOnBoard();
            }
            else if (currentPlayer.playerProperties.myPlayerType == PlayerType.Human)
            {

                uiData.ShowFoulHandling();
                

                PlaceStrikerInAir();

                // Human places with pinch/trigger + vertical raycast
                isPlacingStriker = true;
                boardData.TurnOnInteractable();

                if (currentStriker != null)
                {
                    var rb = currentStriker.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.isKinematic = true; // keep kinematic while dragging
                    }
                }
                else
                {
                    currentStriker = strikerData.currentStriker;
                    var rb = currentStriker.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.isKinematic = true; // keep kinematic while dragging
                    }
                }
            }
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
                    Vector3 candidatePos = boardPosition + offset + Vector3.up * (boardLift);

                    if (CanPlaceOnBoard(candidatePos, strikerRadius))
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

        public void PlaceStrikerInAir()
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
                    Vector3 candidatePos = boardPosition + offset + Vector3.up * (heightLift);

                    if (CanPlaceInAir(candidatePos, strikerRadius))
                    {
                        currentStriker.transform.position = candidatePos;
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    currentStriker.transform.position = boardPosition + Vector3.up * (heightLift);
                }
            }
        }

        private bool CanPlaceOnBoard(Vector3 targetPosition, float strikerRadius)
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

        private bool CanPlaceInAir(Vector3 targetPosition, float strikerRadius)
        {
            // 2. SphereCast vertically downward to check for colliders underneath
            Vector3 castDirection = Vector3.down;


            // We use strikerRadius * 0.95f slightly smaller than the full radius 
            // to prevent the edges of the sphere from catching walls right next to it.
            bool hitSomethingBelow = Physics.SphereCast(
                targetPosition,
                strikerRadius * 1.1f,
                castDirection,
                out RaycastHit hitInfo      
            );

            if (hitSomethingBelow)
            {
                Debug.Log("Hit " + hitInfo.collider.gameObject.name);
            }
            else
            {
                Debug.Log("Not hit");
            }

            if(hitSomethingBelow && hitInfo.collider.CompareTag("Board"))
            {
                return true;
            }
            // If it hit a collider directly below, return false. Otherwise, return true.
            return false;
        }


        private void PinchStarted(Vector2 startPosition)
        {
            if (isPlacingStriker)
            {
             
                // Mark that the interaction has begun
                isPinchStarted = true;

                // Store the initial position for swipe direction calculation later
                pinchStartPosition = startPosition;

                // Initialize your frame-by-frame tracking variable
                lastFramePosition = startPosition;
            }
        }

        private void PinchContinued(Vector2 position)
        {
            if (!isPinchStarted) return;

            // 1. Calculate how far the finger/pointer moved since the previous frame
            Vector2 deltaPosition = position - lastFramePosition;

            // 2. Map the 2D screen delta to a 3D movement vector in the world plane
            // Adjust swipeSensitivity to control how fast the striker moves with your finger
            Vector3 movement3D = (currentPlayer.transform.right*deltaPosition.x+currentPlayer.transform.forward*deltaPosition.y) * swipeSensitivity;

            // 3. Calculate where the striker *wants* to go
            Vector3 proposedPosition = currentStriker.transform.position + movement3D;

            // 4. Check if there is anything directly below this new proposed position
            // You'll need to pass your striker's actual radius variable here
            if (CanPlaceInAir(proposedPosition, boardData.StrikerRadius))
            {
                // If the air underneath is clear, apply the movement!
                currentStriker.transform.position = proposedPosition;
            }
            else
            {
                // Optional: If blocked, you can handle collision feedback here 
                // (like stopping movement entirely, or sliding along the obstacle)
                Debug.LogWarning("Cannot move striker: Object detected below target position!");
            }

            // 5. Update memory cache position to evaluate the next frame correctly
            lastFramePosition = position;
        }

        private void PinchEnded(Vector2 pos)
        {
            if (!isPinchStarted) return;

            // Reset your state flag
            isPinchStarted = false;
        }

        private void FinalisePlacement()
        {
            if (isPinchStarted)
            {
                strikerData.SetFoul(false);
                strikerData.FoulComplete();
                Reset();

                
            }
        }
    }
}