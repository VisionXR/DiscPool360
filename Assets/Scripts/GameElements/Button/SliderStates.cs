using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scritable Objects")]
    public AppPropertiesDataSO appPropertiesData;

    [Header("UI Elements")]
    [SerializeField] private Image Handle;
 
    

    // local variables
    private bool isHovering = false;
    // popout/bring-forward
    [Header("Popout")]
    [Tooltip("Scale multiplier applied to ActualImage on hover")]
    public float popScale = 1.08f;
    [Tooltip("Popout lerp duration in seconds")]
    public float popDuration = 0.12f;
    private Coroutine popRoutine = null;
    private Vector3 originalScale = Vector3.one;


    void OnDisable()
    {
        if (isHovering)
        {
            isHovering = false;
           
            if (popRoutine != null) { StopCoroutine(popRoutine); popRoutine = null; }
            // reset scale if needed
            if (Handle != null)
            {
                Handle.rectTransform.localScale = originalScale;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {

        if (!isHovering)
        {
            isHovering = true;
    
            appPropertiesData.StartVibration();

            // scale/pop the actual image slightly toward the user
            if (Handle != null)
            {
                if (originalScale == Vector3.one) originalScale = Handle.rectTransform.localScale;
                if (popRoutine != null) StopCoroutine(popRoutine);
                popRoutine = StartCoroutine(ScaleRoutine(Handle.rectTransform, originalScale * popScale, popDuration));
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
        if (isHovering)
        {
            isHovering = false;

            if (Handle != null)
            {
                if (popRoutine != null) StopCoroutine(popRoutine);
                popRoutine = StartCoroutine(ScaleRoutine(Handle.rectTransform, originalScale, popDuration));
            }
        }
       
    }

    private System.Collections.IEnumerator ScaleRoutine(RectTransform rt, Vector3 targetScale, float duration)
    {
        if (rt == null) yield break;
        Vector3 start = rt.localScale;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float f = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duration));
            rt.localScale = Vector3.Lerp(start, targetScale, f);
            yield return null;
        }
        rt.localScale = targetScale;
        popRoutine = null;
    }

}
