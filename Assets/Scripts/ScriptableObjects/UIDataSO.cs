using System;
using UnityEngine;
using com.VisionXR.HelperClasses;
using System.Collections.Generic;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIDataSO", menuName = "ScriptableObjects/UIDataSO")]
    public class UIDataSO : ScriptableObject
    {
        // variables
        public GameType currentGameType;
        public GameMode currentGameMode;
        public AIDifficulty currentAIDifficulty;

        // Actions

        public Action HomeEvent;
        public Action ShowFoulEvent;
        public Action<string> ShowFoulHandlingEvent;

        public Action SetCoinsEvent;
        public Action UpdateCoinsEvent;

        public Action ExitBtnClickedEvent;
        public Action ResetAllPanelsEvent;

        // Mic and speaker Actions
        public Action TurnOnMicEvent;
        public Action TurnOffMicEvent;
        
        public Action TurnOnSpeakerEvent;
        public Action TurnOffSpeakerEvent;

        //Methods

        public void UpdateCoins()
        {
            UpdateCoinsEvent?.Invoke();
        }

        public void SetCoins()
        {
            SetCoinsEvent?.Invoke();
        }

        public void ShowFoulHandling(string displayText)
        {
            ShowFoulHandlingEvent?.Invoke(displayText);
        }

        public void ShowFoul()
        {
            ShowFoulEvent?.Invoke();
        }

        public void TriggerHomeEvent()
        {
            HomeEvent?.Invoke();
        }

        public void SetGameType(GameType gameType)
        {
            currentGameType = gameType;
        }

        public void SetGameMode(GameMode gameMode)
        {
            currentGameMode = gameMode;
        }

        public void SetAIDifficulty(AIDifficulty aiDifficulty)
        {
            currentAIDifficulty = aiDifficulty;
        }

        public void ExitButtonClicked()
        {
            ExitBtnClickedEvent?.Invoke();
        }

        public void ResetAllPanels()
        {
            ResetAllPanelsEvent?.Invoke();
        }
    }
}
