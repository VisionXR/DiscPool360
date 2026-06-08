using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
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
    public BoardType boardType;
    public GameObject EdgeHighLight;
    public GameObject grabbableComponent;
    public List<GameObject> allEdgeColliders;

    [Header("Audio Triggers")]
    public AudioSource pickupTrigger;
    public AudioSource dropTrigger;

    [Header("Pinch Settings")]
    public float pinchThreshold = 0.1f;         // meters
    public float rotationSensitivity = 0.2f;    // Adjusted lower since frame deltas are more responsive

    private bool isPinchStarted = false;
    private Vector2 lastFramePosition;          // Changed to Vector2 to keep screen space math native
    private GameObject allAssets;


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

        uiData.SetBoardType(boardType);
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
        if (!grabbableComponent.activeInHierarchy) return;

        if (allAssets == null)
        {
            allAssets = tableData.allAssets;
        }



        isPinchStarted = true;
        lastFramePosition = startPosition; // Store the exact start position vector

        if (allAssets != null)
        {
            allAssets.transform.SetParent(this.transform, true);
        }

        TurnOnBoardHighlight();
        inputData.PlatformHighlight(true);
        tableData.PlatformRotationStarted();
    }

    private void PinchContinued(Vector2 position)
    {
        if (!isPinchStarted) return;

        // 1. Calculate how far the finger/pointer moved since the previous frame
        float deltaX = position.x - lastFramePosition.x;

        // 2. Compute rotation modifier step based purely on this frame's horizontal change
        float rotationDelta = deltaX * rotationSensitivity;

        // 3. Construct rotation step using modern Quaternions
        Quaternion rotationModifier = Quaternion.AngleAxis(-rotationDelta, Vector3.up);

        // Multiply step directly into current rotation object to keep updates seamless
        transform.rotation = transform.rotation * rotationModifier;

        // 4. Update memory cache position to evaluate the next frame correctly
        lastFramePosition = position;

        // 5. Fire off event metrics
        tableData.PlatformRotationChanged(transform.eulerAngles);
    }

    private void PinchEnded(Vector2 pos)
    {
        if (!isPinchStarted) return;

        if (allAssets != null)
        {
            allAssets.transform.SetParent(null, true);
        }

        isPinchStarted = false;

        TurnOffBoardHighlight();
        inputData.PlatformHighlight(false);
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

        if (!strikerData.isFoul)
        {
            strikerData.TurnOffRigidBody();
        }

        coinData.TurnOffRigidBodies();
        inputData.BoardGrabbed();
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
    }
}