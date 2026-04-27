using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public List<CamProperties> camPropertiesList; // List of camera properties for different boards
        public GameObject cameraRig;
        public LineRenderer lineRenderer;


        [Header("Camera Movement Settings")]
        public float MovementSpeed = 0.005f;
        public float SwipeAzimuthSensitivity = 2f; // Controls how fast horizontal swipe rotates
        public float SwipePolarSensitivity = 2f; // Controls how fast vertical swipe rotates

        [Header("Zoom Settings")]
        public float ZoomSensitivity = 1f;
        public float MinRadius = 0.4f; // r1 - Minimum distance from striker
        public float MaxRadius = 2f; // r2 - Maximum distance from striker
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
            SetCamProperties(1);
            uiData.HomeEvent += ResetCamPosition;
            inputData.ZoomChangedEvent += ApplyZoom;
            inputData.ZoomStartedEvent += StartZoom;

            inputData.HorizontalSwipedEvent += HorizontalSwiped;
            inputData.VerticalSwipedEvent += VerticalSwiped;

            tableData.SetCamRotationEvent += SetCamProperties;
        }

        private void OnDisable()
        {
            uiData.HomeEvent -= ResetCamPosition;
            inputData.ZoomChangedEvent -= ApplyZoom;
            inputData.ZoomStartedEvent -= StartZoom;

            inputData.HorizontalSwipedEvent -= HorizontalSwiped;
            inputData.VerticalSwipedEvent -= VerticalSwiped;

            tableData.SetCamRotationEvent -= SetCamProperties;
        }

      
        private void ResetCamPosition()
        {
            SetCamProperties(1);

        }

        public void SetCamProperties(int id)
        {
            id = id - 1;

            _targetRadius = camPropertiesList[id].Radius;
            _targetPolarAngle = camPropertiesList[id].PolarAngle;
            _targetAzimuth = camPropertiesList[id].Azimuth;
             MinAzimuthAngle = camPropertiesList[id].minAzimuthAngle;
             MaxAzimuthAngle = camPropertiesList[id].maxAzimuthAngle;
             MinPolarAngle = camPropertiesList[id].minPolarAngle;
             MaxPolarAngle = camPropertiesList[id].maxPolarAngle;
             MinRadius = camPropertiesList[id].minRadius;
             MaxRadius = camPropertiesList[id].maxRadius;
        }

        private void StartZoom()
        {
            _playerTransform = tableData.GetCamTransform(playerData.GetMainPlayer().playerProperties.myId);
        }

      

        private void ApplyZoom(float delta)
        {
            
            // Zoom adjusts the radius
            _targetRadius -= delta * ZoomSensitivity;

            _targetRadius = Mathf.Clamp(_targetRadius,MinRadius,MaxRadius);

            Debug.Log("Radius" + _targetRadius);

        }

        private void VerticalSwiped(float delta)
        {
            // Subtracting delta often feels more natural for "pulling" the camera up/down
            _targetPolarAngle += delta * SwipePolarSensitivity;

          

            _targetPolarAngle = Mathf.Clamp(_targetPolarAngle, MinPolarAngle, MaxPolarAngle);

            Debug.Log("Polar" + _targetPolarAngle);
        }

        private void HorizontalSwiped(float delta)
        {
            // Adding delta rotates the camera around the Y axis
           _targetAzimuth += delta * SwipeAzimuthSensitivity;

           
            // We don't necessarily need to Clamp/Repeat targetAzimuth, LerpAngle handles it

            _targetAzimuth = Mathf.Clamp(_targetAzimuth, MinAzimuthAngle, MaxAzimuthAngle);

            Debug.Log("Azimuth" + _targetAzimuth);
        }

        private void LateUpdate()
        {

             _strikerPos = boardData.Board.transform.position;
            
            // 1. Smoothly interpolate values
            // Use LerpAngle for Azimuth to prevent the "360-degree flip" bug
            _azimuth = Mathf.LerpAngle(_azimuth, _targetAzimuth, Time.deltaTime * CameraFollowSmoothing);
            _polarAngle = Mathf.Lerp(_polarAngle, _targetPolarAngle, Time.deltaTime * CameraFollowSmoothing);
            _radius = Mathf.Lerp(_radius, _targetRadius, Time.deltaTime * CameraFollowSmoothing);

            // 2. Update striker position

            _azimuth = Mathf.Clamp(_azimuth, MinAzimuthAngle, MaxAzimuthAngle);
            _polarAngle = Mathf.Clamp(_polarAngle, MinPolarAngle, MaxPolarAngle);
            _radius = Mathf.Clamp(_radius, MinRadius, MaxRadius);


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

    [Serializable]
    public class CamProperties
    {
        public float Azimuth;
        public float PolarAngle;
        public float Radius;


        public float minAzimuthAngle;
        public float maxAzimuthAngle;
        public float minPolarAngle;
        public float maxPolarAngle;
        public float minRadius;
        public float maxRadius;
    }
}
