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
    public void ConnectToDestination(Destination destination,Action OnDestinationSuccess,Action<string> OnDestinationFailed)
    {
        ConnectToDestinationEvent?.Invoke(destination,OnDestinationSuccess,OnDestinationFailed);
    }

    public void ClearDestination()
    {
        currentDestination = null;
        ClearDestinationEvent?.Invoke();
    }


}



[Serializable]
public class Destination
{
    public ServerRegion region;
    public GameType gameType;
    public GameMode gameMode;
    public AIDifficulty aIDifficulty;
    public string lobbyName;
    public string roomName;
    public bool isJoinable;
    
    public ServerRegion GetRegion()
    {
        return Enum.TryParse(lobbyName, true, out ServerRegion serverRegion) ? serverRegion : ServerRegion.any;
    }

}
