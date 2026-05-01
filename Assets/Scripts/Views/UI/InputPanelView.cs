using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class InputPanelView : MonoBehaviour
    {

        public List<PanelOnOff> panelsToOff;
        public void TurnOff()
        {
           
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }


        public void TurnOn()
        {
            gameObject.SetActive(true);
       

        }
    }
}
