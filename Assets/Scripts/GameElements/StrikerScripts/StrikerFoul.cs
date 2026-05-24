using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class StrikerFoul : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public StrikerDataSO strikerData;
    public UIDataSO uiData;

    [Header("Local Objects")]
    public GameObject GlowStriker;
    public float GlowDuration = 0.3f;
    private Coroutine glowRoutine;
    private void OnEnable()
    {
        uiData.ShowFoulHandlingEvent += StartGlow;
        strikerData.FoulCompleteEvent += StopGlow;
    }

    private void OnDisable()
    {
        uiData.ShowFoulHandlingEvent -= StartGlow;
        strikerData.FoulCompleteEvent -= StopGlow;
    }

    private void StartGlow()
    {
        if(glowRoutine == null)
        {
            glowRoutine = StartCoroutine(GlowContinously());
        }
    }

    private void StopGlow()
    {
        if(glowRoutine != null)
        {
            StopCoroutine(glowRoutine);
            glowRoutine = null;
        }
    }

    private IEnumerator GlowContinously()
    {
        while(true)
        {
            GlowStriker.SetActive(true);
            yield return new WaitForSeconds(GlowDuration);
            GlowStriker.SetActive(false);
            yield return new WaitForSeconds(GlowDuration);
        }
    }
}
