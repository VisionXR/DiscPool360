using Fusion;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Android; // Required for Android permissions

public class VoicePermissionHandler : NetworkBehaviour
{
    public Recorder photonRecorder;

    void Start()
    {
        if (HasStateAuthority)
        {
            if (Application.isEditor)
            {
                StartRecording();
            }

            else
            {

                if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
                {
                    // This triggers the system popup
                    var callbacks = new PermissionCallbacks();
                    callbacks.PermissionGranted += OnPermissionGranted;
                    Permission.RequestUserPermission(Permission.Microphone, callbacks);
                }
                else
                {
                    StartRecording();
                }
            }

        }
    }

    void OnPermissionGranted(string permissionName)
    {
        if (permissionName == Permission.Microphone)
        {
            StartRecording();
        }
    }

    void StartRecording()
    {
        if (photonRecorder != null)
        {
            photonRecorder.RecordingEnabled = true;
            photonRecorder.RestartRecording();
            Debug.Log("Microphone recording started after permission granted.");
        }
    }
}