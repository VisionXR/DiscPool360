using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using Photon.Voice.Unity;
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




    public void GainChanged()
    {
        
        gainText.text = "Gain: " + gainSlider.value.ToString();

        Player mp = playerData.GetMainPlayer();

        if(mp == null)
        {
            Debug.LogError("Main player not found!");
            return;
        }

        WebRtcAudioDsp dsp = mp.GetComponent<PlayerVoiceControl>().dsp;

        dsp.AgcCompressionGain = (int)gainSlider.value;
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

        WebRtcAudioDsp dsp = mp.GetComponent<PlayerVoiceControl>().dsp;

        dsp.AgcTargetLevel = (int)targetLevelSlider.value;
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

        WebRtcAudioDsp dsp = mp.GetComponent<PlayerVoiceControl>().dsp;

        dsp.ReverseStreamDelayMs = (int)streamDelaySlider.value;
    }
}
