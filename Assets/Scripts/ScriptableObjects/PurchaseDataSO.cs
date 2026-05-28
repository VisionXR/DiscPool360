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

    public AssetData GetBoardByProductId(string id)
    {
        foreach (var item in BoardsData)
        {
            if (item.productId == id) return item;
        }
        return null;
    }

    public void MarkBoardAsPurchased(string id)
    {
        AssetData board = GetBoardByProductId(id);
        if (board != null)
        {
            board.isPurchased = true;
        }
        BoardAssetPurchasedEvent?.Invoke();
    }


    public void SetPurchasedItems(List<AssetData> productdIds)    
    {
        foreach(var id in productdIds)
        {
            
            foreach (var board in BoardsData)
            {
                if(board.productId == id.productId)
                {
                    board.isPurchased = true;
                    Debug.Log("Purchased Item " + board.skuName);
                }
            }

        }
    }

    public void SetPriceOfItems(List<AssetData> productdIds)
    {
        foreach (var id in productdIds)
        {

            foreach (var board in BoardsData)
            {
                if (board.productId == id.productId)
                {
                    board.Price = id.Price;
                    Debug.Log("Price Item " + board.Price);
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
