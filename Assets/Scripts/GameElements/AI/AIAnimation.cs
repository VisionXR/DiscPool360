using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    [Header("Data References")]
    public AIDataSO aiData;
    public Animator animator;
    public Animator rightAnimator;
    public Animator leftAnimator;
    

    [Header("Hand Transforms")]
    public GameObject rightHand;
    public GameObject leftHand;

    public Transform leftHandOriginalParent;
    public Transform leftHandInitTransform;
    public Transform leftHandFinalTransform;

    public Transform rightHandInitTransform;
    public Transform rightHandFinalTransform;


    // internal
    private Coroutine _rightHandMoveRoutine;
    private Coroutine _leftHandMoveRoutine;

    private void OnEnable()
    {
        aiData.PlayAnimationEvent += PlayAnimation;
        aiData.PlayHandAnimationEvent += PlayHandAnimation;
        aiData.PlayLeftHandAnimationEvent += PlayLeftHandAnimation;


        aiData.StartRightHandMoveEvent += StartRightHandMove;
        aiData.StartRightHandRotationEvent += StartRightHandRotation;

        aiData.StartLeftHandRotationEvent += StartLeftHandRotation;
    }

    private void OnDisable()
    {
        aiData.PlayAnimationEvent -= PlayAnimation;
        aiData.PlayHandAnimationEvent -= PlayHandAnimation;
        aiData.PlayLeftHandAnimationEvent -= PlayLeftHandAnimation;


        aiData.StartRightHandMoveEvent -= StartRightHandMove;
        aiData.StartRightHandRotationEvent -= StartRightHandRotation;

        aiData.StartLeftHandRotationEvent -= StartLeftHandRotation;
    }

    private void StartLeftHandRotation(bool isForward)
    {
        if (leftHand == null || leftHandInitTransform == null || leftHandOriginalParent == null)
            return;

        if (_leftHandMoveRoutine != null)
            StopCoroutine(_leftHandMoveRoutine);

        if (isForward)
        {
            // Find nearest handle
            GameObject[] handles = GameObject.FindGameObjectsWithTag("Handle");
            if (handles == null || handles.Length == 0)
                return;

            Transform nearest = null;
            float bestSqr = float.MaxValue;
            Vector3 handPos = leftHand.transform.position;

            for (int i = 0; i < handles.Length; i++)
            {
                var h = handles[i];
                if (h == null) continue;
                float d = (h.transform.position - handPos).sqrMagnitude;
                if (d < bestSqr)
                {
                    bestSqr = d;
                    nearest = h.transform;
                }
            }

            if (nearest == null)
                return;

            // Move to handle (world space), then parent under handle
            _leftHandMoveRoutine = StartCoroutine(MoveLeftHandToTarget(nearest.position - transform.forward*0.15f, leftHandFinalTransform.rotation, () =>
            {
                leftHand.transform.SetParent(nearest, worldPositionStays: true);
                
            }));
        }
        else
        {
            leftHand.transform.SetParent(leftHandOriginalParent, worldPositionStays: true);
            leftHand.transform.localScale = Vector3.one;
            // Move back to init, then restore original parent
            _leftHandMoveRoutine = StartCoroutine(MoveLeftHandToTarget(leftHandInitTransform.position, leftHandInitTransform.rotation, () =>
            {
               
            }));
        }
    }

    private void StartRightHandRotation(Vector3 direction)
    {
        Vector3 up = Vector3.Cross(Vector3.up, direction);
        rightHand.transform.rotation = Quaternion.LookRotation(-direction, up);
    }

    private void PlayHandAnimation(string triggerName, bool state)
    {
        if (animator != null)
        {
          
            animator.SetBool(triggerName, state);
            rightAnimator.SetBool(triggerName, state);
            leftAnimator.SetBool(triggerName, state);
        }
    }

    private void PlayLeftHandAnimation(string triggerName, bool state)
    {
        leftAnimator.SetBool(triggerName, state);
    }

    private void PlayAnimation(string triggerName)
    {
        if (animator != null)
        {
         
            animator.SetTrigger(triggerName);
            rightAnimator.SetTrigger(triggerName);
            leftAnimator.SetTrigger(triggerName);
        }
    }

    // Public starter to move right hand. forward=true: init->final, forward=false: final->init
    public void StartRightHandMove(bool forward)
    {
        if (_rightHandMoveRoutine != null)
            StopCoroutine(_rightHandMoveRoutine);

        _rightHandMoveRoutine = StartCoroutine(MoveRightHandCoroutine(forward));
    }

    // Moves the right hand between init and final in 1 second (position + rotation, world space)
    private IEnumerator MoveRightHandCoroutine(bool forward)
    {
        if (rightHand == null || rightHandInitTransform == null || rightHandFinalTransform == null)
            yield break;

      

        Transform from = forward ? rightHandInitTransform : rightHandFinalTransform;
        Transform to = forward ? rightHandFinalTransform : rightHandInitTransform;

        Vector3 startPos = from.localPosition;
        Quaternion startRot = from.localRotation;
        Vector3 endPos = to.localPosition;
        Quaternion endRot = to.localRotation;


        const float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            rightHand.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            rightHand.transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveLeftHandToTarget(Vector3 targetPos, Quaternion targetRot, System.Action onComplete)
    {
        const float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startPos = leftHand.transform.position;
        Quaternion startRot = leftHand.transform.rotation;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            leftHand.transform.position = Vector3.Lerp(startPos, targetPos, t);
            leftHand.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        leftHand.transform.SetPositionAndRotation(targetPos, targetRot);
        onComplete?.Invoke();
        _leftHandMoveRoutine = null;
    }
}
