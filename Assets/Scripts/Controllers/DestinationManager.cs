using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;
using Application = UnityEngine.Application;

public class DestinationManager : MonoBehaviour
{
    [Header("ScriptableObjects")]
    public DestinationSO destinationData;
    public NetworkOutputDataSO networkOutputData;
    public NetworkInputDataSO networkInputData;
    public UIDataSO uiData;
    public UserDataSO userData;
    public GameDataSO gameData;

    [Header("Game Objects")]
    public Destination currentDestination;
    public SinglePlayerManager singlePlayerManager;
    public MultiPlayerManager multiPlayerManager;
    public TutorialManager tutorialManager;
    public NetworkManager networkManager;
    public com.VisionXR.Controllers.UIManager uiManager;  


    // Action
    public Action OnDestinationSuccessEvent, RoomCreateSuccessEvent,RoomJoinSuccessEvent;
    public Action<string> OnDestinationFailedEvent, RoomCreateFailedEvent,RoomJoinFailedEvent;


    private void OnEnable()
    {
        destinationData.ConnectToDestinationEvent += ConnectToDestination;
       

        RoomCreateSuccessEvent += RoomCreationSuccess;
        RoomCreateFailedEvent += RoomCreationFailed;

        RoomJoinSuccessEvent += RoomJoinSuccess;
        RoomJoinFailedEvent += RoomJoinFailed;

    }

    private void OnDisable()
    {
        destinationData.ConnectToDestinationEvent -= ConnectToDestination;
    

        RoomCreateSuccessEvent -= RoomCreationSuccess;
        RoomCreateFailedEvent -= RoomCreationFailed;

        RoomJoinSuccessEvent -= RoomJoinSuccess;
        RoomJoinFailedEvent -= RoomJoinFailed;

     
    }


    public void ConnectToDestination(Destination destination, Action OnSuccess, Action<string> OnFailure)
    {
        currentDestination = destination;

        OnDestinationSuccessEvent = OnSuccess;
        OnDestinationFailedEvent = OnFailure;

        uiData.SetGameMode(destination.gameMode);
        uiData.SetGameType(destination.gameType);

        ResetManagers();
      

        if (destination.gameType == GameType.MultiPlayer)
        {
            if (string.IsNullOrEmpty(destination.roomName))
            {
                networkInputData.CreateRoom(destination.region, RoomCreateSuccessEvent, RoomCreateFailedEvent);
            }
            else
            {
                networkInputData.JoinRoom(destination.region, destination.roomName, RoomJoinSuccessEvent, RoomJoinFailedEvent);
            }
        }

        else if (destination.gameType == GameType.SinglePlayer)
        {
          
            singlePlayerManager.gameObject.SetActive(true);
           
            currentDestination.lobbyName = "SinglePlayer";
            currentDestination.roomName = "SinglePlayer";
            currentDestination.isJoinable = false;
          
            destinationData.currentDestination = currentDestination;
            singlePlayerManager.StartGame(1);
            OnDestinationSuccessEvent?.Invoke();

           

        }

        else if (destination.gameType == GameType.Tutorial)
        {
            
            tutorialManager.gameObject.SetActive(true);
            currentDestination.lobbyName = "Tutorial";
            currentDestination.roomName = "Tutorial";
            currentDestination.isJoinable = false;
            uiManager.ShowTutorialPanel();
         
            destinationData.currentDestination = currentDestination;
            OnDestinationSuccessEvent?.Invoke();
           
        }

    }


    public void RoomCreationSuccess()
    {
      
        multiPlayerManager.gameObject.SetActive(true);
        currentDestination.lobbyName = networkOutputData.runner.SessionInfo.Region;
        currentDestination.roomName = networkOutputData.runner.SessionInfo.Name;

        currentDestination.isJoinable = true;
      
        destinationData.currentDestination = currentDestination;
        OnDestinationSuccessEvent?.Invoke();
      
    }

    public void RoomCreationFailed(string message)
    {
       OnDestinationFailedEvent?.Invoke(message);
        
    }

    public void RoomJoinSuccess()
    {

        multiPlayerManager.gameObject.SetActive(true);
        currentDestination.lobbyName = networkOutputData.runner.SessionInfo.Region;
        currentDestination.roomName = networkOutputData.runner.SessionInfo.Name;
        currentDestination.isJoinable = false;
    
        destinationData.currentDestination = currentDestination;
        OnDestinationSuccessEvent?.Invoke();
     

    }

    public void RoomJoinFailed(string message)
    {
        OnDestinationFailedEvent?.Invoke(message);

    }

    private void ResetManagers()
    {
        singlePlayerManager.gameObject.SetActive(false);
        multiPlayerManager.gameObject.SetActive(false);
        tutorialManager.gameObject.SetActive(false);
    }
}
