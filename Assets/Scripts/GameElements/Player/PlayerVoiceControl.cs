using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using Photon.Voice.Unity;
using UnityEngine;

public class PlayerVoiceControl : MonoBehaviour { 

    [Header(" Scriptable Objects Objects")]
    public UIDataSO uiData;

    [Header(" Local Objects")]
    public Player currentPlayer;
    public AudioSource speaker;
    public Recorder recorder;

    public void OnEnable()
    {
        uiData.TurnOnMicEvent += TurnOnMic;
        uiData.TurnOffMicEvent += TurnOffMic;
        uiData.TurnOnSpeakerEvent += TurnOnSpeaker;
        uiData.TurnOffSpeakerEvent += TurnOffSpeaker;
    }

    private void OnDisable()
    {
        uiData.TurnOnMicEvent -= TurnOnMic;
        uiData.TurnOffMicEvent -= TurnOffMic;
        uiData.TurnOnSpeakerEvent -= TurnOnSpeaker;
        uiData.TurnOffSpeakerEvent -= TurnOffSpeaker;
    }

    private void TurnOnSpeaker()
    {
        if (currentPlayer.playerProperties.myPlayerType == PlayerType.Human && currentPlayer.playerProperties.myPlayerControl == PlayerControl.Remote)
        {
            speaker.mute = false;
        }
    }

    private void TurnOffSpeaker()
    {
        if (currentPlayer.playerProperties.myPlayerType == PlayerType.Human && currentPlayer.playerProperties.myPlayerControl == PlayerControl.Remote)
        {
            speaker.mute = true;
        }
    }

    private void TurnOnMic()
    {
        if (currentPlayer.playerProperties.myPlayerType == PlayerType.Human && currentPlayer.playerProperties.myPlayerControl == PlayerControl.Local)
        {
            recorder.TransmitEnabled = true;
        }
    }

    private void TurnOffMic()
    {
        if (currentPlayer.playerProperties.myPlayerType == PlayerType.Human && currentPlayer.playerProperties.myPlayerControl == PlayerControl.Local)
        {
            recorder.TransmitEnabled = false;
        }
    }
}
