using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;


namespace com.VisionXR.Controllers
{
    public class KeyBoardInputManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public TableDataSO tableData;
        public GameObject rotationTestObject;
        public float rotationSpeed = 1f;

                            
        [Header("Key Bindings (New Input System)")]
        public Key LeftRotateStrikerKey = Key.LeftArrow;
        public Key RightRotateStrikerKey = Key.RightArrow;
        public Key StartStrikeKey = Key.Space;
        public Key EndStrikeKey = Key.E;

        public Key RotatePlatformKey = Key.R;
      


        private void Start()
        {
            if(Application.isEditor == false)
            {
               this.enabled = false;
            }
        }

        // Methods
        public void Update()
        {
            //if (inputData.isInputEnabled == false)
            //    return;

            var kb = Keyboard.current;
            if (kb == null)
                return;

            // Striker rotation (continuous)
            if (kb[LeftRotateStrikerKey].isPressed)
            {
                inputData.RotateStrikerRelative(-1f);
            }
            if (kb[RightRotateStrikerKey].isPressed)
            {
                inputData.RotateStrikerRelative(1f);
            }

            // Strike (pressed this frame)
            if (kb[StartStrikeKey].wasPressedThisFrame)
            {
                inputData.StartStrike();
            }

            if (kb[EndStrikeKey].wasPressedThisFrame)
            {
                inputData.EndStrike();
            }

            // Platform rotation (continuous)
            if (kb[RotatePlatformKey].wasPressedThisFrame)
            {
                Debug.Log("Rotate Platform Key Pressed");
                tableData.PlatformRotationStarted();
            }
            else if (kb[RotatePlatformKey].isPressed)
            {
                Debug.Log("Rotate Platform Key Held");
               // inputData.RotationPinchContinued(Vector3.one);
            }
            else if (kb[RotatePlatformKey].wasReleasedThisFrame)
            {
                Debug.Log("Rotate Platform Key Released");
                tableData.PlatformRotationEnded();
               // inputData.RotationPinchEnded();
            }
        }


    }

}
