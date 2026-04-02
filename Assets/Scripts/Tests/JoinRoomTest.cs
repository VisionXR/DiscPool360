using com.VisionXR.ModelClasses;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinRoomTest : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public DestinationSO destinationData;
    public UIDataSO uiData;
    public AudioDataSO audioData;

    [Header("Other Objects")]
    public ChangeDestinationPanel changeDestinationPanel;
    public DestinationPanel destinationPanel;
    public Destination destination;

    [Header("Key Bindings (New Input System)")]
    public Key JoinDestinationKey = Key.J;
    public Key ChangeDestinationKey = Key.C;


    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;

    
        if (kb[JoinDestinationKey].isPressed)
        {
            JoinRoom();
        }

       
        if (kb[ChangeDestinationKey].isPressed)
        {
            ChangeDestination();
        }
    }


    public void JoinRoom()
    {
        uiData.ResetAllPanels();
        destinationPanel.gameObject.SetActive(true);
        destinationPanel.ConnectToDestination(destination);
    }

    public void ChangeDestination()
    {
        if(destinationData.currentDestination != null)
        {
            audioData.PlayAudio(com.VisionXR.Controllers.AudioClipType.ButtonClick);
            changeDestinationPanel.gameObject.SetActive(true);
            changeDestinationPanel.SetDestination(destination);
        }
    }
}
