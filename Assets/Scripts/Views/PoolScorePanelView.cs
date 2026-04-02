using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoolScorePanelView : MonoBehaviour
{
    [Header("Data References")]
    public AudioDataSO audioData;
    public GameDataSO gameData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;
    public CoinDataSO coinData;


    [Header("P1 References")]
    public TMP_Text P1NameText;
    public TMP_Text P1CoinText;
    public Image P1TurnIndicatorImage;
    public Image P1PlayerImage;
    public List<Image> P1Coins;
    public List<Image> P1CoinsHighlight;

    [Header("P2 References")]
    public TMP_Text P2NameText;
    public TMP_Text P2CoinText;
    public Image P2TurnIndicatorImage;
    public Image P2PlayerImage;
    public List<Image> P2Coins;
    public List<Image> P2CoinsHighlight;

    [Header("Sprites")]
    public List<Sprite> solidImages;
    public List<Sprite> stripeImages;
    public Sprite blackImage;



    const float activeAlpha = 1f;
    const float inactiveAlpha = 0.1f;


    private void OnEnable()
    {
       
        gameData.TurnChangeEvent += ShowIndicator;
        uiData.SetCoinsEvent += SetCoins;
        uiData.UpdateCoinsEvent += UpdateCoins;
        playerData.PlayerImageReceivedEvent += SetPlayerImage;

        SetPlayerData(1);
    }

    private void OnDisable()
    {
        
        gameData.TurnChangeEvent -= ShowIndicator;
        uiData.SetCoinsEvent -= SetCoins;
        uiData.UpdateCoinsEvent -= UpdateCoins;
        playerData.PlayerImageReceivedEvent -= SetPlayerImage;

    }

    private void SetPlayerImage(int id, Sprite sprite)
    { 
        if(id == 1)
        {
            P1PlayerImage.sprite = sprite;
        }
        else if(id == 2)
        {
            P2PlayerImage.sprite = sprite;
        }
    }

    private void SetPlayerData(int id)
    {
        ResetCoins();

        Player p1 = playerData.GetPlayerById(1);
        if (p1 != null)
        {
            P1NameText.text = p1.playerProperties.myName;
            P1CoinText.text = "Open Table";
            P1PlayerImage.sprite = p1.playerProperties.myImage;

        }

        Player p2 = playerData.GetPlayerById(2);
        if (p2 != null)
        {
            P2NameText.text = p2.playerProperties.myName;
            P2CoinText.text = "Open Table";
            P2PlayerImage.sprite = p2.playerProperties.myImage;

        }
    }

    private void SetCoins()
    {
        ResetCoins();

        Player p1 = playerData.GetPlayerById(1);
        Player p2 = playerData.GetPlayerById(2);

        // Update group text
        if (p1 != null)
        {
            P1CoinText.text = Enum.GetName(typeof(PlayerCoin), p1.playerProperties.myCoin);
        }
        if (p2 != null)
        {
            P2CoinText.text = Enum.GetName(typeof(PlayerCoin), p2.playerProperties.myCoin);
        }

        // Assign sprites for P1: first 7 are group coins, 8th is black
        if (p1 != null)
        {
            List<Sprite> p1Source = p1.playerProperties.myCoin == PlayerCoin.Stripe ? stripeImages : solidImages;
            List<GameObject> groupList = p1.playerProperties.myCoin == PlayerCoin.Stripe ? coinData.stripes : coinData.solids;
            for (int i = 0; i < groupList.Count; i++)
            {
                Sprite s1 = null;
             
                s1 = (p1Source != null && i < p1Source.Count) ? p1Source[i] : null;

                P1Coins[i].sprite = s1;
                
            }

            
            Sprite  s = blackImage;
            P1Coins[groupList.Count].sprite = s;
            
        }

        // Assign sprites for P2: first 7 are group coins, 8th is black
        if (p2 != null)
        {
            List<Sprite> p2Source = p2.playerProperties.myCoin == PlayerCoin.Stripe ? stripeImages : solidImages;
            List<GameObject> groupList = p2.playerProperties.myCoin == PlayerCoin.Stripe ? coinData.stripes : coinData.solids;
            for (int i = 0; i < groupList.Count; i++)
            {
                Sprite s1 = null;

                s1 = (p2Source != null && i < p2Source.Count) ? p2Source[i] : null;

                P2Coins[i].sprite = s1;
               
            }

            Sprite s = blackImage;
            P2Coins[groupList.Count].sprite = s;
         
        }

    }

    private void UpdateCoins()
    {
        // Safety: ensure we have coin lists and UI slots
        if (P1Coins == null || P2Coins == null) return;

        // Update P1
        var p1 = playerData.GetPlayerById(1);
        UpdateSide(p1, P1CoinsHighlight);

        // Update P2
        var p2 = playerData.GetPlayerById(2);
        UpdateSide(p2, P2CoinsHighlight);

       
    }

    // Helper to update one side
    void UpdateSide(Player player, List<Image> uiCoins)
    {
        if (player == null || uiCoins == null) return;

        bool isStripe = player.playerProperties.myCoin == PlayerCoin.Stripe;
        bool isSolid = player.playerProperties.myCoin == PlayerCoin.Solid;

        // Only apply for assigned groups
        if (!isStripe && !isSolid) return;

        List<GameObject> groupList = isStripe ? coinData.stripes : coinData.solids;

        // For each group coin slot (up to available UI slots and group count)
        int groupCount =  groupList.Count;

        for (int i = 0; i < groupCount; i++)
        {
            var coinGo = groupList[i];
            bool isActive =   coinGo.activeSelf;
            uiCoins[i].gameObject.SetActive(!isActive);

        }


        // Black coin status at the next slot (groupCount) if within bounds
        int blackIndex = groupCount;

        bool blackActive =  coinData.black.activeSelf;
        uiCoins[blackIndex].gameObject.SetActive(!blackActive);
        
    }


    private void ShowIndicator(int turnId)
    {
        
        if (turnId == 1)
        {
            P1TurnIndicatorImage.color = Color.green;
            P2TurnIndicatorImage.color = Color.white;
        }
        else
        {
            P2TurnIndicatorImage.color = Color.green;
            P1TurnIndicatorImage.color = Color.white;
        }

        UpdateCoins();
    }

    private void ResetCoins()
    {
     
        foreach (Image image1 in P1Coins)
        {
            image1.sprite = null;
            image1.color = Color.white;      
        }

        foreach (Image image2 in P2Coins)
        {
            image2.sprite = null;
            image2.color = Color.white;          
        }

        foreach (Image image1 in P1CoinsHighlight )
        {
             image1.gameObject.SetActive(false);
        }

        foreach (Image image1 in P2CoinsHighlight)
        {
            image1.gameObject.SetActive(false);
        }
    }
    
}
