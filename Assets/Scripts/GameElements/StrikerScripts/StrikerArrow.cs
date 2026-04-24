using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class StrikerArrow : MonoBehaviour
    {

        [Header(" Striker Arrow Settings")]
        public StrikerDataSO strikerData;

        [Header(" Game Objects")]
        public GameObject arrow;
        public GameObject displayArrow;
        public GameObject aimLine;
        public Renderer arrowRenderer;
     
        private void OnEnable()
        {                 
            strikerData.TurnOnArrowEvent += TurnOnArrow;
            strikerData.TurnOffArrowEvent += TurnOffArrow;
        }

        private void OnDisable()
        {
            strikerData.TurnOnArrowEvent -= TurnOnArrow;
            strikerData.TurnOffArrowEvent -= TurnOffArrow;
        }

        public void TurnOnArrow()
        {
            arrow.SetActive(true);
            displayArrow.SetActive(true);
            aimLine.SetActive(true);

        }

        public void TurnOnDisplayArrow()
        {
            displayArrow.SetActive(true);
        }

        public void TurnOffDisplayArrow()
        {
            displayArrow.SetActive(false);
        }

        public void TurnOffArrow()
        {
            arrowRenderer.material.SetFloat("_Threshold", 0);
            arrow.SetActive(false);
            displayArrow.SetActive(false);
            aimLine.SetActive(false);
             
        }

        public void ChangeColorOfArrow(float value)
        {

            if (arrowRenderer.material.HasProperty("_Threshold"))
            {
                // Set the new threshold value
                arrowRenderer.material.SetFloat("_Threshold", value);
            }
        }



    }
}
