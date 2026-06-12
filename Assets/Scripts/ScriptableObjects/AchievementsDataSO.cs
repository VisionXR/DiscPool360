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

        private void OnEnable()
        {
            specialBoardWinsStats = new SpecialBoardWinsStats();
            defaultBoardWinsData = new DefaultBoardWinsData();
            SetSpecialBoardsData();
        }

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

        public void SetSpecialBoardsData()
        {
            if(specialBoardWinsStats.boardStats.Count == 0)
            {
                BoardStats squareStats = new BoardStats();
                squareStats.boardType = BoardType.Square4;

                BoardStats trainglestats = new BoardStats();
                trainglestats.boardType = BoardType.Triangle3;

                BoardStats hex6Stats = new BoardStats();
                hex6Stats.boardType = BoardType.Hexagon6;

                BoardStats hex4Stats = new BoardStats();
                hex4Stats.boardType = BoardType.Hexagon4;

                BoardStats oct4Stats = new BoardStats();
                oct4Stats.boardType = BoardType.Octagon4;

                BoardStats circle4stats = new BoardStats();
                circle4stats.boardType = BoardType.Circle4;

                specialBoardWinsStats.boardStats.Add(squareStats);
                specialBoardWinsStats.boardStats.Add(trainglestats);
                specialBoardWinsStats.boardStats.Add(hex4Stats);
                specialBoardWinsStats.boardStats.Add(hex6Stats);
                specialBoardWinsStats.boardStats.Add(oct4Stats);
                specialBoardWinsStats.boardStats.Add(circle4stats);
            }
        }

        public void Clear()
        {
            defaultBoardWinsData = new DefaultBoardWinsData();
            specialBoardWinsStats = new SpecialBoardWinsStats();
            SetSpecialBoardsData();
        }
    }


}
