
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "LeaderBoardSO", menuName = "ScriptableObjects/LeaderBoardSO", order = 1)]
    public class LeaderBoardSO : ScriptableObject
    {
        // variables
        public List<LeaderBoardPoints> leaderBoardPoints;

        // Action
        //public Action<string, LeaderboardStartAt> GetTop10EntriesEvent;
        public Action GetMyPointsEvent;
   
        public Action<List<string>, List<int>, List<int>> ShowLeaderBoardDataEvent;


        // Methods

        //public void GetTop10Entries(string apiName, LeaderboardStartAt leaderboardStartAt)
        //{
        //    GetTop10EntriesEvent?.Invoke(apiName, leaderboardStartAt);
        //}

        public void GetMyPoints()
        {
            GetMyPointsEvent?.Invoke();
        }

        public void ShowLeaderBoardData(List<string> names,List<int> ranks,List<int> points)
        {
            ShowLeaderBoardDataEvent?.Invoke(names, ranks, points);
        }

        public int GetPointsByApiName(string apiName)
        {
            foreach (var item in leaderBoardPoints)
            {
                if (item.apiName == apiName)
                {
                    return item.wins;
                }
            }
            return 0;
        }

        public int GetRankByApiName(int id)
        {
          
            return leaderBoardPoints[id].rank;
        }
        public void SetMyPoints(string apiName, int points)
        {
            
            foreach (var item in leaderBoardPoints)
            {
                if (item.apiName == apiName)
                {
                    item.wins = points;                  
                    break;
                }
            }

        }

        public void SetMyRank(string apiName, int rank)
        {
            foreach (var item in leaderBoardPoints)
            {
                if (item.apiName == apiName)
                {
                    item.rank = rank;
                    break;
                }
            }
        }
    }


    [Serializable]
    public class  LeaderBoardPoints
    {
        public string apiName;
        public int wins;
        public int rank;
    }
}
