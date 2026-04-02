using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class MultiPlayerGameStart : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public GameDataSO gameData;
        public PlayerDataSO playerData;



        public void StartBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            SendPlayerData sendPlayerData = playerData.GetMainPlayer().GetComponent<SendPlayerData>();
            sendPlayerData.RPC_StartGame(1);
            gameObject.SetActive(false);
        }

    }
}
