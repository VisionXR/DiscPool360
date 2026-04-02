using com.VisionXR.Controllers;
using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "AudioDataSO", menuName = "ScriptableObjects/AudioDataSO", order = 1)]
    public class AudioDataSO : ScriptableObject
    {
        // variables

     

        // Events
        public Action<AudioClipType> PlayAudioEvent;


        // Methods

        public void PlayAudio(AudioClipType clipType)
        {
            PlayAudioEvent?.Invoke(clipType);
        }
    }
}
