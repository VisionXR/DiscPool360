using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.VisionXR.ModelClasses;
using com.VisionXR.GameElements;

[RequireComponent(typeof(Slider))]
public class AimSlider : MonoBehaviour, IDragHandler
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData;
    public PlayerDataSO playerData;
    public GameObject aimArrow;

    // local
    private RectTransform sliderRectTransform;
    private Player mp;

    private void Awake()
    {
       
        sliderRectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (aimArrow != null)
            aimArrow.transform.rotation = Quaternion.identity;

        mp = playerData.GetMainPlayer();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 1. Get the center of the slider in Screen Space
        Vector2 sliderCenterScreen = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, sliderRectTransform.position);

        // 2. Calculate the vector from the center of the slider to the mouse/touch position
        Vector2 direction = eventData.position - sliderCenterScreen;

        if (direction.magnitude > 0.1f) // Avoid jitter when very close to center
        {
            // 3. Calculate angle
            // Atan2 returns the angle in radians. Rad2Deg converts to 0-360.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            if (mp != null)
            {
                if(mp.playerProperties.myId == 2)
                {
                    aimArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                    inputData.RotateStrikerAbsolute(-(angle+180));
                    return;
                }


                aimArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
                inputData.RotateStrikerAbsolute(-angle);

            }
        }
    }


}