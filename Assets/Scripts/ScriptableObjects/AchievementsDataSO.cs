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
        public UserData userData;


        // Actions
        public Action GetAllAchievementsEvent;
        public Action SinglePlayerGameWonEvent;
        public Action MultiPlayerGameWonEvent;
        public Action MultiPlayerGameStartEvent;

        public Action GotAllAchievementsEvent;

        public void GetAllAchievemnets()
        {
            GetAllAchievementsEvent?.Invoke();
        }

        public void GotAll()
        {
            GetAllAchievementsEvent?.Invoke(); 
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

        public bool IsAchievementUnlocked(string apiName)
        {
            foreach (AchievementInfo info in AllAchievementInfo)
            {
                if (info.apiName == apiName)
                {
                    return info.isAchieved;
                }
            }
            return false;
        }
    }


}
