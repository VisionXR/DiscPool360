//using com.VisionXR.GameElements;
//using com.VisionXR.HelperClasses;
//using com.VisionXR.ModelClasses;
//using Oculus.Platform;
//using Oculus.Platform.Models;
//using System;
//using System.Collections;
//using System.IO;
//using UnityEngine;


//public class AchievementManager : MonoBehaviour
//{

//    [Header("Scriptable Objects")]
//    public AchievementsDataSO achievementData;
//    public PlayerDataSO playerData;
//    public UIDataSO uiData;
//    public GameDataSO gameData;
//    public DestinationSO destinationData;
//    public string userDataKey = "DiscPoolUserData";
//    public AudioSource achievementAS;

//    [Header(" Achievement Api Names")]
//    public string Login_Milestone1;
//    public string Login_Milestone2;
//    public string Login_Milestone3;
//    public string Single_Pool_AIEasy_First_Win;
//    public string Single_Pool_AIMedium_First_Win;
//    public string Single_Pool_AIHard_First_Win;
//    public string Single_Pool_AIHard_Ten_Win;
//    public string Single_Snooker_AIEasy_First_Win;
//    public string Single_Snooker_AIMedium_First_Win;
//    public string Single_Snooker_AIHard_First_Win;
//    public string Single_Snooker_AIHard_Ten_Win;
//    public string Multi_Pool_Win1;
//    public string Multi_Pool_Win3;
//    public string Multi_Pool_Win5;
//    public string Multi_Pool_Win10;
//    public string Multi_Snooker_Win1;
//    public string Multi_Snooker_Win3;
//    public string Multi_Snooker_Win5;
//    public string Multi_Snooker_Win10;


//    // local variables
//    private Coroutine unlockRoutine;

//    private void OnEnable()
//    {
//        LoadUserData();
//        AddLogin();

//        achievementData.GetAllAchievementsEvent += GetAllAchievements;
      

//        gameData.StartGameEvent += GameStarted;
//        gameData.GameCompletedEvent += GameCompleted;
      
        
//    }

//    private void OnDisable()
//    {
//        achievementData.GetAllAchievementsEvent -= GetAllAchievements;
       

//        gameData.StartGameEvent -= GameStarted;
//        gameData.GameCompletedEvent -= GameCompleted;
      

//    }


//    private IEnumerator UnlockAndWait(string apiName)
//    {
//        bool completed = false;

//        // Capture the request to keep it in scope
//        var request = Achievements.Unlock(apiName);

//        request.OnComplete((Message<AchievementUpdate> msg) =>
//        {
//            // Now we are inside the handler; it won't be "missing"
//            if (msg.IsError)
//            {
//                Debug.LogError($"Failed: {msg.GetError().Message}");
//                return;
//            }

//            achievementAS.Play();
//            achievementData.UnLockLocal(apiName);
//            completed = true;
//        });

//        // Wait for the server to actually respond before the coroutine moves to the next 'if'
//        yield return new WaitUntil(() => completed);
//        yield return new WaitForSeconds(0.1f); // Tiny breather for the SDK
//    }

//    public void GetAllAchievements()
//    {
   
//        Achievements.GetAllProgress().OnComplete((Message<AchievementProgressList> msg) =>
//        {
//            if (msg.IsError)
//            {
//                Debug.Log("Failed to fetch achievements: " + msg.GetError().Message);
//                return;
//            }

//            foreach (var progress in msg.Data)
//            {
//                if (progress.IsUnlocked)
//                {
//                    achievementData.UnLockLocal(progress.Name);
//                }

//            }

//            if(unlockRoutine == null)
//            {
//                unlockRoutine = StartCoroutine(CheckUnLockStatus());
//            }

//            achievementData.GotAll();
            
//        });
       
//    }

//    private IEnumerator CheckUnLockStatus()
//    {
//        yield return StartCoroutine(UnLockLoginAchievements());
//        yield return StartCoroutine(UnLockWinAchievements());
//        unlockRoutine = null;
//    }

//    public void GameStarted()
//    {
//        Destination d = destinationData.currentDestination;

//        if (d != null)
//        {
//            if(d.gameType == GameType.SinglePlayer)
//            {
//                achievementData.userData.spTotalGames++;
//                SaveUserData();
//            }
//            else if (d.gameType == GameType.MultiPlayer)
//            {
//                achievementData.userData.mpTotalGames++;
//                SaveUserData();
//            }
//        }
//    }

//    public void GameCompleted(int id)
//    {
//        Destination d = destinationData.currentDestination;
//        Player mp = playerData.GetMainPlayer();
//        if (d != null && mp.playerProperties.myId == id)
//        {
//            if (d.gameType == GameType.SinglePlayer)
//            {
//                if(d.aIDifficulty == AIDifficulty.Easy)
//                {
//                    if(d.gameMode == GameMode.Pool)
//                    {
//                        achievementData.userData.spPoolEasyWins++;
//                        achievementData.userData.spTotalWins++;
//                    }
//                    else if (d.gameMode == GameMode.Snooker)
//                    {
//                        achievementData.userData.spSnookerEasyWins++;
//                        achievementData.userData.spTotalWins++;
//                    }
//                }
//                else if (d.aIDifficulty == AIDifficulty.Medium)
//                {
//                    if (d.gameMode == GameMode.Pool)
//                    {
//                        achievementData.userData.spPoolMediumWins++;
//                        achievementData.userData.spTotalWins++;
//                    }
//                    else if (d.gameMode == GameMode.Snooker)
//                    {
//                        achievementData.userData.spSnookerMediumWins++;
//                        achievementData.userData.spTotalWins++;
//                    }
//                }
//                else if (d.aIDifficulty == AIDifficulty.Hard)
//                {
//                    if (d.gameMode == GameMode.Pool)
//                    {
//                        achievementData.userData.spPoolHardWins++;
//                        achievementData.userData.spTotalWins++;
//                    }
//                    else if (d.gameMode == GameMode.Snooker)
//                    {
//                        achievementData.userData.spSnookerHardWins++;
//                        achievementData.userData.spTotalWins++;
//                    }
//                }
//            }
//            else if (d.gameType == GameType.MultiPlayer)
//            {
//                if (d.gameMode == GameMode.Pool)
//                {
//                    achievementData.userData.mpPoolWins++;
//                    achievementData.userData.mpTotalWins++;
//                }
//                else if (d.gameMode == GameMode.Snooker)
//                {
//                    achievementData.userData.mpSnookerWins++;
//                    achievementData.userData.mpTotalWins++;
//                }
//            }

//            SaveUserData();

//            StartCoroutine(UnLockWinAchievements());
//        }
//    }

//    public void AddLogin()
//    {
//        // If we have no record, count this as first login
//        if (string.IsNullOrEmpty(achievementData.userData.lastLoginDate))
//        {
            
//            achievementData.userData.lastLoginDate = DateTime.Now.ToLongDateString();
//            achievementData.userData.totalLogins += 1;
//            SaveUserData();
//            return;
//        }

//        // Parse stored date and compare calendar date only
//        DateTime.TryParse(achievementData.userData.lastLoginDate, out DateTime lastLogin);
      

//        if (lastLogin.Date != DateTime.Now.Date)
//        {
//            achievementData.userData.lastLoginDate = DateTime.Now.ToLongDateString();
//            achievementData.userData.totalLogins += 1;
//            SaveUserData();
//        }

        
//    }

//    public IEnumerator UnLockLoginAchievements()
//    {
//        if(achievementData.userData.totalLogins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Login_Milestone1))
//            {
//                yield return StartCoroutine(UnlockAndWait(Login_Milestone1));
//            }
//        }
    

//        if (achievementData.userData.totalLogins >= 3)
//        {
//            if (!achievementData.IsAchievementUnlocked(Login_Milestone2))
//            {
//                yield return StartCoroutine(UnlockAndWait(Login_Milestone2));
//            }
//        }
      

//        if (achievementData.userData.totalLogins >= 5)
//        {
//            if (!achievementData.IsAchievementUnlocked(Login_Milestone3))
//            {
//                yield return StartCoroutine(UnlockAndWait(Login_Milestone3));
//            }
//        }
    
//    }
//    public IEnumerator UnLockWinAchievements()
//    {
//        if (achievementData.userData.spPoolEasyWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Pool_AIEasy_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Pool_AIEasy_First_Win));
              
//            }
//        }
       

//        if (achievementData.userData.spPoolMediumWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Pool_AIMedium_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Pool_AIMedium_First_Win));
                
//            }
//        }
        

//        if (achievementData.userData.spPoolHardWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Pool_AIEasy_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Pool_AIEasy_First_Win));
              
//            }
//        }

//        if (achievementData.userData.spPoolHardWins >= 10)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Pool_AIHard_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Pool_AIHard_First_Win));
              
//            }
//        }


//        if (achievementData.userData.spSnookerEasyWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Snooker_AIEasy_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Snooker_AIEasy_First_Win));
              
//            }
//        }
      

//        if (achievementData.userData.spSnookerMediumWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Snooker_AIMedium_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Snooker_AIMedium_First_Win));
                
//            }
//        }
        

//        if (achievementData.userData.spSnookerHardWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Snooker_AIHard_First_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Snooker_AIHard_First_Win));
               
//            }
//        }

//        if (achievementData.userData.spSnookerHardWins >= 10)
//        {
//            if (!achievementData.IsAchievementUnlocked(Single_Snooker_AIHard_Ten_Win))
//            {
//                yield return StartCoroutine(UnlockAndWait(Single_Snooker_AIHard_Ten_Win));
           
//            }
//        }

//        if (achievementData.userData.mpPoolWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Pool_Win1))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Pool_Win1));
           
//            }
//        }
//        if (achievementData.userData.mpPoolWins >= 3)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Pool_Win3))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Pool_Win3));
             
//            }
//        }
//        if (achievementData.userData.mpPoolWins >= 5)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Pool_Win5))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Pool_Win5));
//            }
//        }

//        if (achievementData.userData.mpPoolWins >= 10)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Pool_Win10))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Pool_Win10));
//            }
//        }

//        if (achievementData.userData.mpSnookerWins >= 1)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Snooker_Win1))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Snooker_Win1));
//            }
//        }
//        if (achievementData.userData.mpSnookerWins >= 3)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Snooker_Win3))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Snooker_Win3));
//            }
//        }
//        if (achievementData.userData.mpSnookerWins >= 5)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Snooker_Win5))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Snooker_Win5));
//            }
//        }

//        if (achievementData.userData.mpSnookerWins >= 10)
//        {
//            if (!achievementData.IsAchievementUnlocked(Multi_Snooker_Win10))
//            {
//                yield return StartCoroutine(UnlockAndWait(Multi_Snooker_Win10));
//            }
//        }



//    }
//    public void LoadUserData()
//    {
//        try
//        {
//            string path = Path.Combine(UnityEngine.Application.persistentDataPath, userDataKey + ".txt");
//            if (File.Exists(path))
//            {
//                string json = File.ReadAllText(path);
//                achievementData.userData = JsonUtility.FromJson<UserData>(json);
//            }

//        }
//        catch (Exception e)
//        {
//            Debug.LogError($"Failed to load user data from file: {e.Message}");
 
//        }
//    }
    
//    public void SaveUserData()
//    {
//        try
//        {
//            string json = JsonUtility.ToJson(achievementData.userData);
//            string path = Path.Combine(UnityEngine.Application.persistentDataPath, userDataKey + ".txt");
//            File.WriteAllText(path, json);
//        }
//        catch (Exception e)
//        {
//            Debug.LogError($"Failed to save user data to file: {e.Message}");
//        }
//    }

//}

