using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnookerScorePanelView : MonoBehaviour
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
    public TMP_Text P1ScoreText;
    public Image P1TurnIndicatorImage;
    public Image P1PlayerImage;
    public Image P1CoinImage;

    [Header("P2 References")]
    public TMP_Text P2NameText;
    public TMP_Text P2CoinText;
    public TMP_Text P2ScoreText;
    public Image P2TurnIndicatorImage;
    public Image P2PlayerImage;
    public Image P2CoinImage;

    [Header("Sprites")]
    public Sprite redSprite;
    public Sprite yellowSprite;
    public Sprite greenSprite;
    public Sprite brownSprite;
    public Sprite blueSprite;
    public Sprite pinkSprite;
    public Sprite blackSprite;
    public Sprite anyColorSprite;


    private void OnEnable()
    {
        gameData.PlayAgainEvent += SetPlayerData;
        gameData.TurnChangeEvent += ShowIndicator;
        uiData.SetCoinsEvent += SetCoins;
        uiData.UpdateCoinsEvent += RefreshScoresAndCoins;
        SetPlayerData(1);
    }

    private void OnDisable()
    {
        gameData.PlayAgainEvent -= SetPlayerData;
        gameData.TurnChangeEvent -= ShowIndicator;
        uiData.SetCoinsEvent -= SetCoins;
        uiData.UpdateCoinsEvent -= RefreshScoresAndCoins;
    }

    private void SetPlayerData(int id)
    {
        // Names and avatars
        var p1 = playerData.GetPlayerById(1);
        var p2 = playerData.GetPlayerById(2);

        if (p1 != null)
        {
            P1NameText.text = p1.playerProperties.myName;
            P1PlayerImage.sprite = p1.playerProperties.myImage;
        }
        if (p2 != null)
        {
            P2NameText.text = p2.playerProperties.myName;
            P2PlayerImage.sprite = p2.playerProperties.myImage;
        }

        // Initial coin text + image and initial scores
        SetCoins();
        RefreshScoresAndCoins();
    }

    private void SetCoins()
    {
        var p1 = playerData.GetPlayerById(1);
        var p2 = playerData.GetPlayerById(2);

        if (p1 != null)
        {
            P1CoinText.text = Enum.GetName(typeof(PlayerCoin), p1.playerProperties.myCoin);
            P1CoinImage.sprite = GetSpriteForPlayerCoin(p1.playerProperties.myCoin);
            P1CoinImage.enabled = P1CoinImage.sprite != null;
        }

        if (p2 != null)
        {
            P2CoinText.text = Enum.GetName(typeof(PlayerCoin), p2.playerProperties.myCoin);
            P2CoinImage.sprite = GetSpriteForPlayerCoin(p2.playerProperties.myCoin);
            P2CoinImage.enabled = P2CoinImage.sprite != null;
        }
    }

    private void RefreshScoresAndCoins()
    {
        // Scores
        int p1Score = gameData.GetSnookerScore(1);
        int p2Score = gameData.GetSnookerScore(2);
        P1ScoreText.text = p1Score.ToString();
        P2ScoreText.text = p2Score.ToString();

        // Coin text + image may change as phase/expectation changes; refresh them
        SetCoins();
    }

    private Sprite GetSpriteForPlayerCoin(PlayerCoin coin)
    {
        switch (coin)
        {
            case PlayerCoin.Red:    return redSprite;
            case PlayerCoin.Color:  return anyColorSprite;
            case PlayerCoin.Yellow: return yellowSprite;
            case PlayerCoin.Green:  return greenSprite;
            case PlayerCoin.Brown:  return brownSprite;
            case PlayerCoin.Blue:   return blueSprite;
            case PlayerCoin.Pink:   return pinkSprite;
            case PlayerCoin.Black:  return blackSprite;
            default:                return null;
        }
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
    }

}
