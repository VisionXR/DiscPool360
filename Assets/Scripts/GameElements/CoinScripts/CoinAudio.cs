using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Com.VisionXR.GameElements
{
    public class CoinAudio : MonoBehaviour
    {
        public BoardDataSO boardData;
        public Rigidbody coinRigidbody;

        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private float CutOffVelocityForMaxAudio = 1f;
        [SerializeField] private float MinVelocityToPlayAudio = 0.005f;


        public void PlayCoinCollisionAudio(float volume)
        {
            if (audioSources.Length > 0)
            {
                audioSources[0].volume = volume;
                audioSources[0].Play();
            }
        }

        public void PlayEdgeCollisionAudio(float volume)
        {
            if (audioSources.Length > 1)
            {
                audioSources[1].volume = volume;
                audioSources[1].Play();
            }
        }

        public void PlayCoinFellInHoleAudio(float volume)
        {
            if (audioSources.Length > 2)
            {
                audioSources[2].Play();
                Invoke("DeActivate", 0.5f);
            }
        }

        private void DeActivate()
        {
            coinRigidbody.gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (coinRigidbody == null)
            {
                return;
            }

            GameObject other = collision.gameObject;
            string otherTag = other.tag;

            // Hole
            if (otherTag == "Hole")
            {

                PlayCoinFellInHoleAudio(1f);
                return;
            }

            // Edge
            if (otherTag == "Edge")
            {
                float speed = coinRigidbody.linearVelocity.magnitude;
                if (speed < MinVelocityToPlayAudio)
                {
                    return;
                }

                float volume = speed >= CutOffVelocityForMaxAudio
                    ? 1f
                    : (speed - MinVelocityToPlayAudio) / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio);

                PlayEdgeCollisionAudio(Mathf.Clamp01(volume));
                return;
            }

            // Coin or striker collision
            if (other.layer == LayerMask.NameToLayer("Coin") )
            {
                // Use relative velocity provided by physics
                float relSpeed = collision.relativeVelocity.magnitude;


                if (relSpeed < MinVelocityToPlayAudio)
                {
                    return;
                }

                float volume = relSpeed >= CutOffVelocityForMaxAudio
                    ? 1f
                    : (relSpeed - MinVelocityToPlayAudio) / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio);

                PlayCoinCollisionAudio(Mathf.Clamp01(volume));
            }

            if( otherTag == "Striker")
            {
                // Use relative velocity provided by physics
                float relSpeed = collision.relativeVelocity.magnitude;


                if (relSpeed < MinVelocityToPlayAudio)
                {
                    return;
                }

                float volume = relSpeed >= CutOffVelocityForMaxAudio
                    ? 1f
                    : (relSpeed - MinVelocityToPlayAudio) / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio);

                PlayCoinCollisionAudio(Mathf.Clamp01(volume));
            }
        }


    }
}
