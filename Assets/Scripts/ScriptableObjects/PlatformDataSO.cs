using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "PlatformDataSO", menuName = "ScriptableObjects/PlatformDataSO")]
    public class PlatformDataSO : ScriptableObject
    {
        // variables


        // Actions
        public Action TablePlacedEvent;


        //Methods

        public void TablePlaced()
        {
            TablePlacedEvent?.Invoke();
        }

    }
}
