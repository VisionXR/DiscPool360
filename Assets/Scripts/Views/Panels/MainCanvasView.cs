using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class MainCanvasView : MonoBehaviour
    {
        [Header("All Panels")]
        public List<GameObject> allMainPanels;

        public void ShowMainPanel(int id)
        {
            
            allMainPanels[id].GetComponent<PanelOnOff>().TurnOnPanel();
        }

        public void HideMainPanel(int id)
        {
            Debug.Log("Trying to hide panel" + id);
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
