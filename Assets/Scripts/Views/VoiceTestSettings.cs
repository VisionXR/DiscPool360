using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using Photon.Voice.Unity;
using POpusCodec.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoiceTestSettings : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PlayerDataSO playerData;

    [Header("Voice Test Settings")]
    public Slider gainSlider;
    public Slider targetLevelSlider;
    public Slider streamDelaySlider;

    public TMP_Text gainText;
    public TMP_Text targetLevelText;
    public TMP_Text streamDelayText;

    public TMP_Dropdown samplingDD;
    public TMP_Dropdown micTypeDD;



    private void OnEnable()
    {
        Player mp = playerData.GetMainPlayer();

        if (mp == null)
        {
            Debug.LogError("Main player not found!");
            return;
        }

    }


    public void GainChanged()
    {
        
        gainText.text = "Gain: " + gainSlider.value.ToString();

        Player mp = playerData.GetMainPlayer();

        if(mp == null)
        {
            Debug.LogError("Main player not found!");
            return;
        }

    }

    public void TargetLevelChanged()
    {
        targetLevelText.text = "Target Level: " + targetLevelSlider.value.ToString();

        Player mp = playerData.GetMainPlayer();

        if (mp == null)
        {
            Debug.LogError("Main player not found!");
            return;
        }
    }

    public void StreamDelayChanged()
    {
        streamDelayText.text = "Delay: " + streamDelaySlider.value.ToString() + "ms";

        Player mp = playerData.GetMainPlayer();

        if (mp == null)
        {
            Debug.LogError("Main player not found!");
            return;
        }

    }

    public void MicTypeChanged()
    {
        Player mp = playerData.GetMainPlayer();
        Recorder mainRecorder = mp.GetComponent<PlayerVoiceControl>().recorder;

        if (micTypeDD.value == 0)
        {
            mainRecorder.MicrophoneType = Recorder.MicType.Unity;

        }
        else
        {
            mainRecorder.MicrophoneType = Recorder.MicType.Photon;
        }
    }

    public void SamplingRateChanged()
    {
        Player mp = playerData.GetMainPlayer();
        Recorder mainRecorder = mp.GetComponent<PlayerVoiceControl>().recorder;

        mainRecorder.SamplingRate = (SamplingRate)samplingDD.value;
    }
}
