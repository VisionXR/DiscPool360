//using com.VisionXR.GameElements;
//using com.VisionXR.HelperClasses;
//using com.VisionXR.ModelClasses;
//using Oculus.Platform;
//using Oculus.Platform.Models;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace com.VisionXR.Controllers
//{
//    public class LeaderBoardManager : MonoBehaviour
//    {
//        [Header(" Scriptable Objects")]
//        public LeaderBoardSO leaderboard;
//        public UserDataSO settings;
//        public GameDataSO gameData;
//        public PlayerDataSO playerData;
//        public DestinationSO destinationData;



//        private void OnEnable()
//        {
            
//            leaderboard.GetTop10EntriesEvent += GetTopTenEntries;
//            leaderboard.GetMyPointsEvent += GetMyPoints;

//            gameData.GameCompletedEvent += SetPoints;
//        }

//        private void OnDisable()
//        {
          
//            leaderboard.GetTop10EntriesEvent -= GetTopTenEntries;
//            leaderboard.GetMyPointsEvent -= GetMyPoints;

//            gameData.GameCompletedEvent -= SetPoints;
//        }

//        private void SetPoints(int id)
//        {
//            Player p = playerData.GetMainPlayer();

//            if (p != null && p.playerProperties.myId == id)
//            {
//                if(destinationData.currentDestination.gameType == GameType.SinglePlayer)
//                {
//                    WriteToLeaderBoard("SinglePlayer", 1);
//                }
//                else if (destinationData.currentDestination.gameType == GameType.MultiPlayer)
//                {
//                    WriteToLeaderBoard("MultiPlayer", 1);
//                }
//            }
//        }

//        public void WriteToLeaderBoard(string ApiName,int points)
//        {
//            try
//            {
//                Leaderboards.WriteEntry(ApiName, leaderboard.GetPointsByApiName(ApiName) + points);

//            }
//            catch (Exception e)
//            {
//                Debug.Log(" Some error " + e.Message);
//            }
//        }

//        public void GetTopTenEntries(string apiName, LeaderboardStartAt leaderboardStartAt)
//        {
//            Leaderboards.GetEntries(apiName, 10, LeaderboardFilterType.None, leaderboardStartAt).OnComplete(GetTop10Callback);
//        }



//        public void GetMyPoints()
//        {
//            foreach (var item in leaderboard.leaderBoardPoints)
//            {
//                Leaderboards.GetEntries(item.apiName, 1, LeaderboardFilterType.None, LeaderboardStartAt.CenteredOnViewer).OnComplete(msg => GetUserPoints(msg, item.apiName));
//            }

//        }


//        void GetTop10Callback(Message<LeaderboardEntryList> msg)
//        {
//            List<string> names = new List<string>();
//            List<int> ranks = new List<int>();
//            List<int> points = new List<int>();
//            if (!msg.IsError)
//            {
//                foreach (var entry in msg.Data)
//                {
//                    names.Add(entry.User.DisplayName);
//                    ranks.Add(entry.Rank);
//                    points.Add((int)entry.Score);

//                }
//            }

//            leaderboard.ShowLeaderBoardData(names, ranks, points);
//        }

//        void GetUserPoints(Message<LeaderboardEntryList> msg, string apiName)
//        {

//            if (!msg.IsError)
//            {
//                foreach (var entry in msg.Data)
//                {
//                    if (settings.MyOculusId == entry.User.ID)
//                    {
//                        leaderboard.SetMyPoints(apiName, (int)entry.Score);
//                        leaderboard.SetMyRank(apiName, entry.Rank);

//                    }
//                }
//            }
//        }

//    }
//}
