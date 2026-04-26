using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class CamPositionManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public StrikerDataSO strikerData;
        public InputDataSO inputData;
        public TableDataSO tableData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;
        public BoardDataSO boardData;


        [Header("Game Objects")]
        public GameObject cameraRig;
        public LineRenderer lineRenderer;


        [Header("Camera Movement Settings")]
        public float MovementSpeed = 0.005f;
        public float SwipeAzimuthSensitivity = 2f; // Controls how fast horizontal swipe rotates
        public float SwipePolarSensitivity = 2f; // Controls how fast vertical swipe rotates

        [Header("Zoom Settings")]
        public float ZoomSensitivity = 1f;
        public float MinDistanceCutoff = 0.4f; // r1 - Minimum distance from striker
        public float MaxDistanceCutoff = 2f; // r2 - Maximum distance from striker
        public float CameraFollowSmoothing = 20f; // Higher = snappier response

        [Header("Polar Angle Constraints")]
        public float MinPolarAngle = 15f; // Prevent camera going too low (degrees)
        public float MaxPolarAngle = 45f; // Prevent camera going too high (degrees)


        [Header("Azimuth Angle Constraints")]
        public float MinAzimuthAngle = 135f; // Prevent camera going too low (degrees)
        public float MaxAzimuthAngle = 225f; // Prevent camera going too high (degrees)

        // Spherical coordinates
        private float _azimuth = 180f; // Horizontal rotation (degrees)
        private float _polarAngle = 45f; // Vertical rotation (degrees, 0 = top, 90 = horizontal, 180 = bottom)
        private float _radius = 1f; // Distance from striker

        // Target values for smooth interpolation
        public float _targetAzimuth;
        public float _targetPolarAngle;
        public float _targetRadius;

        private Vector3 _strikerPos;
        private Transform _playerTransform;
  


        private void OnEnable()
        {
            uiData.HomeEvent += ResetCamPosition;
            inputData.ZoomChangedEvent += ApplyZoom;
            inputData.ZoomStartedEvent += StartZoom;

            inputData.HorizontalSwipedEvent += HorizontalSwiped;
            inputData.VerticalSwipedEvent += VerticalSwiped;
        }

        private void OnDisable()
        {
            uiData.HomeEvent -= ResetCamPosition;
            inputData.ZoomChangedEvent -= ApplyZoom;
            inputData.ZoomStartedEvent -= StartZoom;

            inputData.HorizontalSwipedEvent -= HorizontalSwiped;
            inputData.VerticalSwipedEvent -= VerticalSwiped;
        }

      
        private void ResetCamPosition()
        {
            tableData.SetTableRotation(1);

        }

        private void StartZoom()
        {
            if (strikerData.currentStriker == null)
            {
               
                return;
            }

   
            _playerTransform = tableData.GetCamTransform(playerData.GetMainPlayer().playerProperties.myId);
            _strikerPos = strikerData.currentStriker.transform.position;

            // Initialize spherical coordinates from current camera position
            InitializeSphericalCoordinates();

            // Set target to current values (no initial jump)
            _targetAzimuth = _azimuth;
            _targetPolarAngle = _polarAngle;
            _targetRadius = _radius;
        }

        private void InitializeSphericalCoordinates()
        {
            // Convert current camera position to spherical coordinates relative to striker
            Vector3 cameraOffset = cameraRig.transform.position - _strikerPos;
            
            _radius = cameraOffset.magnitude;
            _radius = Mathf.Clamp(_radius, MinDistanceCutoff, MaxDistanceCutoff);

            // Calculate azimuth (rotation around Y-axis)
            _azimuth = Mathf.Atan2(cameraOffset.x, cameraOffset.z) * Mathf.Rad2Deg;
            
            // Calculate polar angle (angle from top)
            _polarAngle = Mathf.Acos(Mathf.Clamp01(cameraOffset.y / _radius)) * Mathf.Rad2Deg;
            _polarAngle = Mathf.Clamp(_polarAngle, MinPolarAngle, MaxPolarAngle);
        }

        private void ApplyZoom(float delta)
        {
            
            // Zoom adjusts the radius
            _targetRadius += delta * ZoomSensitivity;
            _targetRadius = Mathf.Clamp(_targetRadius, MinDistanceCutoff, MaxDistanceCutoff);
        }

        private void VerticalSwiped(float delta)
        {
            // Subtracting delta often feels more natural for "pulling" the camera up/down
            _targetPolarAngle -= delta * SwipePolarSensitivity;
            _targetPolarAngle = Mathf.Clamp(_targetPolarAngle, MinPolarAngle, MaxPolarAngle);
        }

        private void HorizontalSwiped(float delta)
        {
            // Adding delta rotates the camera around the Y axis
            _targetAzimuth += delta * SwipeAzimuthSensitivity;
            // We don't necessarily need to Clamp/Repeat targetAzimuth, LerpAngle handles it

            _targetAzimuth = Mathf.Clamp(_targetAzimuth, MinAzimuthAngle, MaxAzimuthAngle);
        }

        private void LateUpdate()
        {
            if (strikerData.currentStriker == null)
            {

                _strikerPos = boardData.Board.transform.position;
            }
            else
            {
                _strikerPos = strikerData.currentStriker.transform.position;
            }

            // 1. Smoothly interpolate values
            // Use LerpAngle for Azimuth to prevent the "360-degree flip" bug
            _azimuth = Mathf.LerpAngle(_azimuth, _targetAzimuth, Time.deltaTime * CameraFollowSmoothing);
            _polarAngle = Mathf.Lerp(_polarAngle, _targetPolarAngle, Time.deltaTime * CameraFollowSmoothing);
            _radius = Mathf.Lerp(_radius, _targetRadius, Time.deltaTime * CameraFollowSmoothing);

            // 2. Update striker position

            _azimuth = Mathf.Clamp(_azimuth, MinAzimuthAngle, MaxAzimuthAngle);
            _polarAngle = Mathf.Clamp(_polarAngle, MinPolarAngle, MaxPolarAngle);
            _radius = Mathf.Clamp(_radius, MinDistanceCutoff, MaxDistanceCutoff);


            // 3. Calculate position on the sphere
            Vector3 cameraPosition = SphericalToCartesian(_strikerPos, _radius, _azimuth, _polarAngle);

            // 4. Set position directly or with a very high smoothing
            // Note: Since we are already smoothing the coordinates above, 
            // setting the position directly prevents "double-smoothing" lag.
            cameraRig.transform.position = cameraPosition;

            // 5. Always look at the center of the sphere (the striker)
            cameraRig.transform.LookAt(_strikerPos);

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, _strikerPos);
            lineRenderer.SetPosition(1, cameraRig.transform.position);
        }

        /// <summary>
        /// Converts spherical coordinates to world Cartesian coordinates
        /// </summary>
        /// <param name="center">Center point (striker position)</param>
        /// <param name="radius">Distance from center</param>
        /// <param name="azimuth">Horizontal rotation in degrees (0 = +Z direction)</param>
        /// <param name="polarAngle">Vertical rotation in degrees (0 = +Y, 90 = horizontal, 180 = -Y)</param>
        private Vector3 SphericalToCartesian(Vector3 center, float radius, float azimuth, float polarAngle)
        {
            float azimuthRad = azimuth * Mathf.Deg2Rad;
            float polarRad = polarAngle * Mathf.Deg2Rad;

            float x = radius * Mathf.Sin(polarRad) * Mathf.Sin(azimuthRad);
            float y = radius * Mathf.Cos(polarRad);
            float z = radius * Mathf.Sin(polarRad) * Mathf.Cos(azimuthRad);

            return center + new Vector3(x, y, z);
        }
    }
}
