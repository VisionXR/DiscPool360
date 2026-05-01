using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class MainCanvasView : MonoBehaviour
    {

        public List<GameObject> allMainPanels;

        public void ShowMainPanel(int id)
        {
            Debug.Log("Showing main panel with id: " + id);
            allMainPanels[id].GetComponent<PanelOnOff>().TurnOnPanel();
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
    }
}
