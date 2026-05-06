using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

namespace com.VisionXR.GameElements
{
    public class StrikerMovement : MonoBehaviour
    {
        [SerializeField, Tooltip("Minimum yaw change (degrees) required to update rotation.")]
        public StrikerDataSO strikerData;
        public float yawThresholdDegrees = 0.5f;
        public float velocityFactor = 0.5f;
        public Quaternion initialRotation = Quaternion.identity;
        public float degreesPerSecond = 60f; // Your target speed


        // Tracks the last yaw we actually applied (to enforce threshold between updates)
        private float _lastAppliedYaw;
        private Coroutine aimingRoutine;
        private void Awake()
        {
            _lastAppliedYaw = transform.eulerAngles.y;
            initialRotation = transform.rotation;
        }

        /// <summary>
        /// Rotates the striker to the specified absolute angle around the Y axis.
        /// </summary>
        /// <param name="angle">The target angle in degrees.</param>
        public void RotateAbsolute(float angle)
        {
            var currentEuler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, angle, 0);
            _lastAppliedYaw = angle;
        }


        public void RotateRelative(float swipeVelocity)
        {
            // If a routine is running, stop it to start a new "flick"
            if (aimingRoutine != null)
            {
                StopCoroutine(aimingRoutine);
            }

            aimingRoutine = StartCoroutine(RotateWithVelocity(swipeVelocity));
        }

        private IEnumerator RotateWithVelocity(float velocity)
        {

            
            // 1. Calculate how many total degrees we should rotate based on velocity
            // You can tweak this multiplier (0.1f) to make it more/less sensitive
            float totalRotationAmount = velocity * velocityFactor;

            Quaternion startRotation = transform.rotation;

            // 2. Define the final target rotation using Quaternions
            Quaternion targetRotation = startRotation * Quaternion.Euler(0, totalRotationAmount, 0);

            // 3. Rotate over time until we reach the target
            // We use RotateTowards for a constant speed, or Slerp for a smooth slow-down
            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                // Move towards the target at a constant speed of 30 degrees per second
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    degreesPerSecond * Time.deltaTime
                );

                yield return null;
            }

            // Snap to final to be precise
            transform.rotation = targetRotation;
            aimingRoutine = null;
        }

        // Clean helper for direct direction looking
        public void RotateTo(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }
    
    }
}