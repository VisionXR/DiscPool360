using com.VisionXR.ModelClasses;

using System.Collections;
using UnityEngine;



namespace com.VisionXR.Controllers
{

    public class OculusDataManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public UIDataSO uiData;
        public DestinationSO destinationData;
        public PurchaseDataSO purchaseData;
        public AchievementsDataSO achievementsData;
        public LeaderBoardSO leaderBoardData;


        [Header("Game Objects")]
        
        public UIManager uiManager;
        public DestinationPanel destinationPanel;
        public ChangeDestinationPanel changeDestinationPanel;
        public GameObject localAvatar;
      

        // local variables
        public Destination tutorialDestination;
        public string Key = "DiscPoolTutorial";

        private bool isEntitlementVerified = false;

        public Destination currentDestination;


            // Start is called once before the first execution of Update after the MonoBehaviour is created
        public  IEnumerator Start()
        {
            destinationData.currentDestination = null;

            if (!UnityEngine.Application.isEditor)
            {
                userData.SetUserNameAndId("User " + UnityEngine.Random.Range(0, 9999), (ulong)UnityEngine.Random.Range(0, 9999));
                //    localAvatar.SetActive(true);

                // Simulate connection delay
                yield return new WaitForSeconds(1);

                uiData.TriggerHomeEvent();

                //if (PlayerPrefs.HasKey(Key))
                //{
                    
                //}
                //else
                //{
                //    currentDestination = tutorialDestination;
                //    PlayerPrefs.SetString(Key, "true");
                //    destinationPanel.gameObject.SetActive(true);
                //    destinationPanel.ConnectToDestination(currentDestination);
                //}
            }
            else
            {

                // Set player settings
                userData.SetUserNameAndId("User " + UnityEngine.Random.Range(0, 9999), (ulong)UnityEngine.Random.Range(0, 9999));
            //    localAvatar.SetActive(true);

                // Simulate connection delay
                yield return new WaitForSeconds(1);

                if (PlayerPrefs.HasKey(Key))
                {
                    uiData.TriggerHomeEvent();
                }
                else
                {
                    currentDestination = tutorialDestination;
                    PlayerPrefs.SetString(Key, "true");
                    destinationPanel.gameObject.SetActive(true);
                    destinationPanel.ConnectToDestination(currentDestination);
                }

            }
        }

    



        private IEnumerator StartOVRPlatform()
        {

            yield return new WaitForSeconds(1);
            
            
        }


   


    }

}