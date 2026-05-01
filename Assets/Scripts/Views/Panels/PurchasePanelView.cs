using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class PurchasePanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public PurchaseDataSO purchaseData;
        public AudioDataSO audioData;


        [Header("List Elements")]
        public GameObject boardsPanel;
        public List<TMP_Text> boardPriceTexts;

        [Header("This Objects")]
        public HomePanelView homePanelView;
        public List<PanelOnOff> panelsToOff;


        private void OnEnable()
        {

            purchaseData.BoardAssetPurchasedEvent += SetProductPrices;

            SetProductPrices();
        }

        private void OnDisable()
        {

            purchaseData.BoardAssetPurchasedEvent -= SetProductPrices;

        }

        public void BoardBundleClicked(int id)
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            string sku = purchaseData.BoardsData[id].skuName;

        }

        public void RefreshButtonClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            purchaseData.GetAllItems();
            purchaseData.GetPurchasedItems();
        }


        private void SetProductPrices()
        {

            for (int i = 0; i < purchaseData.BoardsData.Count; i++)
            {
                if (boardPriceTexts[i] != null)
                {
                    if (purchaseData.BoardsData[i].isPurchased)
                    {
                        boardPriceTexts[i].text = "Purchased";
                    }
                    else
                    {
                        boardPriceTexts[i].text = purchaseData.BoardsData[i].Price;
                    }
                }
            }

        }


        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            TurnOff();
            
        }

        public void TurnOff()
        {

            StartCoroutine(WaitAndTurnOff());
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
