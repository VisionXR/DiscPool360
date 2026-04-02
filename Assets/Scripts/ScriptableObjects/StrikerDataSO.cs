using System;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "StrikerDataSO", menuName = "ScriptableObjects/StrikerDataSO", order = 1)]
    public class StrikerDataSO : ScriptableObject
    {
        // Striker Data
        public GameObject currentStriker;
        public float period = 10.0f;
        public float forceUpperLimit = 5.0f;
        public float forceLowerLimit = 1.0f;
        public bool isFoul = false;
        public float strikeForce = 2.0f;
        public float normalValue = 0;
        public Vector3 strikerDir;

        // local
        private int pocketId = 1;

        // Events

        public Action<Transform> CreateStrikerEvent;
        public Action DestroyStrikerEvent;


        public Action StrikerStartedEvent;
        public Action StrikerStoppedEvent;

        public Action TurnOnArrowEvent;
        public Action TurnOffArrowEvent;

        public Action StrikeForceStartedEvent;
        public Action<float,Vector3> StrikeForceChangedEvent;
        public Action ResetStrikerEvent;

        public Action<int> StrikerPocketedEvent;
        public Action<GameObject> StrikerCollidedEvent;
        public Action<int> HandleFoulEvent;
        public Action FoulCompleteEvent;
        public Action<int> PlaceStrikerEvent;

        //Methods

        private void OnEnable()
        {
            // Reset data when the ScriptableObject is enabled
            isFoul = false;
        }

        public void Register(GameObject striker)
        {
            currentStriker = striker;
        }

        public void Unregister()
        {
            currentStriker = null;
        }   

        public void ResetStriker()
        {
            ResetStrikerEvent?.Invoke();
        }

        public void StrikeForceStarted()
        {
            StrikeForceStartedEvent?.Invoke();
        }


        public void CreateStriker(Transform strikerTransform)
        {
            CreateStrikerEvent?.Invoke(strikerTransform);
        }

        public void DestroyStriker()
        {
            DestroyStrikerEvent?.Invoke();
        }
        

        public void FoulComplete()
        {
            FoulCompleteEvent?.Invoke();
        }

        public void HandleFoul(int id)
        {
           
            HandleFoulEvent?.Invoke(id);
        }

        public void SetFoul(bool val)
        {
            isFoul = val;
        }

        public void StrikerStarted()
        {
            StrikerStartedEvent?.Invoke();
        }


        public void StrikeForceChanged(float force,Vector3 dir)
        {
            StrikeForceChangedEvent?.Invoke(force,dir);
        }
        public void StrikerStopped()
        {
            StrikerStoppedEvent?.Invoke();
        }

        public void StrikerPocketed(int id)
        {
            pocketId = id;
            StrikerPocketedEvent?.Invoke(id);
        }

        public void PlaceStriker()
        {
            PlaceStrikerEvent?.Invoke(pocketId);
        }

        public void StrikerCollided(GameObject collidedObject)
        {
            StrikerCollidedEvent?.Invoke(collidedObject);
        }

        public void TurnOnArrow()
        {
            TurnOnArrowEvent?.Invoke();
        }

        public void TurnOffArrow()
        {
            TurnOffArrowEvent?.Invoke();
        }


        public void TurnOnRigidBody()
        {  
            if (currentStriker != null)
            {
                Rigidbody rb = currentStriker.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }
        }

        public void TurnOffRigidBody()
        {
            if (currentStriker != null)
            {
                Rigidbody rb = currentStriker.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }

    }
}
