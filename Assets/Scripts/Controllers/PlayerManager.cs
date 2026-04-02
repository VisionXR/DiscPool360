using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PlayerDataSO playerData;
        public TableDataSO tableData;

        [Header("Game Objects")]
        public GameObject playerPrefab;
        private void OnEnable()
        {
            playerData.DestroyAllPlayersEvent += DestroyPlayers;
            playerData.CreatePlayerEvent += CreatePlayer;
        }
        private void OnDisable()
        {
            playerData.DestroyAllPlayersEvent -= DestroyPlayers;
            playerData.CreatePlayerEvent -= CreatePlayer;
        }


        private void CreatePlayer(PlayerProperties p)
        {

            GameObject player1 = Instantiate(playerPrefab, transform);
            player1.transform.position = tableData.GetPlayerTransform(p.myId).position;
            player1.transform.rotation = tableData.GetPlayerTransform(p.myId).rotation;
            player1.GetComponent<Player>().InitializePlayer(p);
            player1.name = "Player " + p.myId;

        }

        private void DestroyPlayers()
        {
            // Iterate backwards to avoid index shifting issues
            for (int i = playerData.players.Count - 1; i >= 0; i--)
            {
                Player p = playerData.players[i];
                if (p != null)
                {
                    Destroy(p.gameObject);
                }
            }

            playerData.players.Clear();
        }
    }
}
