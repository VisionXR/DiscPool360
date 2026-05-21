using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class AchievementTest : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementsData;
    public UIDataSO uiData;
    public DestinationSO destinationData;

    [Header("Local Objects")]
    public AchievementManager achievementManager;
    public string achievementName;
    public AchievementInfo achievementInfo;
    public Destination destination;


    [Header("Key Bindings (New Input System)")]
    public Key GetKey = Key.G;
    public Key SetAchievementKey = Key.A;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;

        if ( kb[GetKey].wasPressedThisFrame)
        {
            achievementInfo = achievementsData.GetAchievementByName(achievementName);
        }

        if( kb[SetAchievementKey].wasPressedThisFrame)
        {
            destinationData.SetDestination(destination);
            achievementManager.GameCompleted(1);
        }
    }
}
