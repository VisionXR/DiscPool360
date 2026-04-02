using System;
using UnityEngine;
using com.VisionXR.HelperClasses;
using System.Collections.Generic;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AppPropertiesDataSO", menuName = "ScriptableObjects/AppPropertiesDataSO")]
    public class AppPropertiesDataSO : ScriptableObject
    {
        // variables
        [Header(" Colors ")]
        public Color HoverColor;
        public Color IdleColor;
  
        [Header(" Local variables")]
        public float vibrationDuration = 0.2f;
        public float vibrationAmplitude = 0.2f;
        public float vibrationFrequency = 0.2f;
        public float strikingVibrationFrequency = 0.5f;
        public float strikingVibrationDuration = 0.5f;


        // Actions

        public Action StartVibrationEvent;
        public Action StartStrikingVibrationEvent;

        //Methods

        public void StartVibration()
        {
            StartVibrationEvent?.Invoke();
        }

        public void StartStrikingVibration()
        {
            StartStrikingVibrationEvent?.Invoke();
        }

    }
}
