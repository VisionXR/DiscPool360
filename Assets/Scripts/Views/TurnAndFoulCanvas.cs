using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TurnAndFoulCanvas : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AudioDataSO audioData;
    public GameDataSO gameData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;

    [Header("UI Elements")]
    public GameObject turnPanel;
    public GameObject foulPanel;
    public TMP_Text turnText;
    public TMP_Text foulText;
    public TMP_Text foulHandlingText;

    private void OnEnable()
    {
        gameData.TurnChangeEvent += ShowPlayerName;
        uiData.ShowFoulEvent += ShowFoul;
        uiData.ShowFoulHandlingEvent += ShowFoulHandling;
    }

    private void OnDisable()
    {
        gameData.TurnChangeEvent -= ShowPlayerName;
        uiData.ShowFoulEvent -= ShowFoul;
        uiData.ShowFoulHandlingEvent -= ShowFoulHandling;
    }

    private void ShowFoulHandling(string displayText)
    {
        StartCoroutine(ShowFoulPanelCoroutine());
        foulHandlingText.text = displayText;
    }

    private void ShowFoul()
    {

        StartCoroutine(ShowFoulPanelCoroutine());
        foulText.text = "Foul!";
    }

    private void ShowPlayerName(int playerId)
    {
        StartCoroutine(ShowTurnPanelCoroutine());
        Player player = playerData.GetPlayerById(playerId);
        if (player != null)
        {
            if (player.playerProperties.myPlayerType == PlayerType.Human && player.playerProperties.myPlayerControl == PlayerControl.Local)
            {
                turnText.text = "Your Turn";
            }
            else
            {
                turnText.text = $"{player.playerProperties.myName}'s Turn";
            }
        }
        
        audioData.PlayAudio(AudioClipType.TurnChange);
    }

    private IEnumerator ShowTurnPanelCoroutine()
    {
        turnPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        turnText.text = "";      
        turnPanel.SetActive(false);
    }

    private IEnumerator ShowFoulPanelCoroutine()
    {
        foulPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        foulText.text = "";
        foulHandlingText.text = "";
        foulPanel.SetActive(false);
    }
}
