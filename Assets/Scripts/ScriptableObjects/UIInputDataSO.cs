using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "UIInputDataSO", menuName = "ScriptableObjects/UIInputDataSO")]
    public class UIInputDataSO : ScriptableObject
    {
        // variables


  
        // Actions

        public Action HomeEvent;
        public Action<int> ShowTurnEvent;
        public Action ShowFoulEvent;
        public Action ShowFoulHandlingEvent;

       

   

        //Methods




    }
}
