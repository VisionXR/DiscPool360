using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIDataSO uiData;
        public GameDataSO gameData;

        [Header("Managers")]
        public GameObject SinglePlayerManager;
        public GameObject MultiPlayerManager;
        public GameObject TutorialManager;



        private void OnEnable()
        {
           
                   
        }

        private void OnDisable()
        {
                    
           
        }


        public void StartGame(int id)
        {
            ResetManagers();
        }

        private void ResetManagers()
        {
            SinglePlayerManager.SetActive(false);
            MultiPlayerManager.SetActive(false);
            TutorialManager.SetActive(false);

        }
    }
}
