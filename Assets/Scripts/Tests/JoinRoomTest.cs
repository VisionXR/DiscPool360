using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
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
    public NetworkManager networkManager;
    public ChangeDestinationView changeDestinationPanel;
    public DestinationPanelView destinationPanel;
    public Destination destination;

    [Header("Key Bindings (New Input System)")]
    public Key JoinDestinationKey = Key.J;
    public Key ChangeDestinationKey = Key.C;
    public Key LeaveRoomKey = Key.L;
    public Key ReconnectRoomKey = Key.R;


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

        if (kb[LeaveRoomKey].wasPressedThisFrame)
        {
            networkManager.LeaveRoom();
        }

        if(kb[ReconnectRoomKey].wasPressedThisFrame)
        {
            networkManager.ReConnect();
        }
    }


    public void JoinRoom()
    {
        uiData.ResetAllPanels();
        //destinationPanel.gameObject.SetActive(true);
        //destinationPanel.ConnectToDestination(destination);
    }

    public void ChangeDestination()
    {
        if(destinationData.currentDestination != null)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);       
            changeDestinationPanel.SetDestination(destination);
            uiData.uiManager.GoToState(StateName.ChangeDestinationState);
        }
    }
}
