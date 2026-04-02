using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurchaseDataSO", menuName = "ScriptableObjects/PurchaseDataSO", order = 1)]
public class PurchaseDataSO : ScriptableObject
{
   

    [Header(" Board skus")]
    public List<AssetData> BoardsData;


    [Header(" Player skus")]
    public string[] allSkusData;


    // Actions
    public Action BoardAssetPurchasedEvent;
    public Action GetPurchasedItemsEvent;
    public Action GetAllItemsEvent;
    public Action RefreshDataEvent;

    // Methods

    public void RefreshData()
    {
        RefreshDataEvent?.Invoke();
    }


    public AssetData GetBoardDataById(int id)
    {
        return BoardsData[id];
    }

    public void MarkBoardAsPurchased(int id)
    {
        AssetData board = GetBoardDataById(id);
        if (board != null)
        {
            board.isPurchased = true;
        }
        BoardAssetPurchasedEvent?.Invoke();
    }

    public void InitialisePurchases(List<string> purchasedSkus)
    {
        foreach (var sku in purchasedSkus)
        {
           
            foreach (var board in BoardsData)
            {
                if (board.skuName == sku)
                {
                    board.isPurchased = true;
                    BoardAssetPurchasedEvent?.Invoke();
                }
            }

        }
    }

    public void SetPurchasePrices(List<AssetData> assetDatas)
    
    {
        foreach(var assetData in assetDatas)
        {
            
            foreach (var board in BoardsData)
            {
                if(board.skuName == assetData.skuName)
                {
                    board.Price = assetData.Price;
                }
            }

        }
    }
    public void GetPurchasedItems()
    {
        GetPurchasedItemsEvent?.Invoke();
    }

    public void GetAllItems()
    {
        GetAllItemsEvent?.Invoke();
    }
}
