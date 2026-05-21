using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.IO;
using UnityEngine;
using GooglePlayGames;
using System.Collections;


public class AchievementManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;
    public GameDataSO gameData;
    public DestinationSO destinationData;
    public CloudDataSO cloudData;

    [Header("Local Objects")]
    public string userDataKey = "DiscPoolUserData";
    public string boardWinsStatsKey = "DiscPoolBoardWinsStats";
    public AudioSource achievementAS;



    // local variables
    private Coroutine unlockRoutine;

    private void OnEnable()
    {
        LoadUserData();
        AddLogin();

        achievementData.GetAllAchievementsEvent += GetAllAchievements;


        gameData.StartGameEvent += GameStarted;
        gameData.GameCompletedEvent += GameCompleted;


    }

    private void OnDisable()
    {
        achievementData.GetAllAchievementsEvent -= GetAllAchievements;

        gameData.StartGameEvent -= GameStarted;
        gameData.GameCompletedEvent -= GameCompleted;

    }

    public void GetAllAchievements()
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning("User is not authenticated with Google Play Games Services");
            return;
        }

        PlayGamesPlatform.Instance.LoadAchievements((achievements) =>
        {
            if (achievements == null)
            {
                Debug.LogError("Failed to load achievements from Google Play Games Services");
                return;
            }

            // Update achievement data with fetched achievements
            foreach (var achievement in achievements)
            {
                if(achievement.completed)
                {
                    achievementData.UnLockLocal(achievement.id);
                }
               
              //  Debug.Log($"Achievement: {achievement.id}, Completed: {achievement.completed}, percentCompleted: {achievement.percentCompleted}");
            }

        });
    }

    public void GameStarted()
    {
        Destination d = destinationData.currentDestination;

        if (d != null)
        {
            if (d.gameType == GameType.SinglePlayer)
            {
                achievementData.userData.spTotalGames++;
                SaveUserData();
            }
            else if (d.gameType == GameType.MultiPlayer)
            {
                achievementData.userData.mpTotalGames++;
                SaveUserData();
            }
        }
    }

    public void GameCompleted(int id)
    {
        Destination d = destinationData.currentDestination;
        Player mp = playerData.GetMainPlayer();
        if (d != null && mp.playerProperties.myId == id)
        {
            if (d.gameType == GameType.SinglePlayer)
            {
                if (d.aIDifficulty == AIDifficulty.Easy)
                {
                    if (d.gameMode == GameMode.Pool)
                    {
                        achievementData.userData.spPoolEasyWins++;
                        achievementData.userData.spTotalWins++;
                    }
                    else if (d.gameMode == GameMode.Snooker)
                    {
                        achievementData.userData.spSnookerEasyWins++;
                        achievementData.userData.spTotalWins++;
                    }
                }
                else if (d.aIDifficulty == AIDifficulty.Medium)
                {
                    if (d.gameMode == GameMode.Pool)
                    {
                        achievementData.userData.spPoolMediumWins++;
                        achievementData.userData.spTotalWins++;
                    }
                    else if (d.gameMode == GameMode.Snooker)
                    {
                        achievementData.userData.spSnookerMediumWins++;
                        achievementData.userData.spTotalWins++;
                    }
                }
                else if (d.aIDifficulty == AIDifficulty.Hard)
                {
                    if (d.gameMode == GameMode.Pool)
                    {
                        achievementData.userData.spPoolHardWins++;
                        achievementData.userData.spTotalWins++;
                    }
                    else if (d.gameMode == GameMode.Snooker)
                    {
                        achievementData.userData.spSnookerHardWins++;
                        achievementData.userData.spTotalWins++;
                    }
                }
            }
            else if (d.gameType == GameType.MultiPlayer)
            {
                if (d.gameMode == GameMode.Pool)
                {
                    achievementData.userData.mpPoolWins++;
                    achievementData.userData.mpTotalWins++;
                }
                else if (d.gameMode == GameMode.Snooker)
                {
                    achievementData.userData.mpSnookerWins++;
                    achievementData.userData.mpTotalWins++;
                }
            }

            SaveUserData();
        }
    }


    /// <summary>
    /// Instantly unlocks a standard, one-time achievement.
    /// </summary>
    /// <param name="achievementId">The exact alphanumeric string ID from the Google Play Console</param>
    public void UnlockSimpleAchievement(string achievementId)
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning($"[Achievements] User not authenticated. Cannot unlock ID: {achievementId}");
            return;
        }

        // Passing 100.0 instantly triggers a full unlock for standard achievements
        Social.ReportProgress(achievementId, 100.0, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"[Achievements] Successfully unlocked standard achievement: {achievementId}");

                // Mark it unlocked in your local ScriptableObject
                achievementData.UnLockLocal(achievementId);

                // Play your sound effect cleanly if a source is set
                if (achievementAS != null && !achievementAS.isPlaying)
                {
                    achievementAS.Play();
                }
            }
            else
            {
                Debug.LogError($"[Achievements] Failed to unlock standard achievement: {achievementId}");
            }
        });
    }

    /// <summary>
    /// Updates an incremental achievement directly to your current absolute count 
    /// and checks if it has been fully unlocked.
    /// </summary>
    /// <param name="achievementId">The exact alphanumeric string ID from the Google Play Console</param>
    /// <param name="currentCount">The current absolute total step count</param>
    /// <param name="targetCount">The unlock threshold for this achievement (e.g., 10 for 10 wins)</param>
    public void UpdateIncrementalAchievement(string achievementId, int currentCount, int targetCount)
    {
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.LogWarning($"[Achievements] User not authenticated. Cannot update incremental ID: {achievementId}");
            return;
        }

        Social.ReportProgress(achievementId, (double)currentCount, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"[Achievements] Successfully updated incremental achievement {achievementId} to step count: {currentCount}/{targetCount}");

                // Check if your local count has officially hit or crossed the required server target
                if (currentCount >= targetCount)
                {
                    Debug.LogWarning($"[Achievements] TARGET REACHED! Achievement {achievementId} is now FULLY UNLOCKED!");

                    // 1. Mark it unlocked in your local ScriptableObject state cache
                    achievementData.UnLockLocal(achievementId);

                    // 2. Play your reward sound effect cleanly
                    if (achievementAS != null && !achievementAS.isPlaying)
                    {
                        achievementAS.Play();
                    }

                    // 3. Optional: Fire a UI event here if you want to show a custom local toast/pop-up notification!
                }
            }
            else
            {
                Debug.LogError($"[Achievements] Failed to update incremental achievement steps for ID: {achievementId}");
            }
        });
    }

    public void SetTotalWins()
    {

        achievementData.userData.spTotalWins = achievementData.userData.spPoolEasyWins + achievementData.userData.spPoolMediumWins + achievementData.userData.spPoolHardWins
        + achievementData.userData.spSnookerEasyWins + achievementData.userData.spSnookerMediumWins + achievementData.userData.spSnookerHardWins;


        achievementData.userData.mpTotalWins = achievementData.userData.mpPoolWins + achievementData.userData.mpSnookerWins;


        foreach (BoardStats stats in achievementData.boardWinsStats.boardStats)
        {
            achievementData.userData.spTotalWins += (stats.spPoolWins + stats.spSnookerWins);
            achievementData.userData.mpTotalWins += (stats.mpPoolWins + stats.mpSnookerWins);
        }


    }

    public void AddLogin()
    {
        // If we have no record, count this as first login
        if (string.IsNullOrEmpty(achievementData.userData.lastLoginDate))
        {

            achievementData.userData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.userData.totalLogins += 1;

            return;
        }

        // Parse stored date and compare calendar date only
        DateTime.TryParse(achievementData.userData.lastLoginDate, out DateTime lastLogin);


        if (lastLogin.Date != DateTime.Now.Date)
        {
            achievementData.userData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.userData.totalLogins += 1;

        }


        achievementData.userData.spTotalWins = achievementData.userData.spPoolEasyWins + achievementData.userData.spPoolMediumWins + achievementData.userData.spPoolHardWins
            + achievementData.userData.spSnookerEasyWins + achievementData.userData.spSnookerMediumWins + achievementData.userData.spSnookerHardWins;

        achievementData.userData.mpTotalWins = achievementData.userData.mpPoolWins + achievementData.userData.mpSnookerWins;


        SaveUserData();
        // StartCoroutine(UnLockLoginAchievements());
    }

    public void AddClient(string clientId)
    {
        if (!achievementData.boardWinsStats.clientNames.Contains(clientId))
        {
            achievementData.boardWinsStats.clientNames.Add(clientId);
            SaveUserData();
            StartCoroutine(UnLockTableHostAchievements());
        }
    }

    public IEnumerator UnLockLoginAchievements()
    {

        yield return null;
        if (achievementData.userData.totalLogins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("login1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("login1").apiName);
            }
        }


        if (achievementData.userData.totalLogins >= 3)
        {
            if (!achievementData.IsAchievementUnlockedByName("login3"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("login3").apiName);
            }
        }


        if (achievementData.userData.totalLogins >= 5)
        {
            if (!achievementData.IsAchievementUnlockedByName("login5"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("login5").apiName);
            }
        }


        if (achievementData.userData.totalLogins <= 10)
        {
            
            if (!achievementData.IsAchievementUnlockedByName("login10"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("login10");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName,info.actual,info.target);
            }

        }

    }
    public IEnumerator UnLockTableHostAchievements()
    {
        yield return null;

        if (achievementData.boardWinsStats.clientNames.Count >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("invite1").apiName);
            }
        }


        if (achievementData.boardWinsStats.clientNames.Count >= 3)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite3"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("invite3").apiName);
            }
        }

        if (achievementData.boardWinsStats.clientNames.Count >= 5)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite5"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("invite5").apiName);
            }
        }


        if (achievementData.boardWinsStats.clientNames.Count <= 10)
        {
            if (!achievementData.IsAchievementUnlockedByName("invite10"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("invite10");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }

        }

    }
    public IEnumerator UnLockWinAchievements()
    {
        yield return null;

        if (achievementData.userData.spPoolEasyWins >= 1)
        { 
            if (!achievementData.IsAchievementUnlockedByName("spPoolEasyWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("spPoolEasyWins1").apiName);
            }
        }


        if (achievementData.userData.spPoolMediumWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spPoolMediumWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("spPoolMediumWins1").apiName);
            }
        }


        if (achievementData.userData.spPoolHardWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spPoolHardWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("spPoolHardWins1").apiName);
            }

        }

        if (achievementData.userData.spPoolHardWins <= 10)
        {
            if (!achievementData.IsAchievementUnlockedByName("spPoolHardWins10"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spPoolHardWins10");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }

        }



        if (achievementData.userData.spSnookerEasyWins >= 1)
        {
         
            if (!achievementData.IsAchievementUnlockedByName("spSnookerEasyWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("spSnookerEasyWins1").apiName);
            }
        }


        if (achievementData.userData.spSnookerMediumWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spSnookerMediumWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("spSnookerMediumWins1").apiName);
            }
        }


        if (achievementData.userData.spSnookerHardWins >= 1)
        {

            if (!achievementData.IsAchievementUnlockedByName("spSnookerHardWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("spSnookerHardWins1").apiName);
            }

        }

        if (achievementData.userData.spSnookerHardWins <= 10)
        {

            if (!achievementData.IsAchievementUnlockedByName("spSnookerHardWins10"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spSnookerHardWins10");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }
        }



        if (achievementData.userData.mpPoolWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("mpPoolWins1").apiName);
            }
        }
        if (achievementData.userData.mpPoolWins >= 3)
        {
            
            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins3"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("mpPoolWins3").apiName);
            }
        }
        if (achievementData.userData.mpPoolWins >= 5)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins5"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("mpPoolWins5").apiName);
            }
        }

        if (achievementData.userData.mpPoolWins <= 10)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpPoolWins10"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpPoolWins10");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }
        }


        if (achievementData.userData.mpSnookerWins >= 1)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins1"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("mpSnookerWins1").apiName);
            }
        }
        if (achievementData.userData.mpSnookerWins >= 3)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins3"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("mpSnookerWins3").apiName);
            }
        }
        if (achievementData.userData.mpSnookerWins >= 5)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins5"))
            {
                UnlockSimpleAchievement(achievementData.GetAchievementByName("mpSnookerWins3").apiName);
            }
        }

        if (achievementData.userData.mpSnookerWins <= 10)
        {

            if (!achievementData.IsAchievementUnlockedByName("mpSnookerWins10"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpSnookerWins10");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }
        }


    }
    public IEnumerator UnLockOverallAchievements()
    {
        yield return null;

        if (achievementData.userData.spTotalWins <= 50)
        {

            if (!achievementData.IsAchievementUnlockedByName("spTotalWins50"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("spTotalWins50");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }
        }


        if (achievementData.userData.mpTotalWins <= 50)
        {
            if (!achievementData.IsAchievementUnlockedByName("mpTotalWins50"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("mpTotalWins50");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }
        }


        int totalWins = achievementData.userData.spTotalWins + achievementData.userData.mpTotalWins;
        if (totalWins <= 100)
        {

            if (!achievementData.IsAchievementUnlockedByName("totalWins100"))
            {
                AchievementInfo info = achievementData.GetAchievementByName("totalWins100");
                info.actual = achievementData.userData.totalLogins;
                UpdateIncrementalAchievement(info.apiName, info.actual, info.target);
            }
        }

    }

    public IEnumerator UnLockBoardAchievements()
    {
        yield return null;

        AchievementInfo achievementInfo = null;

        foreach (BoardStats stats in achievementData.boardWinsStats.boardStats)
        {
            achievementInfo = achievementData.GetAchievementByName("spPool" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.spPoolWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        UnlockSimpleAchievement(achievementInfo.apiName);
                    }
                }

            }

            achievementInfo = achievementData.GetAchievementByName("spSnooker" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.spSnookerWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        UnlockSimpleAchievement(achievementInfo.apiName);
                    }
                }

            }

            achievementInfo = achievementData.GetAchievementByName("mpPool" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.mpPoolWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        UnlockSimpleAchievement(achievementInfo.apiName);
                    }
                }

            }

            achievementInfo = achievementData.GetAchievementByName("mpSnooker" + Enum.GetName(typeof(BoardType), stats.boardType));

            if (achievementInfo != null)
            {
                if (stats.mpSnookerWins >= 1)
                {
                    if (!achievementData.IsAchievementUnlockedByName(achievementInfo.name))
                    {
                        UnlockSimpleAchievement(achievementInfo.apiName);
                    }
                }

            }
        }

    }
    public void LoadUserData()
    {
        try
        {
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, userDataKey + ".txt");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                achievementData.userData = JsonUtility.FromJson<UserData>(json);
            }

        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load user data from file: {e.Message}");

        }

        try
        {
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, boardWinsStatsKey + ".txt");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                achievementData.boardWinsStats = JsonUtility.FromJson<BoardWinsStats>(json);
            }

        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load board wins stats from file: {e.Message}");

        }
    }

    public void SaveUserData()
    {
        try
        {
            string json = JsonUtility.ToJson(achievementData.userData);
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, userDataKey + ".txt");
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save user data to file: {e.Message}");
        }

        try
        {
            string json = JsonUtility.ToJson(achievementData.boardWinsStats);
            string path = Path.Combine(UnityEngine.Application.persistentDataPath, boardWinsStatsKey + ".txt");
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save board wins stats to file: {e.Message}");
        }
    }

}

