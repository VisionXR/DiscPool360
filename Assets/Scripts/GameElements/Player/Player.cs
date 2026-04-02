using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class Player : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PlayerDataSO playerData;
      

        [Header("Local Objects")]
        public PlayerProperties playerProperties;
        public IAIBehaviour aIBehaviour;

        [Header("Prefabs")]
        [SerializeField] private GameObject EasyBot;
        [SerializeField] private GameObject MediumBot;
        [SerializeField] private GameObject HardBot;

        private void OnEnable()
        {
            playerData.AddPlayer(this);
        }

        private void OnDisable()
        {
            playerData.RemovePlayer(this);
        }

        public void SetProperties(PlayerProperties properties)
        {
            playerProperties = properties;
        }

        public void InitializePlayer(PlayerProperties properties)
        {
            playerProperties = properties;

            if (playerProperties.myPlayerType == PlayerType.AI)
            {
                switch (playerProperties.myAiDifficulty)
                {
                    case AIDifficulty.Easy:
                        GameObject easyBot = Instantiate(EasyBot, transform);
                        aIBehaviour = easyBot.GetComponent<IAIBehaviour>();
                        break;
                    case AIDifficulty.Medium:
                        GameObject mediumBot = Instantiate(MediumBot, transform);
                        aIBehaviour = mediumBot.GetComponent<IAIBehaviour>();
                        break;
                    case AIDifficulty.Hard:
                        GameObject hardBot = Instantiate(HardBot, transform);
                        aIBehaviour = hardBot.GetComponent<IAIBehaviour>();
                        break;
                    default:
                        GameObject defaultBot = Instantiate(EasyBot, transform);
                        aIBehaviour = defaultBot.GetComponent<IAIBehaviour>();
                        break;
                }
            }

        }

    }
}
