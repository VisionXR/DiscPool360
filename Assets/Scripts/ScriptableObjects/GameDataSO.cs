using System;
using UnityEngine;


namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO")]
    public class GameDataSO : ScriptableObject
    {
        // variables
        public int currentTurnId = 1;
        public int firstTurnId = 1;
        public int Player1SnookerScore;
        public int Player2SnookerScore;
        public int snookerWinnerId = 1;

        // Game Events
        public Action StartGameEvent;
        public Action ExitGameEvent;
        public Action<int> GameCompletedEvent;
        public Action<int> PlayAgainEvent;

        public Action<int> TurnChangeEvent;
      
        

        //Methods

        public void ResetSnookerScore()
        {
            snookerWinnerId = 1;
            Player1SnookerScore = 0;
            Player2SnookerScore = 0;
        }

        public void SetSnookerWinner(int id)
        {
            snookerWinnerId = id;
        }
        public int GetSnookerScore(int id)
         {
            if (id == 1)
            {
                return Player1SnookerScore;
            }
            else
            {
                return Player2SnookerScore;
            }
         
        }
        public void AddSnookerScore(int playerId, int score)
        {
            if (playerId == 1)
            {
                Player1SnookerScore += score;
            }
            else if (playerId == 2)
            {
                Player2SnookerScore += score;
            }
        }


        public void SetSnookerScore(int p1Score,int p2Score)
        {
            Player1SnookerScore = p1Score;
            Player2SnookerScore = p2Score;
        }


        public void StartGame()
        {
            firstTurnId = 1;
            StartGameEvent?.Invoke();
        }

        public void GameCompleted(int winnerId)
        {
            GameCompletedEvent?.Invoke(winnerId);
        }

        public void ExitGame()
        {
            ExitGameEvent?.Invoke();
        }

        public void PlayAgain()
        {
            if (firstTurnId == 1)
            {
                firstTurnId = 2;
            }
            else
            {
                firstTurnId = 1;
            }

            PlayAgainEvent?.Invoke(firstTurnId);
        }

        public void ChangeTurn(int id)
        {
            currentTurnId = id;
            TurnChangeEvent?.Invoke(currentTurnId);
        }

    }
}
