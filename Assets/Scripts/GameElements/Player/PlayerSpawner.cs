using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
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
   

    public void PlayerJoined(PlayerRef player)
    {
       


        if (Runner.LocalPlayer == player)
        {
            SpawnPlayer(player);
        }
        else
        {
            networkOutputData.OnPlayerJoined();
        }
           
    }

    public void PlayerLeft(PlayerRef player)
    {
   

        if (Runner.LocalPlayer == player)
        {      
            DespawnPlayer();
        }
        else
        {
           networkOutputData.OnPlayerLeft();
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