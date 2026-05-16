using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;
// Added for Google Play Games Services
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

namespace com.VisionXR.Controllers
{
    public class LeaderBoardManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public LeaderBoardSO leaderboard;

        private void OnEnable()
        {
            leaderboard.GetMyPointsEvent += GetMyPoints;
            leaderboard.SetMyPointsEvent += WriteToLeaderBoard;
            leaderboard.GetTop10EntriesEvent += GetTopTenEntries;
        }

        private void OnDisable()
        {
            leaderboard.GetMyPointsEvent -= GetMyPoints;
            leaderboard.SetMyPointsEvent -= WriteToLeaderBoard;
            leaderboard.GetTop10EntriesEvent -= GetTopTenEntries;
        }

        private void SetPoints(int id)
        {
            // Handle parsing your event if needed
        }

        /// <summary>
        /// Writes/Reports score to specified GPGS Leaderboard ID
        /// </summary>
        public void WriteToLeaderBoard(string apiName, int points)
        {
            try
            {
                if (!PlayGamesPlatform.Instance.IsAuthenticated()) return;

                // GPGS handles overriding lower scores automatically based on console configuration
                // If you want to accumulate points manually instead, calculate it via local cached values first
                int finalScore = leaderboard.GetPointsByApiName(apiName) + points;

                Social.ReportScore(finalScore, apiName, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log($"Successfully reported score: {finalScore} to leaderboard: {apiName}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to report score to leaderboard: {apiName}");
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Error writing to leaderboard: " + e.Message);
            }
        }

        /// <summary>
        /// Fetches top 10 global user entries
        /// </summary>
        public void GetTopTenEntries(string apiName)
        {
            if (!PlayGamesPlatform.Instance.IsAuthenticated()) return;

            // Load scores starting from the top entry globally
            PlayGamesPlatform.Instance.LoadScores(
                apiName,
                LeaderboardStart.TopScores,
                10,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                (LeaderboardScoreData data) =>
                {
                    ProcessTopEntriesCallback(data);
                }
            );
        }

        /// <summary>
        /// Iterates through your 3 configured API tracking configurations to find user data
        /// </summary>
        public void GetMyPoints()
        {
            if (!PlayGamesPlatform.Instance.IsAuthenticated()) return;

            foreach (var item in leaderboard.leaderBoardPoints)
            {
                string trackingApiName = item.apiName; // Capture reference variable for the asynchronous callback closure

                // CenteredOnPlayer extracts a batch of rows with the local user directly in focus
                PlayGamesPlatform.Instance.LoadScores(
                    trackingApiName,
                    LeaderboardStart.PlayerCentered,
                    1,
                    LeaderboardCollection.Public,
                    LeaderboardTimeSpan.AllTime,
                    (LeaderboardScoreData data) =>
                    {
                        ProcessUserPointsCallback(data, trackingApiName);
                    }
                );
            }
        }

        private void ProcessTopEntriesCallback(LeaderboardScoreData data)
        {
            List<string> names = new List<string>();
            List<int> ranks = new List<int>();
            List<int> points = new List<int>();

            if (data.Status == ResponseStatus.Success || data.Status == ResponseStatus.SuccessWithStale)
            {
                foreach (IScore score in data.Scores)
                {
                    names.Add(score.userID); // GPGS returns User ID string. For Display Names, use metadata or local user profile matching if loaded
                    ranks.Add(score.rank);
                    points.Add((int)score.value);
                }
            }
            else
            {
                Debug.LogError("GPGS failed to fetch top entries. Status code: " + data.Status);
            }

            leaderboard.ShowLeaderBoardData(names, ranks, points);
        }

        private void ProcessUserPointsCallback(LeaderboardScoreData data, string apiName)
        {
            if (data.Status == ResponseStatus.Success || data.Status == ResponseStatus.SuccessWithStale)
            {
                // Verify if the dataset populated valid local user structural records
                if (data.PlayerScore != null)
                {
                    int myValue = (int)data.PlayerScore.value;
                    int myRank = data.PlayerScore.rank;

                    leaderboard.SavePointsData(apiName, myValue);
                    leaderboard.SaveRankData(apiName, myRank);

                    Debug.Log($"GPGS loaded player score for leaderboard: {apiName}. Points: {myValue}, Rank: {myRank}");
                }
            }
            else
            {
                Debug.LogError($"GPGS failed loading player scores for leaderboard: {apiName}. Status: {data.Status}");
            }
        }
    }
}