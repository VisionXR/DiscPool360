using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class PlayerMovementInput : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public InputDataSO inputData;
        public UserDataSO userData;

        [Header("Local Data")]
        public AudioSource swipeAudio;
        public int delta = 20;
        [Header("Controller Thumbstick")]
        public float thumbThreshold = 0.6f;

        //local
        private bool isHandTrackingActive;

        //public void LeftGesture(OVRHand.MicrogestureType gesture)
        //{

        //    if (userData.myDominantHand == DominantHand.Right)
        //    {

        //        swipeAudio.Play();
        //        switch (gesture)
        //        {
        //            case OVRHand.MicrogestureType.SwipeLeft:
        //                inputData.MovePlayerX(-delta);
        //                break;
        //            case OVRHand.MicrogestureType.SwipeRight:
        //                inputData.MovePlayerX(delta);
        //                break;
        //            case OVRHand.MicrogestureType.SwipeForward:
        //                inputData.MovePlayerY(delta);
        //                break;
        //            case OVRHand.MicrogestureType.SwipeBackward:
        //                inputData.MovePlayerY(-delta);
        //                break;
        //            case OVRHand.MicrogestureType.ThumbTap:
        //                //Debug.Log("Right Thumb Tap Detected");
        //                break;
        //        }
        //    }

        //}

        //public void RightGesture(OVRHand.MicrogestureType gesture)
        //{

        //    if (userData.myDominantHand == DominantHand.Left)
        //    {
        //        swipeAudio.Play();
        //        switch (gesture)
        //        {
        //            case OVRHand.MicrogestureType.SwipeLeft:
        //                inputData.MovePlayerX(-delta);
        //                break;
        //            case OVRHand.MicrogestureType.SwipeRight:
        //                inputData.MovePlayerX(delta);
        //                break;
        //            case OVRHand.MicrogestureType.SwipeForward:
        //                inputData.MovePlayerY(delta);
        //                break;
        //            case OVRHand.MicrogestureType.SwipeBackward:
        //                inputData.MovePlayerY(-delta);
        //                break;
        //            case OVRHand.MicrogestureType.ThumbTap:
        //                //Debug.Log("Right Thumb Tap Detected");
        //                break;
        //        }
        //    }

        //}

     
    }
}
