using com.VisionXR.ModelClasses;
using UnityEngine;
using com.VisionXR.HelperClasses;
using Fusion;
using System;

namespace com.VisionXR.Controllers
{
    public class AIManager : MonoBehaviour
    {


        [Header("Scriptable Objects")]
        public AIDataSO aIData;
      

        [Header("Prefabs")]
        [SerializeField] private GameObject EasyBot;
        [SerializeField] private GameObject MediumBot;
        [SerializeField] private GameObject HardBot;



 
        public void CreateBot(AIDifficulty type,Action<GameObject> AICreatedEvent)
        {

            GameObject currentBot = null;
            if (type == AIDifficulty.Easy)
            {
                currentBot = Instantiate(EasyBot, transform.position, transform.rotation);
               

            }
            else if (type == AIDifficulty.Medium)
            {
                currentBot = Instantiate(MediumBot, transform.position, transform.rotation);

            }
            else // create hard bot
            {
                currentBot = Instantiate(HardBot, transform.position, transform.rotation);

            }

        
            AICreatedEvent?.Invoke(currentBot);
        }


    }
}

