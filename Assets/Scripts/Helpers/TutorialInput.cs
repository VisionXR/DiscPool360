using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using UnityEngine;


namespace com.VisionXR.GameElements
{
    public class TutorialInput : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public TutorialDataSO tutorialData;
        public BoardDataSO boardData;
        public StrikerDataSO strikerData;

        [Header("Game Objects")]
        public GameObject striker;
        public StrikerShooting strikerShooting;
        public StrikerMovement strikerMovement;


        // Foul
        [Header("Foul Variables")]
        public LineRenderer lineRenderer;


        private void OnEnable()
        {
            inputData.FireStrikeEvent += FireStriker;
            inputData.RotateStrikerAbsoluteEvent += RotateStriker;

            inputData.SwipedEvent += Swiped;
            inputData.StrikerForceChangedEvent += StrikerForceChanged;
        }


        private void OnDisable()
        {
            inputData.FireStrikeEvent -= FireStriker;
            inputData.RotateStrikerAbsoluteEvent -= RotateStriker;

            inputData.SwipedEvent -= Swiped;
            inputData.StrikerForceChangedEvent -= StrikerForceChanged;

        }

        private void Swiped(float velocity)
        {
            
            strikerMovement.RotateRelative(velocity);
        }

        private void StrikerForceChanged(float obj)
        {
            strikerShooting.SetStrikerForce(obj);
        }

        private void RotateStriker(float angle)
        {
            strikerMovement.RotateAbsolute(angle);
        }

        private void FireStriker(float val)
        {
          
            strikerShooting.FireStriker(val);
        }


    }
}
