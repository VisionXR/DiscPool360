using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ChangeDestinationPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public DestinationSO destinationData;
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public GameDataSO gameData;


    [Header("Local Objects")]
    public TMP_Text quitText;
    public TMP_Text connectionText;

    [Header("Buttons")]
    public GameObject HomeBtn;
    public GameObject RetryBtn;
    public GameObject YesBtn;
    public GameObject NoBtn;

    // local actions
    private Action DestinationSuccessEvent;
    private Action<string> DestinationFailureEvent;
    private Destination currentDestination;

    private Coroutine connectionRoutine = null;


    private void OnEnable()
    {
        DestinationSuccessEvent += OnSuccess;
        DestinationFailureEvent += OnFailure;

        HomeBtn.SetActive(false);
        RetryBtn.SetActive(false);
    }

    private void OnDisable()
    {
        DestinationSuccessEvent -= OnSuccess;
        DestinationFailureEvent -= OnFailure;
    }



    private void OnSuccess()
    {
        if (connectionRoutine != null)
        {
            StopCoroutine(connectionRoutine);
            connectionRoutine = null;
        }
       gameObject.SetActive(false);
    }

    private void OnFailure(string msg)
    {
        if (connectionRoutine != null)
        {
            StopCoroutine(connectionRoutine);
            connectionRoutine = null;
        }
        HomeBtn.SetActive(true);
        RetryBtn.SetActive(true);
    }

    public void SetDestination(Destination destination)
    {
        currentDestination = destination;
        quitText.gameObject.SetActive(true);
        YesBtn.SetActive(true);
        NoBtn.SetActive(true);
    }

    public void ConnectToDestination(Destination destination)
    {
        if (destination != null)
        {
        
            currentDestination = destination;
            destinationData.ConnectToDestination(destination, DestinationSuccessEvent, DestinationFailureEvent);       
        }
    }

    public void HomeBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        HomeBtn.SetActive(false);
        RetryBtn.SetActive(false);

        uiData.TriggerHomeEvent();
        gameObject.SetActive(false);
    }

    public void RetryBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        HomeBtn.SetActive(false);
        RetryBtn.SetActive(false);

        ConnectToDestination(currentDestination);
    }

    public void YesBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        gameData.ExitGame();     
        YesBtn.SetActive(false);
        NoBtn.SetActive(false);
        quitText.gameObject.SetActive(false);
        if (connectionRoutine == null)
        {
            connectionRoutine = StartCoroutine(ShowConnectionStatus());
        }

        StartCoroutine(WaitAndConnect());

    }

    private IEnumerator WaitAndConnect()
    {
        yield return new WaitForSeconds(1);
        uiData.ResetAllPanels();
        ConnectToDestination(currentDestination);
    }

    public void NoBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        gameObject.SetActive(false);
    }


    private IEnumerator ShowConnectionStatus()
    {
        while (true)
        {
            yield return null;
            connectionText.text = "Connecting to destination";
            yield return new WaitForSeconds(0.2f);
            connectionText.text = "Connecting to destination..";
            yield return new WaitForSeconds(0.2f);
            connectionText.text = "Connecting to destination....";
            yield return new WaitForSeconds(0.2f);
            connectionText.text = "Connecting to destination..";
            yield return new WaitForSeconds(0.2f);
        }
    }
}
