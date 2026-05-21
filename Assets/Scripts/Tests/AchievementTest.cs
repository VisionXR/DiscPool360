using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class AchievementTest : MonoBehaviour
{
    public AchievementsDataSO achievementsData;
    public string achievementName;
    public AchievementInfo achievementInfo;


    [Header("Key Bindings (New Input System)")]
    public Key GetKey = Key.G;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null)
            return;

        if ( kb[GetKey].wasPressedThisFrame)
        {
            achievementInfo = achievementsData.GetAchievementByName(achievementName);
        }
    }
}
