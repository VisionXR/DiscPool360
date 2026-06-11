using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AchievementsDataSO", menuName = "ScriptableObjects/AchievementsDataSO", order = 1)]
    public class AchievementsDataSO : ScriptableObject
    {
        // Variables
        public List<AchievementInfo> AllAchievementInfo;
        public DefaultBoardWinsData defaultBoardWinsData;
        public SpecialBoardWinsStats specialBoardWinsStats;


        // Actions
        public Action GetAllAchievementsEvent;
        public Action SinglePlayerGameWonEvent;
        public Action MultiPlayerGameWonEvent;
        public Action MultiPlayerGameStartEvent;
        public Action UserLoggedInEvent;
        public Action GotAllAchievementsEvent;

        public void GetAllAchievemnets()
        {
            GetAllAchievementsEvent?.Invoke();
        }

        public void UserLoggedIn()
        {
            UserLoggedInEvent?.Invoke();
        }


        public void UnLockLocal(string apiName)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.apiName == apiName)
                {
                    info.isAchieved = true;
                }
            }
        }

        public void UpdateLocalProgress(string apiName,int actualCount)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.apiName == apiName)
                {
                    info.actual = actualCount;
                }
            }
        }


        public AchievementInfo GetAchievementByName(string name)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.name == name)
                {
                    return info;
                }
            }

            return null;
        }

        public AchievementInfo GetAchievementByApiId(string apiId)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.apiName == apiId)
                {
                    return info;
                }
            }

            return null;
        }

        public bool IsAchievementUnlockedByName(string Name)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.name == Name)
                {
                    return info.isAchieved;
                }
            }
            return false;
        }

        public void Clear()
        {
            defaultBoardWinsData = new DefaultBoardWinsData();
            foreach(BoardStats stats in specialBoardWinsStats.boardStats)
            {
                stats.spSnookerWins = 0;
                stats.spPoolWins = 0;
                stats.mpPoolWins = 0;
                stats.mpSnookerWins = 0;
            }
            specialBoardWinsStats.clientNames.Clear();
        }
    }


}
