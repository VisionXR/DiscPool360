using System;
using UnityEngine;
using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using com.VisionXR.GameElements;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO")]
    public class PlayerDataSO : ScriptableObject
    {
        [Header("Physics Settings")]
        public int SendRate = 7;
        public int DelayRate = 2;
        public float strikerK = 1.457f;
        public float strikerangularK = 1.457f;
        public float coinK = 1.391f;
        public float coinangularK = 1.391f;
       

        // variables
        public List<Player> players = new List<Player>();

        // Actions

        public Action<PlayerProperties> CreatePlayerEvent;
        public Action<int> DestroyPlayerEvent;
        public Action DestroyAllPlayersEvent;
        public Action<int, Sprite> PlayerImageReceivedEvent;


        //Methods

        public void PlayerImageReceived(int playerId, Sprite playerImage)
        {
            PlayerImageReceivedEvent?.Invoke(playerId, playerImage);
        }

        public void CreatePlayer(PlayerProperties properties)
        {
            CreatePlayerEvent?.Invoke(properties);
        }

        public void DestroyPlayer(int playerId)
        {
            DestroyPlayerEvent?.Invoke(playerId);
        }

        public void DestroyAllPlayers()
        {
            DestroyAllPlayersEvent?.Invoke();
        }

        public void AddPlayer(Player player)
        {
              players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
              players.Remove(player);
        }

        public Player GetPlayerById(int playerId)
        {
            return players.Find(p => p.playerProperties.myId == playerId);
        }

        public Player GetMainPlayer()
        {
            return players.Find(p => p.playerProperties.myPlayerControl == PlayerControl.Local && p.playerProperties.myPlayerType == PlayerType.Human);
        }

        public Player GetOpponentPlayer()
        {
            return players.Find(p => p.playerProperties.myPlayerControl == PlayerControl.Remote && p.playerProperties.myPlayerType == PlayerType.Human);
        }
    }
}
