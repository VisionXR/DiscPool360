using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class WinningPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public GameDataSO gameData;
        public UIDataSO uiData;
        public PlayerDataSO playerData;

        [Header("Other Elements")]
        public TMP_Text GameModeText;

        [Header("Player Elements")]
        public TMP_Text Player1Name;
        public Image Player1Winner;
        public Image Player1Image;

        public TMP_Text Player2Name;
        public Image Player2Winner;
        public Image Player2Image;

        public void ShowWinner(int winnerId)
        {
            GameModeText.text = Enum.GetName(typeof(GameMode), uiData.currentGameMode);

            Player p1 = playerData.GetPlayerById(1);
            Player1Image.sprite = p1.playerProperties.myImage;
            Player1Name.text = p1.playerProperties.myName;

            Player p2 = playerData.GetPlayerById(2);
            Player2Image.sprite = p2.playerProperties.myImage;
            Player2Name.text = p2.playerProperties.myName;

            if(winnerId == 1)
            {
                Player1Winner.gameObject.SetActive(true);
                Player2Winner.gameObject.SetActive(false);
            }
            else
            {
                Player1Winner.gameObject.SetActive(false);
                Player2Winner.gameObject.SetActive(true);
            }
        }

        public void HomeBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
            gameData.ExitGame();
            gameObject.SetActive(false);
        }

        public void PlayAgainBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            gameData.PlayAgain();
            gameObject.SetActive(false);
        }

        
    }
}
