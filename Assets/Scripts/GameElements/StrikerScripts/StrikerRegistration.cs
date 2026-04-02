using com.VisionXR.ModelClasses;
using UnityEngine;

public class StrikerRegistration : MonoBehaviour
{
   public StrikerDataSO strikerData;

    private void OnEnable()
    {
        strikerData.Register(this.gameObject);
    }

    private void OnDisable()
    {
        strikerData.Unregister();
    }

}
