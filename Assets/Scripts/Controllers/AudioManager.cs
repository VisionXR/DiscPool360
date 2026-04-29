using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{

    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Data")]
        public AudioDataSO audioData;
        

        [Header("Audio Clips")]
        public List<AudioClipData> audioClips;


        private void OnEnable()
        {
            audioData.PlayAudioEvent += OnPlayAudioEvent;
        }

        private void OnDisable()
        {
            audioData.PlayAudioEvent -= OnPlayAudioEvent;
        }

        private void OnPlayAudioEvent(AudioClipType type)
        {
            foreach (AudioClipData clipData in audioClips)
            {
                if (clipData.audioClipType == type)
                {
                    clipData.audioSource.Play();
                    break;
                }
            }
        }

 
    }


}
