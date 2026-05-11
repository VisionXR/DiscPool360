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
        public UserDataSO userData;

        [Header("Other Elements")]
        public TMP_Text GameModeText;

        [Header("Player Elements")]
        public TMP_Text Player1Name;
        public Image Player1Winner;
        public Image Player1Image;

        public TMP_Text Player2Name;
        public Image Player2Winner;
        public Image Player2Image;


        private void OnEnable()
        {
            gameData.GameCompletedEvent += ShowWinner;
        }

        private void OnDisable()
        {
            gameData.GameCompletedEvent -= ShowWinner;
        }

        public void ShowWinner(int winnerId)
        {
            if (userData.myCoins == 0)
            {
                GameModeText.text = "8 Pool";
            }
            else if (userData.myCoins == 1)
            {
                GameModeText.text = "5 Pool";
            }
            else if (userData.myCoins == 2)
            {
                GameModeText.text = "10 Snooker";
            }

            else if (userData.myCoins == 3)
            {
                GameModeText.text = "6 Snooker";
            }
            else if (userData.myCoins == 4)
            {
                GameModeText.text = "Color Challenge";
            }

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
            uiData.uiManager.ChangeState("GameType", false);
            uiData.uiManager.ChangeState("Home", true);
            uiData.uiManager.ResetAllBools();
        }

        public void PlayAgainBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            gameData.PlayAgain();
            uiData.uiManager.ChangeState("MultiPlayerStartGame", false);
            uiData.uiManager.ChangeState("GameCompleted", false);
        }

        
    }
}
