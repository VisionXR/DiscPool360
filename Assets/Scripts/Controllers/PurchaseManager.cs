using com.VisionXR.HelperClasses;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing; // Required for Unity IAP

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    [Header("Scriptable Objects")]
    public PurchaseDataSO purchaseData;

    private IStoreController storeController; // Handles purchases and queries
    private IExtensionProvider storeExtensionProvider; // Accesses platform-specific stores (Google Play)

    private void Start()
    {
        InitializeUnityIAP();
    }

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

    private void InitializeUnityIAP()
    {
        // Don't initialize if already set up or if you don't have IDs configured
        if (storeController != null || purchaseData.allSkusData == null)
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Register all your board IDs from your ScriptableObject as Non-Consumable items
        foreach (string sku in purchaseData.allSkusData)
        {
            builder.AddProduct(sku, ProductType.NonConsumable);
        }

        // Kick off asynchronous initialization
        UnityPurchasing.Initialize(this, builder);
    }

    public void RefreshData()
    {
        GetAllItems();
        GetPurchasedItems();
    }

    // 1. Fetches items that the player has already bought
    public void GetPurchasedItems()
    {
        if (storeController == null)
        {
            Debug.LogWarning("IAP not initialized yet.");
            return;
        }

        List<string> purchasedSkus = new List<string>();


        List<AssetData> assetDatas = new List<AssetData>();

        // Loop through all products registered in the application
        foreach (var product in storeController.products.all)
        {
            AssetData data = new AssetData();
            data.productId = product.definition.id;
            // If it's owned (non-consumables retain ownership on the device/account)
            if (product.hasReceipt)
            {
                data.isPurchased = true;
                assetDatas.Add(data);
                
            }
        }

        Debug.Log("Got all purchases");
        // Send the owned SKUs back to your ScriptableObject data holder
        purchaseData.SetPurchasedItems(assetDatas);
    }

    // 2. Fetches all registered products along with their localized currency prices
    public void GetAllItems()
    {
        if (storeController == null)
        {
            Debug.LogWarning("IAP not initialized yet.");
            return;
        }

        List<AssetData> assetDatas = new List<AssetData>();

        foreach (var product in storeController.products.all)
        {
            AssetData data = new AssetData();
            data.productId = product.definition.id;
          
            // metadata.localizedPriceString automatically handles local currency conversion (e.g., "$4.99", "€4.99")
            data.Price = product.metadata.localizedPriceString;

            assetDatas.Add(data);
        }

        Debug.Log("Got all items");
        purchaseData.SetPriceOfItems(assetDatas);
    }

    // 3. Call this method from your UI buttons when a user wants to buy a board
    public void BuyProduct(string productId)
    {
        if (storeController != null)
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.LogError("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.LogError("BuyProductID FAIL. Not initialized.");
        }
    }

    #region Unity IAP Interface Implementation (IStoreListener)

    // Automatically runs when Unity IAP successfully connects to Google Play Store
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Unity IAP Initialization Complete.");
        storeController = controller;
        storeExtensionProvider = extensions;

        // Populate your UI and data fields once connection is live
        RefreshData();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Unity IAP Initialization Failed: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Unity IAP Initialization Failed: {error}. Message: {message}");
    }

    // Automatically triggers when a purchase attempt completes successfully
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string prodctId = args.purchasedProduct.definition.id;
        Debug.Log($"Purchase Successful: {prodctId}");

        purchaseData.MarkBoardAsPurchased(prodctId);
        // Instantly recalculate owned items to grant access to the board
        RefreshData();

        // Return Complete to signal Unity IAP to automatically handle transaction acknowledgement 
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase of {product.definition.id} failed due to: {failureReason}");
    }

    #endregion
}