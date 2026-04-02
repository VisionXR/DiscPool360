using com.VisionXR.ModelClasses;
using UnityEngine;

namespace Com.VisionXR.GameElements
{
    public class StrikerAudio : MonoBehaviour
    {
        [Header(" Scriptable Objects")]
        public StrikerDataSO strikerData;
        public BoardDataSO boardData;

        [Header(" Local variables")]
        public Rigidbody strikerRigidbody;
        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private float CutOffVelocityForMaxAudio = 1f;
        [SerializeField] private float MinVelocityToPlayAudio = 0.005f;

        // audioSources[0] = coin, [1] = edge, [2] = hole
        public void PlayCoinCollisionAudio(float volume)
        {
            audioSources[0].volume = volume;
            audioSources[0].Play();
        }

        public void PlayEdgeCollisionAudio(float volume)
        {
            audioSources[1].volume = volume;
            audioSources[1].Play();
        }

        public void PlayCoinFellInHoleAudio(float volume)
        {
            audioSources[2].volume = volume;
            audioSources[2].Play();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (strikerRigidbody == null)
            {
                return;
            }

            GameObject other = collision.gameObject;
            string otherTag = other.tag;

            // Hole impact (if holes use colliders instead of triggers)
            if (otherTag == "Hole")
            {
                PlayCoinFellInHoleAudio(1f);
                return;
            }

            // Edge collision
            if (otherTag == "Edge")
            {
                

                float speed = strikerRigidbody.linearVelocity.magnitude;
                if (speed < MinVelocityToPlayAudio)
                {
                    return;
                }

                float volume = speed >= CutOffVelocityForMaxAudio
                    ? 1f
                    : (speed - MinVelocityToPlayAudio) / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio);

                volume = Mathf.Clamp01(volume);
                PlayEdgeCollisionAudio(volume);
                return;
            }

            // Coin collision (Stripe / Solid / Black)
            if (otherTag == "Stripe" || otherTag == "Solid" || otherTag == "Black" || otherTag == "Red"|| otherTag == "Color")
            {
                // Use Unity-provided relative velocity
                float relSpeed = collision.relativeVelocity.magnitude;

                if (relSpeed < MinVelocityToPlayAudio)
                {
                    return;
                }

                float volume = relSpeed >= CutOffVelocityForMaxAudio
                    ? 1f
                    : (relSpeed - MinVelocityToPlayAudio) / (CutOffVelocityForMaxAudio - MinVelocityToPlayAudio);

                volume = Mathf.Clamp01(volume);
                PlayCoinCollisionAudio(volume);
            }
        }
    }
}