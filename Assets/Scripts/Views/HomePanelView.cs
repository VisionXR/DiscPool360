using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
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
     

        [Header("Next Panels")]
        public SinglePlayerView singlePlayerView;
        public MultiPlayerPanelView multiPlayerPanel;
        public PanelOnOff internetToastPanel;

        [Header("Home Panels")]
        public List<PanelOnOff> panelsToOff;


        //local variables
        private GameObject allAssets;


        private void OnEnable()
        {

            allAssets = tableData.allAssets;
            allAssets.transform.rotation = Quaternion.identity;
            destinationData.ClearDestination();
            
        }


        public void SinglePlayerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.SetGameType(GameType.SinglePlayer);        
            TurnOff();
            singlePlayerView.TurnOn();
           
        }

        public void MultiPlayerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                uiData.SetGameType(GameType.MultiPlayer);
              
                TurnOff();
            }
            else
            {
                StartCoroutine(ShowInternetToast());
            }
        }

        public void TutorialBtnClicked()
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
            
        }

        private IEnumerator ShowInternetToast()
        {            
            internetToastPanel.TurnOnPanel();
            yield return new WaitForSeconds(2);
            internetToastPanel.TurnOffPanel();
        }

        public void TurnOff()
        {
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOffPanel();
            }
            StartCoroutine(WaitAndTurnOff());
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }


        public void TurnOn()
        {
           gameObject.SetActive(true);
            foreach (PanelOnOff panel in panelsToOff)
            {
                panel.TurnOnPanel();
            }
        }
    }
}
