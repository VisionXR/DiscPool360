using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.VisionXR.HelperClasses
{
    public class LeaderBoardPanel : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public UserDataSO settings;
        public LeaderBoardSO leaderBoard;
        public AudioDataSO audioData;

        [Header(" Game Objects")]
     
        public TMP_Text myRankText;
        public TMP_Dropdown ApiDD;


        [Header(" Local Objects")]
        public List<GameObject> playerObjects;
        public List<TMP_Text> Names;
        public List<TMP_Text> Ranks;
        public List<TMP_Text> Points;
        

       
        public string apiName = "MultiPlayer";

        private void OnEnable()
        {
            leaderBoard.ShowLeaderBoardDataEvent += ShowLeaderBoard;

        }

        private void OnDisable()
        {
            leaderBoard.ShowLeaderBoardDataEvent -= ShowLeaderBoard;
        }

        public void ShowLeaderBoard(List<string> names, List<int> ranks, List<int> points)
        {
          
            myRankText.text = "My Rank : " + leaderBoard.GetRankByApiName(ApiDD.value);
            ClearAllText();
            for (int i = 0; i < names.Count; i++)
            {
                playerObjects[i].SetActive(true);
                Names[i].text = names[i];
                Ranks[i].text = ranks[i].ToString();
                Points[i].text = points[i].ToString();
            }
        }

        private void ClearAllText()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                playerObjects[i].SetActive(false);
                Names[i].text = "";
                Ranks[i].text = "";
                Points[i].text = "";
            }
        }


        public void ApiDDChanged(int value)
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
 
    }
}
