using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "SaveDataSO", menuName = "ScriptableObjects/SaveDataSO", order = 1)]
    public class SaveDataSO : ScriptableObject
    {
        // User Data


        // Actions

        public Action<PlayerSettings> SaveSettingsEvent;
        public Action LoadSettingsEvent;

        // Methods

      
        public void SaveSettings(PlayerSettings playerSettings)
        {
            SaveSettingsEvent?.Invoke(playerSettings);
        }

        public void LoadSettings()
        {
            LoadSettingsEvent?.Invoke(); 
        }

    }
}
