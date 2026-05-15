using com.VisionXR.HelperClasses;
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

        [Header("Navigation Panels")]
        public PanelOnOff leftSideNavigation;
        public PanelOnOff rightSideNavigation;

        [Header("Off Panels")]
        public List<PanelOnOff> panelsToOff;


        private void OnEnable()
        {
           


        }

        public void ShowPoolUI()
        {
            Reset();

            if (userData.myCoins == CoinsType.EightPool)
            {
                EightpoolScorePanelView.enabled = true;
                FivepoolScorePanelView.enabled = false;

                lefteightPoolImages.SetActive(true);
                righteightPoolImages.SetActive(true);
            }
            else if (userData.myCoins == CoinsType.FivePool)
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

            if (userData.myDominantHand == DominantHand.Left)
            {
              
                rightSideNavigation.TurnOnPanel();
               
            }
            else if (userData.myDominantHand == DominantHand.Right)
            {
  
                leftSideNavigation.TurnOnPanel();
  
            }
        }

        public void TurnOff()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOffPanel();
            }


            if (userData.myDominantHand == DominantHand.Left)
            {

                rightSideNavigation.TurnOffPanel();

            }
            else if (userData.myDominantHand == DominantHand.Right)
            {

                leftSideNavigation.TurnOffPanel();

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
