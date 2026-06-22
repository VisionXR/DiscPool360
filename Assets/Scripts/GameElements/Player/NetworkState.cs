using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkState : SimulationBehaviour, INetworkRunnerCallbacks
{
    [Header("Scriptable Objects")]
    public NetworkOutputDataSO networkOutputData;
    public UserDataSO UserData;
    public UIDataSO uiData;
    public PlayerDataSO playersData;

    [Header("Game Objects")]
    public GameObject NetworkPlayer;

    // local variables
    private NetworkObject myPlayer;

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log(" Connected to server ");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log(" Failed to Connected to server ");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log(" Disconnected from server " + reason.ToString());
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log(" Host left the game ");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player joined: " + player.PlayerId);

        // Ensure we only spawn our own local player avatar representation
        if (runner.LocalPlayer == player)
        {
            SpawnPlayer(player);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player left: " + player.PlayerId);
        if (Runner.LocalPlayer == player)
        {
            Debug.Log(" I Left");
        }
        else
        {
            Debug.Log("Other player left");
            networkOutputData.OnOpponentPlayerLeft();
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        networkOutputData.OnMainPlayerLeft();
        Debug.Log($"Fusion runner shut down due to: {shutdownReason}. Rejoin logic should take over if inviting.");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }



    public void SpawnPlayer(PlayerRef playerRef)
    {
        CreatePlayerForP1VsP2(playerRef);
    }

    public void DespawnPlayer()
    {
        if (myPlayer != null)
        {
            Runner.Despawn(myPlayer);
        }
    }

    public void CreatePlayerForP1VsP2(PlayerRef playerRef)
    {
        PlayerProperties p = new PlayerProperties();

        // 1. DETERMINE LOGICAL GAMEPLAY SEAT ID (FOOLPROOF REJOIN METHOD)
        // If the player is the Master Client of this Room, they are ALWAYS Player 1.
        // Even if they rejoin late and get assigned PlayerId 3 by the router, IsSharedModeMasterClient preserves identity.
        if (Runner.IsSharedModeMasterClient)
        {
            p.myId = 1;
            Debug.Log($"[Identity] Rejoined/Joined as Room Creator. Forcing Logical Game ID: 1 (Fusion Real ID was: {playerRef.PlayerId})");
        }
        else
        {
            p.myId = 2;
            Debug.Log($"[Identity] Joined as Guest Client. Forcing Logical Game ID: 2 (Fusion Real ID was: {playerRef.PlayerId})");
        }

        // 2. Map standard profile metrics
        p.myOculusID = UserData.MyOculusId;
        p.myName = UserData.MyName;
        p.imageURL = UserData.MyImageUrl;
        p.myPlayerControl = PlayerControl.Local;
        p.myPlayerType = PlayerType.Human;

        // 3. Map gameplay mode values
        if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
        {
            // Using our new assigned fixed logical IDs instead of mutable raw network values
            if (p.myId == 1)
            {
                p.myCoin = PlayerCoin.AllPool;
            }
            else
            {
                p.myCoin = PlayerCoin.AllPool;
            }
        }
        else // (For other modes like Carrom, etc.)
        {
            if (p.myId == 1)
            {
                p.myCoin = PlayerCoin.Red;
            }
            else
            {
                p.myCoin = PlayerCoin.Red;
            }
        }

        // 4. Instantiation & injection
        myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
        PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
        playerNetworkData.SetPlayerData(p);
    }
}