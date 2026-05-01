using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIDataSO uiData;
        public Animator uiController;

        [Header("Canvas Objects")]
        public List<GameObject> allCanvases;
        public List<GameObject> allMainPanels;

       
       


        private void Start()
        {
            ResetCanvases();
            ResetMainPanels();

            uiData.uiManager = this;
            uiController.enabled = true;

        }


        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
           
        }


        public void ShowCanvas(int id)
        {
            allCanvases[id].SetActive(true);
        }

        public void ShowMainPanel(int id)
        {
            Debug.Log("Showing main panel with id: " + id);
            allMainPanels[id].GetComponent<PanelOnOff>().TurnOnPanel();
        }

        public void HideCanvas(int id)
        {
            allCanvases[id].SetActive(false);
        }

        public void HideMainPanel(int id)
        {
            allMainPanels[id].GetComponent<PanelOnOff>().TurnOffPanel();
        }

        private void ResetMainPanels()
        {
            foreach (GameObject panel in allMainPanels)
            {
                panel.SetActive(false);
            }
        }

        private void ResetCanvases()
        {     
            foreach(GameObject canvas in allCanvases)
            {
                canvas.SetActive(false);
            }
        }

    }
}
