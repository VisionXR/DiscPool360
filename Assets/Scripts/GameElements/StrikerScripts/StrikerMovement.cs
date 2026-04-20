using com.VisionXR.ModelClasses;
using System;
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

        /// <summary>
        /// Rotates the striker by a relative delta around the Y axis.
        /// Positive delta rotates clockwise, negative delta rotates anticlockwise.
        /// </summary>
        /// <param name="delta">The angle delta in degrees.</param>
        public void RotateRelative(float delta)
        {
            var currentEuler = transform.eulerAngles;
            float newY = currentEuler.y + delta;
            transform.eulerAngles = new Vector3(0, newY, 0);
            _lastAppliedYaw = newY;
        }

        /// <summary>
        /// Snap-rotate to the given direction's yaw only when the change exceeds the threshold.
        /// - If |delta| &gt;= threshold: snap directly to the target yaw.
        /// - Else: do nothing (ignore minor changes).
        /// Subsequent updates only occur when the new target deviates from the last applied yaw by the threshold again.
        /// </summary>
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