using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.IO;
using UnityEngine;


public class AchievementManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;
    public GameDataSO gameData;
    public DestinationSO destinationData;
    public CloudDataSO cloudData;
    public string userDataKey = "DiscPoolUserData";
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

    public void AddLogin()
    {
        // If we have no record, count this as first login
        if (string.IsNullOrEmpty(achievementData.userData.lastLoginDate))
        {

            achievementData.userData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.userData.totalLogins += 1;
            SaveUserData();
            return;
        }

        // Parse stored date and compare calendar date only
        DateTime.TryParse(achievementData.userData.lastLoginDate, out DateTime lastLogin);


        if (lastLogin.Date != DateTime.Now.Date)
        {
            achievementData.userData.lastLoginDate = DateTime.Now.ToLongDateString();
            achievementData.userData.totalLogins += 1;
            SaveUserData();
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

        cloudData.SavePlayerData();
    }

}

