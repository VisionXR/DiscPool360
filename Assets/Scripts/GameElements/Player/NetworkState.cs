using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using Fusion.Sockets;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

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

        Debug.Log(" Disconnected from server "+reason.ToString());
      //  networkData.DisconnectedFromServer(reason.ToString());
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
        Debug.Log("Player joined" + player.PlayerId);
        if (Runner.LocalPlayer == player)
        {
            SpawnPlayer(player);
        }
        else
        {
            networkOutputData.OnOpponentPlayerLeft();
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player left" + player.PlayerId);
        if (Runner.LocalPlayer == player)
        {
            Debug.Log(" I Left");
            // DespawnPlayer();
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
            Debug.Log("Fusion runner shut down due to timeout. Triggering rejoin sequence...");
    }



    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // --- APP WENT TO BACKGROUND ---
            Debug.Log("App minimized. Photon will hold connection as long as OS permits.");
        }
        else
        {
            // --- APP RETURNED TO FOREGROUND ---
            Debug.Log("App restored! Verifying Photon connection status...");

            // Check if we got dropped while we were away
            if (!PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                Debug.Log("Photon disconnected in background. Initiating fast reconnect/rejoin...");

            }
        }
    }

    public void SpawnPlayer(PlayerRef playerRef)
    {
        CreatePlayerForP1VsP2(playerRef);
    }


    public void DespawnPlayer()
    {
        Runner.Despawn(myPlayer);
    }


    public void CreatePlayerForP1VsP2(PlayerRef playerRef)
    {
        PlayerProperties p = new PlayerProperties();

        p.myId = playerRef.PlayerId;

        p.myOculusID = UserData.MyOculusId;
        p.myName = UserData.MyName;
        p.imageURL = UserData.MyImageUrl;

        p.myPlayerControl = PlayerControl.Local;
        p.myPlayerType = PlayerType.Human;


        if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
        {
            if (playerRef.PlayerId == 1)
            {

                p.myCoin = PlayerCoin.AllPool;

            }
            else
            {

                p.myCoin = PlayerCoin.AllPool;

            }
        }

        if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
        {
            if (playerRef.PlayerId == 1)
            {

                p.myCoin = PlayerCoin.Red;

            }
            else
            {

                p.myCoin = PlayerCoin.Red;

            }
        }

        myPlayer = Runner.Spawn(NetworkPlayer, Vector3.zero, Quaternion.identity);
        PlayerNetworkData playerNetworkData = myPlayer.GetComponent<PlayerNetworkData>();
        playerNetworkData.SetPlayerData(p);
    }
}
