using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using Fusion.Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.Collections.Unicode;

public class NetworkManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public NetworkOutputDataSO networkOutputData;
    public NetworkInputDataSO networkInputData;
    public PhotonAppSettings settings;
    public UIDataSO uiData;
    public UserDataSO userData;

    [Header("Game Objects")]
    public GameObject NetworkRunnerObject; // Prefab for creating a network runner instance
    private NetworkRunner runner;


    private void OnEnable()
    {
        networkInputData.CreateRoomEvent += CreateRoom;
        networkInputData.JoinRoomEvent += JoinRoom;
        networkInputData.LeaveRoomEvent += LeaveRoom;
     
    }

    private void OnDisable()
    {
        networkInputData.CreateRoomEvent -= CreateRoom;
        networkInputData.JoinRoomEvent -= JoinRoom;
        networkInputData.LeaveRoomEvent -= LeaveRoom;
    }


    private void LeaveRoom()
    {
        if (runner != null)
        {
            runner.Shutdown();
            Destroy(runner.gameObject);
            runner = null;
        }
    }

    /// <summary>
    /// Sets the server region for the network session based on the provided enum.
    /// </summary>
    private void SetRegion(ServerRegion region)
    {
        string regionName = region == ServerRegion.any ? "" : region.ToString().ToLower();
        settings.AppSettings.FixedRegion = regionName;
    }

    public ServerRegion GetCurrentRegion()
    {
        string fixedRegion = settings.AppSettings.FixedRegion;
        if (string.IsNullOrEmpty(fixedRegion))
        {
            return ServerRegion.any;
        }
        foreach (ServerRegion region in Enum.GetValues(typeof(ServerRegion)))
        {
            if (region.ToString().Equals(fixedRegion, StringComparison.OrdinalIgnoreCase))
            {
                return region;
            }
        }
        return ServerRegion.any; // Default to 'any' if no match found
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
            SessionName = UnityEngine.Random.Range(1000, 9999).ToString() // Generate a random session name if not provided
        });

        if (result.Ok)
        {
            networkOutputData.SetHost(true);
         //   Debug.Log(" Room created successfully with name: " + roomName);
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

        Debug.Log(" Joining " + roomName);

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = Fusion.GameMode.Shared,
            CustomLobbyName = "DiscPoolLobby",
            SessionName = roomName
        });

        if (result.Ok)
        {

            ReadRoomSessionProperties();
            networkOutputData.SetHost(false);
            RoomSuccessEvent?.Invoke();
        }
        else
        {
            RoomFailedEvent?.Invoke("Could not join room ");
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
