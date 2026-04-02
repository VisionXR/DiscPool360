using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PurchaseDataSO purchaseData;
    public AudioDataSO audioData;


    [Header("List Elements")]
    public GameObject boardsPanel;
    public List<TMP_Text> boardPriceTexts;


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

    public void BackBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        boardsPanel.SetActive(true);
        gameObject.SetActive(false);

    }
    public void CrossButtonClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        gameObject.SetActive(false);
    }

    private void SetProductPrices()
    {
    
        for (int i = 0; i < purchaseData.BoardsData.Count; i++)
        {
            if (boardPriceTexts[i] != null)
            {
                if(purchaseData.BoardsData[i].isPurchased)
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


}
