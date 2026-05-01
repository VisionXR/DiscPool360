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
        public Quaternion initialRotation = Quaternion.identity;

        // Tracks the last yaw we actually applied (to enforce threshold between updates)
        private float _lastAppliedYaw;
        private Coroutine aimingRoutine;
        private void Awake()
        {
            _lastAppliedYaw = transform.eulerAngles.y;
            initialRotation = transform.rotation;
        }


        public void AimStriker(Vector3 direction)
        {

            transform.rotation = initialRotation;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.eulerAngles = VectorUtility.RoundPositionUpto3Decimals(transform.eulerAngles);
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


        public void RotateRelative(float velocity)
        {
            //var currentEuler = transform.eulerAngles;
            //float newY = currentEuler.y + velocity;
            //transform.eulerAngles = new Vector3(0, newY, 0);
            //_lastAppliedYaw = newY;

            if (aimingRoutine == null)
            {
                aimingRoutine = StartCoroutine(RotateToDirectionSmoothly(velocity, strikerData.aimingDuration));
            }
        }

     
        private IEnumerator RotateToDirectionSmoothly(float velocity, float duration)
        {
            Vector3 currentEuler = transform.eulerAngles;
            float newY = currentEuler.y + velocity;
           
            Vector3 targetEuler = new Vector3(0, newY, 0);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetEuler, t);
                yield return null;
            }
            transform.eulerAngles = targetEuler;

            aimingRoutine = null;
        }

        public void RotateTo(Vector3 direction)
        {
            // Ignore invalid/zero directions
            if (direction.sqrMagnitude < 1e-8f)
            {
                return;
            }

            // Work on the XZ plane to avoid pitch/roll changes
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
            if (flatDir.sqrMagnitude < 1e-8f)
            {
                return;
            }
            flatDir.Normalize();

            // Compute target yaw
            float targetYaw = Mathf.Atan2(flatDir.x, flatDir.z) * Mathf.Rad2Deg;

            // Compare against the last yaw we actually applied (not the current noisy rotation)
            float deltaYawFromLast = Mathf.DeltaAngle(_lastAppliedYaw, targetYaw);
            if (Mathf.Abs(deltaYawFromLast) < yawThresholdDegrees)
            {
                // Below threshold: ignore to minimize shaking
                return;
            }

            // Snap directly to the new target yaw
            var euler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(euler.x, targetYaw, euler.z);

            // Remember the last applied yaw to enforce threshold on future updates
            _lastAppliedYaw = targetYaw;
        }

        public void RotateToOld(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}