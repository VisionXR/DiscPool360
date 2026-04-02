using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scritable Objects")]
    public AppPropertiesDataSO appPropertiesData;  

    // local variables
    public bool isHovering = false;


    void OnDisable()
    {
        if (isHovering)
        {
            isHovering = false;
     
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        if (!isHovering)
        {
            isHovering = true;
            Debug.Log("Is Hovering");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        if (isHovering)
        {
            isHovering = false;
            Debug.Log("stopped");
        }
       
    }


}
