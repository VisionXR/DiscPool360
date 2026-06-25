using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using Fusion.Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class NetworkManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public NetworkOutputDataSO networkOutputData;
    public NetworkInputDataSO networkInputData;
    public PhotonAppSettings settings;
    public UIDataSO uiData;
    public UserDataSO userData;
    public PlayerDataSO playerData;

    [Header("Game Objects")]
    public string currentRoomName;
    public GameObject NetworkRunnerObject; // Prefab for creating a network runner instance
    private NetworkRunner runner;
    private bool isInvitingFriend = false;


    private void OnEnable()
    {
        networkInputData.CreateRoomEvent += CreateRoom;
        networkInputData.JoinRoomEvent += JoinRoom;
        networkInputData.LeaveRoomEvent += LeaveRoom;
        networkInputData.SetTimeOutEvent += SetTimeOut;
     
    }

    private void OnDisable()
    {
        networkInputData.CreateRoomEvent -= CreateRoom;
        networkInputData.JoinRoomEvent -= JoinRoom;
        networkInputData.LeaveRoomEvent -= LeaveRoom;
        networkInputData.SetTimeOutEvent -= SetTimeOut;
    }

    // --- FIX: Removed old PUN properties, optimized for Fusion ---

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Debug.Log("[NetworkManager] App focused. Reconnecting.");
            StartCoroutine(ReconnectRoom());
        }
    }

    public void ReConnect()
    {
        isInvitingFriend = true;
        StartCoroutine(ReconnectRoom());
    }

    private IEnumerator ReconnectRoom()
    {
        yield return new WaitForSeconds(2);
        if (isInvitingFriend)
        {
            isInvitingFriend = false;
            Debug.Log("[NetworkManager] Trying to rejoin ." + currentRoomName);
            Player p = playerData.GetMainPlayer();
            if (p == null)
            {
                Debug.Log("[NetworkManager] Rejoining ");
                RejoinLastSession();
            }
            else
            {
                Debug.Log("[NetworkManager] Player is not null.");
                
            }
        }
    }


    public void LeaveRoom()
    {
        if (runner != null)
        {
            runner.Shutdown();
            Destroy(runner.gameObject);
            runner = null;
            
        }
    }

    public void SetTimeOut(int time)
    {
        Debug.Log("[NetworkManager] Setting timeout to: " + time);
        isInvitingFriend = true;
    }

    /// <summary>
    /// Sets the server region for the network session based on the provided enum.
    /// </summary>
    private void SetRegion(ServerRegion region)
    {
        string regionName = region == ServerRegion.any ? "" : region.ToString().ToLower();
        settings.AppSettings.FixedRegion = regionName;
    }


    /// <summary>
    /// Creates a new room and starts a game session.
    /// </summary>
    private void CreateRoom(ServerRegion region, string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
    {
        SetRegion(region);
        StartGame(roomName, RoomSuccessEvent, RoomFailedEvent);
    }


    /// <summary>
    /// Joins a specific room by name.
    /// </summary>
    private void JoinRoom(ServerRegion region, string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
    {
        SetRegion(region);
        JoinGame(roomName, RoomSuccessEvent, RoomFailedEvent);
    }


    /// <summary>
    /// Starts a new game session, setting properties and configurations based on UI and player settings.
    /// </summary>
    public async Task StartGame(string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
    {
        InitializeNetworkRunner();

        var customRoomProps = new Dictionary<string, SessionProperty>
                {
                    { "gameType", (int)uiData.currentGameType },
                    { "gamemode", (int)uiData.currentGameMode },
                    { "board", (int)userData.myBoard },
                   
                };

        Fusion.GameMode gameMode = Fusion.GameMode.Shared;

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            SessionProperties = customRoomProps,
            IsVisible = false,
            AuthValues = new AuthenticationValues(userData.MyOculusId.ToString()),
            CustomLobbyName = "DiscPoolLobby",
            PlayerCount = 2,         
            SessionName = UnityEngine.Random.Range(10000, 99999).ToString() // Generate a random session name if not provided
        });

        if (result.Ok)
        {
            networkOutputData.SetHost(true);
            currentRoomName = runner.SessionInfo.Name;

            RoomSuccessEvent?.Invoke();
        }
        else
        {

            RoomFailedEvent?.Invoke("Could not create room ");
        }

    }

    /// <summary>
    /// Joins an existing game session.
    /// </summary>
    public async Task JoinGame(string roomName, Action RoomSuccessEvent, Action<string> RoomFailedEvent)
    {
        InitializeNetworkRunner();

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = Fusion.GameMode.Shared,
            CustomLobbyName = "DiscPoolLobby",
            SessionName = roomName,
         
        });

        if (result.Ok)
        {

            ReadRoomSessionProperties();
            currentRoomName = roomName;
            networkOutputData.SetHost(false);
            RoomSuccessEvent?.Invoke();
        }
        else
        {
            RoomFailedEvent?.Invoke("Could not join room ");
        }
    }

    /// <summary>
    /// Reconnects and rejoins the last active session if dropped due to backgrounding.
    /// </summary>
    public async Task RejoinLastSession()
    {
        Debug.Log("[Fusion Rejoin] Attempting to rejoin last session: " + currentRoomName);

        if (string.IsNullOrEmpty(currentRoomName))
        {
            Debug.Log("Cannot rejoin: Last session name is null or empty.");
            return;
        }

        
        InitializeNetworkRunner();

       
        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = Fusion.GameMode.Shared,
            CustomLobbyName = "DiscPoolLobby",
            SessionName = currentRoomName,
            AuthValues = new AuthenticationValues(userData.MyOculusId.ToString())
        });

        if (result.Ok)
        {
            Debug.Log("[Fusion Rejoin] Rejoined successfully!");
          //  ReadRoomSessionProperties();
        }
        else
        {
            Debug.LogError($"[Fusion Rejoin] Failed to rejoin: {result.ShutdownReason}");
        }
    }


    public void ReadRoomSessionProperties()
    {
        if (runner != null && runner.SessionInfo != null && runner.SessionInfo.Properties != null)
        {
            var props = runner.SessionInfo.Properties;

            if (props.TryGetValue("gameType", out var gameTypeProp))
            {
                uiData.SetGameType((GameType)(int)gameTypeProp);
            }

            if (props.TryGetValue("gameMode", out var gameMode))
            {
                Debug.Log(" Game mode from session properties: " + gameMode);
                uiData.SetGameMode((com.VisionXR.HelperClasses.GameMode)(int)gameMode);
            }

            if (props.TryGetValue("board", out var boardProp))
            {
                userData.SetBoard((int)boardProp);
            }


            Debug.Log(" Session properties received and assigned.");
        }
        else
        {
            Debug.LogWarning("SessionInfo or Properties are null, cannot read session properties.");
        }
    }

    /// <summary>
    /// Initializes the NetworkRunner object and assigns it to the runner variable.
    /// </summary>
    private void InitializeNetworkRunner()
    {
        if (runner != null)
        {
            Destroy(runner.gameObject);
        }

        GameObject tmpObject = Instantiate(NetworkRunnerObject, transform);
        runner = tmpObject.GetComponent<NetworkRunner>();
        runner.ProvideInput = true;
        networkOutputData.SetRunner(runner);
     

    }


}
