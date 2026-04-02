using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [Header("Scritable Objects")]
    public AppPropertiesDataSO appPropertiesData;

    [Header("UI Elements")]
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image HoverImage;
    

    // local variables
    private bool isHovering = false;


    public void OnPointerEnter(PointerEventData eventData)
    {

        if (!isHovering && BackgroundImage.gameObject.GetComponent<UIGradient>().enabled == false)
        {
            isHovering = true;
            HoverImage.color = appPropertiesData.HoverColor;
            appPropertiesData.StartVibration();

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        if (isHovering)
        {
            isHovering = false;
            HoverImage.color = appPropertiesData.IdleColor;
            
        }    
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
       
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }
}
