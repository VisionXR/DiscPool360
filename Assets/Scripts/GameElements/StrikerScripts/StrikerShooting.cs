using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using UnityEngine;

public class StrikerShooting : MonoBehaviour
{
    [Header("Scriptable Object)")]
    public StrikerDataSO strikerData;
    public InputDataSO inputData;
  

    [Header(" Local variables ")]
    public StrikerArrow strikerArrow;
    public Rigidbody strikerRigidbody;
    

    // actions
    public Action<float> StrikeForceChangedEvent;
    public Action StrikeForceStartedEvent;

    // variables
 
    public float cutOffValue = 0.1f;
    private float strikeForce = 2;
    private bool isFired = false;  
    private float startTime;
    private Coroutine FireRoutine;
    private Coroutine WaitRoutine;

    public void StartStrike(float strength)
    {
        if (strength > 0.5f && !isFired)
        {
            isFired = true;
          
            strikerData.StrikeForceStarted();
        }
    }


    public void EndStrike(float strength)
    {
        if (strength < 0.1f && isFired)
        {

            isFired = false;
          
           
            strikerArrow.TurnOffArrow();
            strikerData.strikeForce = strikeForce;
            strikerData.strikerDir = transform.forward;

            strikerRigidbody.AddForce(transform.forward * strikeForce, ForceMode.VelocityChange);
            inputData.DisableInput();
            strikerData.StrikerStarted();


            if (WaitRoutine == null)
            {
                WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
            }

        }
    }

    public void Fire(float force)
    {
      
        strikerArrow.TurnOffArrow();
        strikerData.strikeForce = force;
        strikerData.strikerDir = transform.forward;
        strikerRigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
        strikerData.StrikerStarted();
        if (WaitRoutine == null)
        {
            WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
        }
    }

    public void FireStriker(float val)  
    {
        if (val > cutOffValue)
        {
            
            SetStrikerForce(val);
            strikerRigidbody.AddForce(transform.forward * strikerData.strikeForce, ForceMode.VelocityChange);
            strikerData.StrikerStarted();
            strikerArrow.TurnOffArrow();
            if (WaitRoutine == null)
            {
                WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
            }
        }

    }

    public void SetStrikerForce(float normalizedValue)
    {
        // Map the normalized value to the desired range
    //    normalizedValue = StrikeCurve.Evaluate(normalizedValue);
        float range = strikerData.forceUpperLimit - strikerData.forceLowerLimit;
        strikeForce = strikerData.forceLowerLimit + (normalizedValue) * range;
        strikerArrow.ChangeColorOfArrow(normalizedValue);
        strikerData.strikeForce = strikeForce;
        strikerData.strikerDir = transform.forward;
        strikerData.StrikeForceChanged(strikeForce,transform.forward);
    }

    public void SetStrikerData(float force, Vector3 dir)
    {
        strikeForce = force;
        transform.forward = dir;
        float range = strikerData.forceUpperLimit - strikerData.forceLowerLimit;
        float val = Mathf.Abs(force)/range;
        strikerArrow.ChangeColorOfArrow(val);
        strikerData.strikeForce = force;
        strikerData.strikerDir = dir;
    }

    public void SetForceAndDir(float force, Vector3 dir)
    {
        strikeForce = force;
        transform.forward = dir;
        strikerRigidbody.AddForce(dir * force, ForceMode.VelocityChange);
    }

    private IEnumerator WaituntilStrikeFinished()
    {

        yield return new WaitUntil(() => strikerRigidbody.linearVelocity.magnitude < 0.005f);
        yield return new WaitForSeconds(5);
        strikerData.StrikerStopped();
        strikerArrow.ChangeColorOfArrow(0);
        WaitRoutine = null;

    }



    public void TurnOffArrow()
    {
        strikerArrow.TurnOffArrow();


    }

    private IEnumerator WaitAndChangeArrow()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            float timeSinceStart = Time.time - startTime;
            float period = 2f; // Time taken to complete one full cycle
            float t = timeSinceStart / period; // Normalized time between 0 and 1

            // Linearly interpolate between 0 and 1 and then back to 0
            float normalizedValue = Mathf.PingPong(t, 1f);

            strikerData.normalValue = normalizedValue;

            // Map the normalized value to the desired range
            float range = strikerData.forceUpperLimit - strikerData.forceLowerLimit;
            strikeForce = strikerData.forceLowerLimit + normalizedValue * range;
            strikerData.strikeForce = strikeForce;
            strikerArrow.ChangeColorOfArrow(normalizedValue);
        }
    }


   
}
