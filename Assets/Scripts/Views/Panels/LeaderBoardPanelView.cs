using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class LeaderBoardPanelView : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UserDataSO settings;
        public LeaderBoardSO leaderBoard;
        public AudioDataSO audioData;
        public UIDataSO uiData;

        [Header("Tab Objects")]
        public List<GameObject> SelectionImages;

        [Header("Panel Objects")]
        public string currentState;

        public string apiName = "MultiPlayer";

        private void OnEnable()
        {
            leaderBoard.ShowLeaderBoardDataEvent += ShowLeaderBoard;

        }

        private void OnDisable()
        {
            leaderBoard.ShowLeaderBoardDataEvent -= ShowLeaderBoard;
        }

        public void TabButtonClicked(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            ResetTabs();
            SelectionImages[id].SetActive(true);
            ApiNameChanged(0);
            
        }

        private void ResetTabs()
        {
            foreach (var img in SelectionImages)
            {
                img.SetActive(false);
            }
        }
        public void ShowLeaderBoard(List<string> names, List<int> ranks, List<int> points)
        {
          
          
        }

        private void ClearAllText()
        {
        
        }


        public void ApiNameChanged(int value)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if (value==0)
            {
                apiName = "MultiPlayer";
            }
            else if(value==1)
            {
                apiName = "SinglePlayer";
            }
            else if(value==2)
            {
                apiName = "TotalGames";
            }
     
        }


        public void RefreshBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            leaderBoard.GetMyPoints();
          
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.ChangeState(currentState, false);
        }

    }


}

