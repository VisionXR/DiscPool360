using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class PoolCanvasView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;

        [Header("Pool Score Panel View")]
        public PoolScorePanelView FivepoolScorePanelView;
        public PoolScorePanelView EightpoolScorePanelView;

        public GameObject leftfivePoolImages;
        public GameObject rightfivePoolImages;
        public GameObject lefteightPoolImages;
        public GameObject righteightPoolImages;

        [Header("Off Panels")]
        public List<PanelOnOff> panelsToOff;


        void OnEnable()
        {
            Reset();

            if (userData.myCoins == 0)
            {
                EightpoolScorePanelView.enabled = true;
                FivepoolScorePanelView.enabled = false;

                lefteightPoolImages.SetActive(true);
                righteightPoolImages.SetActive(true);
            }
            else if (userData.myCoins == 1)
            {
                FivepoolScorePanelView.enabled = true;
                EightpoolScorePanelView.enabled = false;

                leftfivePoolImages.SetActive(true);
                rightfivePoolImages.SetActive(true);
            }
         
        }

        public void TurnOn()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOnPanel();
            }
        }

        public void TurnOff()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOffPanel();
            }
        }



        private void Reset()
        {
            leftfivePoolImages.SetActive(false);
            rightfivePoolImages.SetActive(false);
            lefteightPoolImages.SetActive(false);
            righteightPoolImages.SetActive(false);
            FivepoolScorePanelView.enabled = false;
            EightpoolScorePanelView.enabled = false;

        }
    }
}
