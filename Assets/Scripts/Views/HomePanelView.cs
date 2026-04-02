using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class HomePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public UIDataSO uiData;
        public GameDataSO gameData;
        public TableDataSO tableData;
        public DestinationSO destinationData;

        [Header("Game Objects")]
        public GameObject GamesPanel;
        public GameObject NavigationPanel;
        public GameObject singlePlayerPanel;
        public GameObject multiPlayerPanel;
        public GameObject internetToastPanel;

        [Header("Home Panels")]
        public List<GameObject> homePanels;
        public GameObject allAssets;


        private void OnEnable()
        {
            foreach (GameObject go in homePanels)
            {
                go.SetActive(false);
            }

            NavigationPanel.SetActive(true);
            GamesPanel.SetActive(true);
            allAssets.transform.rotation = Quaternion.identity;

            destinationData.ClearDestination();


        }


        public void SinglePlayerButtonPressed()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameType(GameType.SinglePlayer);
            singlePlayerPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void MultiPlayerButtonPressed()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                uiData.SetGameType(GameType.MultiPlayer);
                multiPlayerPanel.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(ShowInternetToast());
            }
        }

        public void TutorialButtonPressed()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameType(GameType.Tutorial);
            Destination d = new Destination
            {
                region = ServerRegion.asia,
                gameType = GameType.Tutorial,
                gameMode = GameMode.Pool,
                lobbyName = "",
                roomName = "",
                isJoinable = false
            };


            destinationData.ConnectToDestination(d, null, null);
            
        }

        private IEnumerator ShowInternetToast()
        {            
            internetToastPanel.SetActive(true);
            yield return new WaitForSeconds(2);
            internetToastPanel.SetActive(false);
        }
     
    }
}
