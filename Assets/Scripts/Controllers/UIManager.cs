using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public GameDataSO gameData;
        public UIDataSO uiData;
        public TableDataSO tableData;

        [Header("Game Objects")]
        public GameObject mainCanvas;


        [Header("All Panels")]
        public List<GameObject> allPanels;
        public GameObject winningPanel;
        public GameObject exitPanel;
        public GameObject homePanel;
        public GameObject tutorialPanel;
        public GameObject destinationPanel;


        private void OnEnable()
        {
            gameData.GameCompletedEvent += ShowGameCompletedPanel;

            uiData.HomeEvent += ShowHomePanel;
 
            uiData.ExitBtnClickedEvent += ShowExitPanel;
            uiData.ResetAllPanelsEvent += ResetPanels;
        }

        private void OnDisable()
        {
            gameData.GameCompletedEvent -= ShowGameCompletedPanel;

            uiData.HomeEvent -= ShowHomePanel;


            uiData.ExitBtnClickedEvent -= ShowExitPanel;      
            uiData.ResetAllPanelsEvent -= ResetPanels;
        }


        private void ShowHomePanel()
        {
            ResetPanels();
            homePanel.SetActive(true);
        }

        public void ShowTutorialPanel()
        {
            ResetPanels();
            tutorialPanel.SetActive(true);
        }

        private void ShowExitPanel()
        {
            ResetPanels();
            exitPanel.SetActive(true);
        }

        public void ShowGameCompletedPanel(int winnerId)
        {
            // Show the game completed panel
            ResetPanels();

            // Activate the winning panel
            winningPanel.SetActive(true);
            WinningPanelView winningPanelView = winningPanel.GetComponent<WinningPanelView>();
            winningPanelView.ShowWinner(winnerId);
        }


        public void ResetPanels()
        {
            foreach (GameObject panel in allPanels)
            {
                panel.SetActive(false);
            }
        }

    }
}
