using com.VisionXR.Controllers;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestinationPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public DestinationSO destinationData;
    public AudioDataSO audioData;
    public UIDataSO uiData;

    [Header("This Objects")]
    public List<PanelOnOff> panelsToOff;


    [Header("Local Objects")]
    public TMP_Text connectionText;
    public GameObject HomeBtn;
    public GameObject RetryBtn;

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

    public void ConnectToDestination(Destination destination)
    {
        if (destination != null)
        {
            if (connectionRoutine == null)
            {
                connectionRoutine = StartCoroutine(ShowConnectionStatus());
            }
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
    }

    public void RetryBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        HomeBtn.SetActive(false);
        RetryBtn.SetActive(false);

        ConnectToDestination(currentDestination);
    }

    public void TurnOff()
    {
        foreach (PanelOnOff panel in panelsToOff)
        {
            panel.TurnOffPanel();
        }
        StartCoroutine(WaitAndTurnOff());
    }

    private IEnumerator WaitAndTurnOff()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }


    public void TurnOn()
    {
        gameObject.SetActive(true);

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
