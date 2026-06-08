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
    public AppPropertiesDataSO appPropertiesData;
    public PlayerDataSO playerData;
  

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

 

    public void Fire(float force)
    {
      
        strikerArrow.TurnOffArrow();
        strikerData.strikeForce = force;
        strikerData.strikerDir = transform.forward;
        strikerRigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
        strikerData.StrikerStarted();


        if (playerData.IsMyTurn())
        {
            appPropertiesData.StartStrikingVibration();
        }

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


            if (playerData.IsMyTurn())
            {
                appPropertiesData.StartStrikingVibration();
            }

            if (WaitRoutine == null)
            {
                WaitRoutine = StartCoroutine(WaituntilStrikeFinished());
            }
        }

    }

    public void SetStrikerForce(float normalizedValue)
    {
       
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

        if (playerData.IsMyTurn())
        {
            appPropertiesData.StartStrikingVibration();
        }
    }

    private IEnumerator WaituntilStrikeFinished()
    {

        float elaspsedTime = 0;

      
        while (!strikerRigidbody.IsSleeping() && elaspsedTime < 6)
        {
            // Yielding WaitForFixedUpdate ensures we check sync'd with the physics engine, 
            // completely bypassing frame rate variance.
            yield return new WaitForFixedUpdate();
            elaspsedTime += Time.fixedDeltaTime;
        }

        // 2. Force it to a complete stop to prevent micro-drifting
        strikerRigidbody.linearVelocity = Vector3.zero;
        strikerRigidbody.angularVelocity = Vector3.zero;

        // 3. Reduced the wait time (Adjust 6f to something lower like 0.5f or 1f if 6s was a bug)
        yield return new WaitForSeconds(3.0f);

        // 4. Clean up and trigger next turn
        strikerData.StrikerStopped();
        strikerArrow.ChangeColorOfArrow(0);
        WaitRoutine = null;
    }

    public void TurnOffArrow()
    {
        strikerArrow.TurnOffArrow();
    }
   
}
