using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class StrikerRegistration : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public StrikerDataSO strikerData;
    public UserDataSO userData;

    [Header("Game Objects")]
    public GameObject guide;

    private void OnEnable()
    {
        strikerData.Register(this.gameObject);    
    }

    private void Update()
    {
        if (userData.myGuide == GuideType.NoGuide)
        {
            guide.SetActive(false);
        }
        else
        {
            if (!guide.activeInHierarchy)
            {
                guide.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        strikerData.Unregister();
    }

}
