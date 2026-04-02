using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData;
    public UIDataSO uiData;
    public GameDataSO gameData;
    public BoardDataSO boardData;
    public CoinDataSO coinData;
    public TableDataSO tableData;
    public StrikerDataSO strikerData;
    public PlayerDataSO playersData;

    [Header("Game Objects")]
    public GameObject EdgeHighLight;
    public GameObject grabbableComponent;
    public List<GameObject> allEdgeColliders;

    [Header("Audio Triggers")]
    public AudioSource pickupTrigger;
    public AudioSource dropTrigger;

    [Header("Pinch Settings")]
    public float pinchThreshold = 0.1f;         // meters
    public float rotationSensitivity = 1.0f;    // degrees per meter of horizontal pinch displacement

    private bool isPinchStarted = false;
    private Vector3 pinchStartPos;
    private float initialYRotation = 0f;
    private GameObject allAssets;
    private Player mainPlayer;
    private GameObject cam;

    public Action<Vector3> PlatformRotationChanged;

    private void OnEnable()
    {
        boardData.TurnOnInteractableEvent += TurnOnInteractable;
        boardData.TurnOffInteractableEvent += TurnOffInteractable;

        tableData.ResetPlatformEvent += ResetPlatform;

        inputData.RotationPinchStartedEvent += PinchStarted;
        inputData.RotationPinchContinuedEvent += PinchContinued;
        inputData.RotationPinchEndedEvent += PinchEnded;

        tableData.SetPlatform(this);
    }

    private void OnDisable()
    {
        boardData.TurnOnInteractableEvent -= TurnOnInteractable;
        boardData.TurnOffInteractableEvent -= TurnOffInteractable;

        tableData.ResetPlatformEvent -= ResetPlatform;

        inputData.RotationPinchStartedEvent -= PinchStarted;
        inputData.RotationPinchContinuedEvent -= PinchContinued;
        inputData.RotationPinchEndedEvent -= PinchEnded;

        tableData.SetPlatform(null);
    }

    private void PinchStarted(Vector2 startPosition)
    {
        // 1. Basic UI/Lock guards
        if (!grabbableComponent.activeInHierarchy || inputData.isInputLocked) return;

        if(allAssets == null)
        {
            allAssets = tableData.AllAssets;
        }

        // 2. Raycast from the 2D pinch/touch position
        Ray ray = Camera.main.ScreenPointToRay(startPosition);
        RaycastHit hit;

        // 3. Check if we hit the "Edge" tag
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Edge"))
            {
                // Start the rotation logic
                isPinchStarted = true;
                pinchStartPos = new Vector3(startPosition.x, 0, startPosition.y); // Use Y for Z in 3D math
                initialYRotation = transform.eulerAngles.y;

                // Parenting assets to platform so they rotate together
                if (allAssets != null)
                {
                    allAssets.transform.SetParent(this.transform, true);
                }

                TurnOnBoardHighlight();
                inputData.PlatformHighlight(true);
                tableData.PlatformRotationStarted();
            }
        }
    }

    private void PinchContinued(Vector2 position)
    {
        if (!isPinchStarted) return;

        // 1. Calculate the horizontal displacement in screen pixels
        // Moving right (positive) usually means rotating clockwise or counter-clockwise
        float deltaX = position.x - pinchStartPos.x;

        // 2. Compute rotation in degrees
        // Using deltaX * sensitivity allows for a "swipe to spin" feel
        float rotationDelta = deltaX * rotationSensitivity;

        // 3. Apply rotation relative to the initial rotation captured at Start
        float newY = initialYRotation - rotationDelta;

        // Apply the rotation to the platform transform
        Vector3 currentRot = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentRot.x, newY, currentRot.z);

        // 4. Notify other systems (like UI or Network) of the change
        tableData.PlatformRotationChanged(transform.eulerAngles);
    }

    private void PinchEnded(Vector2 pos)
    {
        if (!isPinchStarted) return;

        // 1. Detach assets so they are no longer children of the rotating platform
        // 'true' maintains their current world-space position/rotation
        if (allAssets != null)
        {
            allAssets.transform.SetParent(null, true);
        }

        // 2. Reset state flags
        isPinchStarted = false;

        // 3. Visuals and Physics cleanup
        TurnOffBoardHighlight();
        inputData.PlatformHighlight(false);

        // 4. Trigger event for game logic (e.g., checking if striker is in valid spot)
        tableData.PlatformRotationEnded();
    }

    public void ResetPlatform()
    {
        SetPlatformRotation(0f);
    }

    public void SetPlatformRotation(float yRotation)
    {
        Vector3 currentRotation = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentRotation.x, yRotation, currentRotation.z);
    }

    public void TurnOnInteractable()
    {
        grabbableComponent.SetActive(true);
    }

    public void TurnOffInteractable()
    {
        grabbableComponent.SetActive(false);
    }
    public void TurnOnBoardHighlight()
    {
        EdgeHighLight.SetActive(true);

        if(!strikerData.isFoul)
        {
            strikerData.TurnOffRigidBody();
        }

        coinData.TurnOffRigidBodies();
        inputData.BoardGrabbed();
       
      //  pickupTrigger.Play();
    }

    public void TurnOffBoardHighlight()
    {
        EdgeHighLight.SetActive(false);
        coinData.TurnOnRigidBodies();
        inputData.BoardReleased();

        if (!strikerData.isFoul)
        {
            strikerData.TurnOnRigidBody();
        }

      //  dropTrigger.Play();
    }
}
