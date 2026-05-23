using System;
using com.VisionXR.HelperClasses;
using UnityEngine;

[CreateAssetMenu(fileName = "DestinationSO", menuName = "ScriptableObjects/DestinationSO")]
public class DestinationSO : ScriptableObject
{
    // variables
    public Destination currentDestination;


    // Actions

    public Action<Destination,Action,Action<string>> ConnectToDestinationEvent;
    public Action ClearDestinationEvent;


    // Methods

    private void OnEnable()
    {
        currentDestination = null;
    }

    public void SetDestination(Destination destination)
    {
        currentDestination = destination;
    }
    public void ConnectToDestination(Destination destination,Action OnDestinationSuccess,Action<string> OnDestinationFailed)
    {
        currentDestination = destination;
        ConnectToDestinationEvent?.Invoke(destination,OnDestinationSuccess,OnDestinationFailed);
    }

    public void ClearDestination()
    {
        currentDestination = null;
        ClearDestinationEvent?.Invoke();
    }


}


