using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Fusion;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class MultiPlayerConnectionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public NetworkInputDataSO networkInputData;
        public NetworkOutputDataSO networkOutputData;
        public PlayerDataSO playerData;
        public CoinDataSO coinData;
        public StrikerDataSO strikerData;

        [Header(" Ui Elements")]
        public GameObject lobbyPanel;
        public GameObject clientDisconnectedPanel;
        public bool isPlaying = false;



        private void OnEnable()
        {
            networkOutputData.OnPlayerJoinedEvent += OnPlayerJoined;
            networkOutputData.OnPlayerLeftEvent += OnPlayerLeft;
            ShowWaitingPanels();
            networkOutputData.SetHostReady(true);
            networkOutputData.SetClientReady(true);
        }

        private void OnDisable()
        {
            networkOutputData.OnPlayerJoinedEvent -= OnPlayerJoined;
            networkOutputData.OnPlayerLeftEvent -= OnPlayerLeft;
        }

        public void SetPlayStatus(bool playStatus)
        {
            isPlaying = playStatus;
        }


        public  void SendActivetCoinData()
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            ActiveCoinsData activeCoinsData = new ActiveCoinsData();
            for (int i = 0; i < coinData.AvailableCoinsInGame.Count; i++)
            {
                Rigidbody c = coinData.AvailableCoinsInGame[i];
                bool isActive = c != null && c.gameObject.activeInHierarchy;
                activeCoinsData.Status.Set(i, isActive);
            }
            networkData.SetActiveCoinsNetworkData(activeCoinsData);
        }

        public void SendStrikeForcheChanged()
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_PlayerStrikeForceStarted();
        }

        public void SendStrikeEnded()
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            ReceivePlayerData receivePlayerData = p.GetComponent<ReceivePlayerData>();
            receivePlayerData.PlayerStrikeEnded();
            networkData.RPC_PlayerStrikeEnded();
        }

        public void SendStrikeStarted()
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
           
            ReceivePlayerData receivePlayerData = p.GetComponent<ReceivePlayerData>();
            receivePlayerData.SendData(p.playerProperties.myId);

            networkData.RPC_PlayerStrikeStarted(strikerData.strikeForce,strikerData.strikerDir);
        }

        public void SendStrikeForceChanged(float force,Vector3 dir)
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();

            ReceivePlayerData receivePlayerData = p.GetComponent<ReceivePlayerData>();
           

            networkData.RPC_StrikeForceChanged(force,dir);
        }
        public void SendSnookerScore(int requiredColorIndex, SnookerPhase phase,int p1Score,int p2Score)
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_UpdateSnookerScore(requiredColorIndex, phase, p1Score, p2Score);
        }

        public void SendSetWinner(int id)
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_SetWinner(id);
        }

        public void SendFoul(int id)
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_SetFoul(id);
        }

        public void SendFoulComplete()
        {
            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_FoulComplete();
        }

        public void SendPlayerAssignedCoins(PlayerCoin coin1,PlayerCoin coin2)
        {

            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_SetAssignedCoins(coin1, coin2);
        }


        private void ShowWaitingPanels()
        {

            Reset();
            lobbyPanel.SetActive(true);
        }

        public void OnPlayerJoined()
        {
            
          

        }

        public void PlayAgain()
        {
            Reset();
            lobbyPanel.SetActive(true);

            if (networkOutputData.IsHostStatus())
            {
                Player p = playerData.GetMainPlayer();
                PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
                networkData.RPC_HostReady();    

            }
            else
            {
                Player p = playerData.GetMainPlayer();
                PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
                networkData.RPC_ClientReady();

            }
        }

        private void OnPlayerLeft()
        {
            if ((isPlaying))
            {
                Reset();
                isPlaying = false;
                clientDisconnectedPanel.SetActive(true);
                Debug.Log(" Player left after game starts");
            }
            else
            {

            }
           
        }

        private void Reset()
        {
            
            lobbyPanel.SetActive(false);
            clientDisconnectedPanel.SetActive(false);
        }

       
    }
}
