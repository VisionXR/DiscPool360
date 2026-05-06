using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class InputCanvasView : MonoBehaviour
    {

        public List<GameObject> panelsToOff;
        public void TurnOff()
        {
           gameObject.SetActive(false);
        }

        public void TurnOn()
        {
            gameObject.SetActive(true);

        }
    }
}
