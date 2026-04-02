using com.VisionXR.HelperClasses;
using Fusion;
using NUnit.Framework.Interfaces;
using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "NetworkOutputDataSO", menuName = "ScriptableObjects/NetworkOutputDataSO", order = 1)]
    public class NetworkOutputDataSO : ScriptableObject
    {
        // User Data

        public NetworkRunner runner;
        public bool isHost = false;

        public Action OnPlayerJoinedEvent;
        public Action OnPlayerLeftEvent;
        public Action<int> StartGameEvent;
        public Action HostReadyEvent;
        public Action ClientReadyEvent;

        public Action<int> SetWinnerEvent;
        public Action<PlayerCoin, PlayerCoin> SetPlayerCoinsEvent;
        public Action<int,SnookerPhase, int, int> UpdateSnookerScoreEvent;
        public Action UpdateCoinsEvent;

        // local
        private bool isHostReady = false;
        private bool isClientReady = false;


        // Methods

        public void OnEnable()
        {
            isHost = false;
        }

        public void UpdateCoins()
        {
            UpdateCoinsEvent?.Invoke();
        }

        public void UpdateSnookerScore(int requiredColorIndex, SnookerPhase phase, int player1Score, int player2Score)
        {
            UpdateSnookerScoreEvent?.Invoke(requiredColorIndex, phase, player1Score, player2Score);
        }

        public void SetPlayerCoins(PlayerCoin player1Coin, PlayerCoin player2Coin)
        {
            SetPlayerCoinsEvent?.Invoke(player1Coin, player2Coin);
        }
        public void SetWinner(int id)
        {
            SetWinnerEvent?.Invoke(id);
        }

        public void StartGame(int id)
        {
            StartGameEvent?.Invoke(id);
        }

        public void OnPlayerJoined()
        {
            OnPlayerJoinedEvent?.Invoke();
        }

        public void OnPlayerLeft()
        {
            OnPlayerLeftEvent?.Invoke();
        }

        public void SetRunner(NetworkRunner networkRunner)
        {
            runner = networkRunner;
        }

        public NetworkRunner GetRunner()
        {
            return runner;
        }

        public void SetHost(bool hostStatus)
        {
          
            isHost = hostStatus;
        }
        public bool IsHostStatus()
        {
            return isHost;
        }

        public void SetHostReady(bool value)
        {
            if (value)
            {
                HostReadyEvent?.Invoke();
            }
            isHostReady = value;
        }

        public void SetClientReady(bool value)
        {
            if(value)
            {
                ClientReadyEvent?.Invoke();
            }
            isClientReady = value;
        }

        public bool IsHostReady()
        {
            return isHostReady;
        }

        public bool IsClientReady()
        {
            return isClientReady;
        }
    }
}
