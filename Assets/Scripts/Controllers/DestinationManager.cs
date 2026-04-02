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
        destinationData.ClearDestinationEvent += ClearDestination;

        RoomCreateSuccessEvent += RoomCreationSuccess;
        RoomCreateFailedEvent += RoomCreationFailed;

        RoomJoinSuccessEvent += RoomJoinSuccess;
        RoomJoinFailedEvent += RoomJoinFailed;

    }

    private void OnDisable()
    {
        destinationData.ConnectToDestinationEvent -= ConnectToDestination;
        destinationData.ClearDestinationEvent -= ClearDestination;

        RoomCreateSuccessEvent -= RoomCreationSuccess;
        RoomCreateFailedEvent -= RoomCreationFailed;

        RoomJoinSuccessEvent -= RoomJoinSuccess;
        RoomJoinFailedEvent -= RoomJoinFailed;

        ClearDestination();
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
            SetDestination(currentDestination);
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
            SetDestination(currentDestination);
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
        SetDestination(currentDestination);
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
        SetDestination(currentDestination);
        destinationData.currentDestination = currentDestination;
        OnDestinationSuccessEvent?.Invoke();
     

    }

    public void RoomJoinFailed(string message)
    {
        OnDestinationFailedEvent?.Invoke(message);

    }

    public void SetDestination(Destination destination)
    {
        if (!Application.isEditor)
        {
            string destinationApiName = "";

            if (destination.gameType == GameType.SinglePlayer)
            {
                destinationApiName = "PvsAI_" + Enum.GetName(typeof(GameMode), destination.gameMode);
            }
            else if (destination.gameType == GameType.MultiPlayer)
            {
                destinationApiName = "P1vsP2_" + Enum.GetName(typeof(GameMode), destination.gameMode);
            }
            else if (destination.gameType == GameType.Tutorial)
            {
                destinationApiName = "Tutorial";
            }

           
            //GroupPresenceOptions groupPresenceOptions = new GroupPresenceOptions();

            //LinkData linkData = new LinkData();
            //linkData.gameType = destination.gameType;
            //linkData.gameMode = destination.gameMode;
            //linkData.roomName = destination.roomName;

            //groupPresenceOptions.SetDestinationApiName(destinationApiName);
            //groupPresenceOptions.SetLobbySessionId(destination.lobbyName);
            //groupPresenceOptions.SetMatchSessionId(JsonUtility.ToJson(linkData));
            //groupPresenceOptions.SetIsJoinable(destination.isJoinable);

          
            //TrySetPresence(groupPresenceOptions);
        }
        else
        {
            string destinationApiName = "";

            if (destination.gameType == GameType.SinglePlayer)
            {
                destinationApiName = "PvsAI_" + Enum.GetName(typeof(GameMode), destination.gameMode);
            }
            else if (destination.gameType == GameType.MultiPlayer)
            {
                destinationApiName = "P1vsP2_" + Enum.GetName(typeof(GameMode), destination.gameMode);
            }
            else if (destination.gameType == GameType.Tutorial)
            {
                destinationApiName = "Tutorial";
            }

            Debug.Log(" Group Presence set " + destinationApiName + "IsJoinable " + destination.isJoinable);
        }
    }

    //void TrySetPresence(GroupPresenceOptions groupPresenceOptions, int attempts = 0)
    //{
    //    const int maxAttempts = 5;
    //    const float retryDelaySeconds = 1f;

    //    attempts++;

    //    GroupPresence.Set(groupPresenceOptions).OnComplete(msg =>
    //    {
    //        if (msg.IsError)
    //        {
    //            var err = msg.GetError();
    //            Debug.LogWarning($"Failed to set GroupPresence (attempt {attempts}): {err?.Message} (Code: {err?.Code})");

    //            if (attempts < maxAttempts)
    //            {
    //                // schedule a retry after a short delay
    //                StartCoroutine(RetryAfterDelay(groupPresenceOptions, attempts, retryDelaySeconds));
    //            }
    //            else
    //            {
    //                Debug.LogError($"Give up setting GroupPresence after {attempts} attempts: {err?.Message}");
                 
    //            }

    //            return;
    //        }

    //    });
    //}

    //private IEnumerator RetryAfterDelay(GroupPresenceOptions groupPresenceOptions, int attempts, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    TrySetPresence(groupPresenceOptions, attempts);
    //}

    public void ClearDestination()
    {
        
     //   GroupPresence.Clear();
    }

    private void ResetManagers()
    {
        singlePlayerManager.gameObject.SetActive(false);
        multiPlayerManager.gameObject.SetActive(false);
        tutorialManager.gameObject.SetActive(false);
    }
}
