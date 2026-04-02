using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class AppPropertiesManager : MonoBehaviour
{
    [Header("Controller Settings")]
    public AppPropertiesDataSO appPropertiesData;

    // local variables
    private bool isRight = false;
    private bool isLeft = false;
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }

    public void StartVibration()
    {
        StartCoroutine(PlayHapticVibrationCoroutine());
    }

    // Summary: Start haptic vibration for a given duration
    public IEnumerator PlayHapticVibrationCoroutine()
    {
        

        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + appPropertiesData.vibrationDuration)
        {
            yield return null;
        }

        StopVibration();
    }

    public void StopVibration()
    {
       
    }

    public void LeftHandHovered()
    {
        isLeft = true;
    }

    public void RightHandHovered()
    {
        isRight = true;
    }

    public void LeftHandUnHovered()
    {
        isLeft = false;
    }

    public void RightHandUnUnHovered()
    {
        isRight = false;
    }
}
