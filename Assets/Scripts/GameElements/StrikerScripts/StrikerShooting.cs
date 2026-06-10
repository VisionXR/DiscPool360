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
    public CoinDataSO coinData;
  

    [Header(" Striker variables ")]
    public StrikerArrow strikerArrow;
    public Rigidbody strikerRigidbody;
    public float cutOffValue = 0.1f;
    private float strikeForce = 2;

    // actions
    public Action<float> StrikeForceChangedEvent;
    public Action StrikeForceStartedEvent;

    // variables
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
            Debug.Log("force is " + strikerData.strikeForce);
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

        yield return new WaitForSeconds(0.5f);

        while (IsAnyObjectMoving())
        {
            Debug.Log("velocity is " + strikerRigidbody.linearVelocity.magnitude);
            // Yielding WaitForFixedUpdate ensures we check sync'd with the physics engine, 
            // completely bypassing frame rate variance.
            yield return new WaitForSeconds(0.5f);
          
        }

        // 2. Force it to a complete stop to prevent micro-drifting
        strikerRigidbody.linearVelocity = Vector3.zero;
        strikerRigidbody.angularVelocity = Vector3.zero;

        // 4. Clean up and trigger next turn
        strikerData.StrikerStopped();
        strikerArrow.ChangeColorOfArrow(0);
        WaitRoutine = null;
    }

    public void TurnOffArrow()
    {
        strikerArrow.TurnOffArrow();
    }

    public bool IsAnyObjectMoving()
    {
        
        if (strikerRigidbody.linearVelocity.magnitude > 0.005f)
        {
          
            return true;

        }

        foreach (var rb in coinData.AvailableCoinsInGame)
        {
            if (rb.linearVelocity.magnitude > 0.005f)
                return true;
        }

        return false;
    }

}
