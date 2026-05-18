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

                Debug.Log("Final Score " + finalScore);

                Social.ReportScore(finalScore, apiName, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log($"Successfully reported score: {finalScore} to leaderboard: {apiName}");
                        //// This forces Android to slide up the live, real-time leaderboard overlay
                        //PlayGamesPlatform.Instance.ShowLeaderboardUI(apiName);

                        leaderboard.AddPoints(apiName, points); // Update local cache with new points after successful report
                    }
                    else
                    {
                        Debug.Log($"Failed to report score to leaderboard: {apiName}");
                    }
                });
            }
            catch (Exception e)
            {
                Debug.Log("Error writing to leaderboard: " + e.Message);
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
                LeaderboardCollection.Social,
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
                Debug.Log($"Attempting to load player-centered score for leaderboard: {trackingApiName}");
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
            if (data.Status != ResponseStatus.Success && data.Status != ResponseStatus.SuccessWithStale)
            {
                Debug.LogError("GPGS failed to fetch top entries. Status code: " + data.Status);
                // Fallback: send empty lists to clean the panel
                leaderboard.ShowLeaderBoardData(new List<string>(), new List<int>(), new List<int>());
                return;
            }

            // 1. Gather all unique User IDs from the loaded scores
            List<string> userIds = new List<string>();
            foreach (IScore score in data.Scores)
            {
                if (!string.IsNullOrEmpty(score.userID))
                {
                    userIds.Add(score.userID);
                }
            }

            // 2. Batch load profiles for all these user IDs from Google's servers
            Social.LoadUsers(userIds.ToArray(), (IUserProfile[] profiles) =>
            {
                // Map user IDs to their Display Names using a dictionary for quick lookup
                Dictionary<string, string> userIdToNameMap = new Dictionary<string, string>();

                if (profiles != null)
                {
                    foreach (IUserProfile profile in profiles)
                    {
                        if (!userIdToNameMap.ContainsKey(profile.id))
                        {
                            userIdToNameMap.Add(profile.id, profile.userName);
                        }
                    }
                }

                List<string> names = new List<string>();
                List<int> ranks = new List<int>();
                List<int> points = new List<int>();

                // 3. Match usernames to their ranks and scores
                foreach (IScore score in data.Scores)
                {
                    string displayName = "Unknown Player";

                    // Check if we successfully found a username matching this score's user ID
                    if (userIdToNameMap.TryGetValue(score.userID, out string mappedName))
                    {
                        displayName = mappedName;
                    }
                    else if (score.userID == Social.localUser.id)
                    {
                        // Simple shortcut fallback check if it's the local active player
                        displayName = Social.localUser.userName;
                    }
                    else
                    {
                        // Fallback text if user profile data is hidden by player privacy settings
                        displayName = $"Player_{score.userID.Substring(0, Mathf.Min(5, score.userID.Length))}";
                    }

                    names.Add(displayName);
                    ranks.Add(score.rank);
                    points.Add((int)score.value);
                }

                // 4. Send the compiled names, ranks, and points over to update your UI panels
                leaderboard.ShowLeaderBoardData(names, ranks, points);
            });
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