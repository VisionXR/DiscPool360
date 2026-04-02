using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PurchaseDataSO purchaseData;



    private void OnEnable()
    {
        purchaseData.GetPurchasedItemsEvent += GetPurchasedItems;
        purchaseData.GetAllItemsEvent += GetAllItems;
        purchaseData.RefreshDataEvent += RefreshData;
    }

    private void OnDisable()
    {
        purchaseData.GetPurchasedItemsEvent -= GetPurchasedItems;
        purchaseData.GetAllItemsEvent -= GetAllItems;
        purchaseData.RefreshDataEvent -= RefreshData;
    }

    public void RefreshData()
    {
        GetAllItems();
        GetPurchasedItems();
    }

    public void GetPurchasedItems()
    {
        List<string> purchasedSkus = new List<string>();
      

    }
    

    public void GetAllItems()
    {
        List<AssetData> assetDatas = new List<AssetData>();
        //IAP.GetProductsBySKU(purchaseData.allSkusData).OnComplete((skuMsg) =>
        //{
        //    if (!skuMsg.IsError)
        //    {
        //        foreach (var product in skuMsg.Data)
        //        {
                   
        //            AssetData data = new AssetData();
        //            data.skuName = product.Sku;
        //            data.Price = product.Price.Formatted;

        //            assetDatas.Add(data);
        //            // Here you can add logic to handle available SKUs
        //        }

        //        purchaseData.SetPurchasePrices(assetDatas);
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to fetch available SKUs: " + skuMsg.GetError().Message);
        //    }
        //});
    }

    
  
}
