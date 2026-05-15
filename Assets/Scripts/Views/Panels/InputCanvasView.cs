using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class InputCanvasView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        


        [Header("Panel Objects")]
        public GameObject leftSidePanel;
        public GameObject rightSidePanel;



        public void TurnOff()
        {
           gameObject.SetActive(false);
        }

        public void TurnOn()
        {
            gameObject.SetActive(true);
            if (userData.myDominantHand == DominantHand.Right)
            {
                Debug.Log("Right Handed User");
                rightSidePanel.SetActive(true);
                leftSidePanel.SetActive(false);
            }
            else
            {
                Debug.Log("Left Handed User");
                rightSidePanel.SetActive(false);
                leftSidePanel.SetActive(true);
            }

           

        }
    }
}
