using com.VisionXR.Controllers;
using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TurnAndFoulCanvas : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AudioDataSO audioData;
    public GameDataSO gameData;
    public PlayerDataSO playerData;
    public UIDataSO uiData;
    public UserDataSO userData;
    public StrikerDataSO strikerData;


    [Header("UI Panels")]
    public GameObject turnPanel;
    public GameObject foulPanel;
    public GameObject foulHandlingPanel;

    [Header("UI Elements")]
    public TMP_Text turnText;
    public TMP_Text foulText;
    public TMP_Text foulHandlingText;

    private void OnEnable()
    {
        uiData.ShowTurnEvent += ShowPlayerName;
        uiData.ShowFoulEvent += ShowFoul;
        uiData.ShowFoulHandlingEvent += ShowFoulHandling;

        strikerData.FoulCompleteEvent += HideFoulHandlingPanel;
    }

    private void OnDisable()
    {
        uiData.ShowTurnEvent -= ShowPlayerName;
        uiData.ShowFoulEvent -= ShowFoul;
        uiData.ShowFoulHandlingEvent -= ShowFoulHandling;

        strikerData.FoulCompleteEvent -= HideFoulHandlingPanel;
    }

    private void ShowFoulHandling()
    {

        StartCoroutine(ShowFoulHandlingPanelCoroutine());

    }

    private void HideFoulHandlingPanel()
    {
        foulHandlingText.text = "";
        foulHandlingPanel.SetActive(false);
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
        foulPanel.SetActive(false);
    }

    private IEnumerator ShowFoulHandlingPanelCoroutine()
    {
        yield return new WaitForSeconds(1f);
        foulHandlingPanel.SetActive(true);
        string displayText = "";
        bool handTrackingActive = false;

        if (userData.myDominantHand == DominantHand.Right)
        {
            if (handTrackingActive)
            {
                displayText = "Hold right hand pinch to move the striker. Release to place.";
              
            }
            else
            {
                displayText = "Hold right trigger to move the striker. Release to place";
              
            }
        }
        else
        {
            if (handTrackingActive)
            {
                displayText = "Hold left hand pinch to move the striker. Release to place.";
              
            }
            else
            {
                displayText = "Hold left trigger to move the striker. Release to place";
              
            }
        }
        foulHandlingText.text = displayText;
        yield return new WaitForSeconds(3f);
        foulHandlingText.text = "";
        foulHandlingPanel.SetActive(false);
    }
}
