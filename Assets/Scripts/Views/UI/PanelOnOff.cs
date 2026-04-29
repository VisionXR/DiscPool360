using com.VisionXR.ModelClasses;
using System.Collections;
using UnityEngine;

public class PanelOnOff : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    

    public void TurnOffPanel()
    {
        if (gameObject.activeInHierarchy)
        {
            
            StartCoroutine(AnimateScaleOff());
        }
    }

    public void TurnOnPanel()
    {
      
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        StartCoroutine(AnimateScaleOn());
        
    }

    private IEnumerator AnimateScaleOff()
    {
       

        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsed < uiData.disableTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / uiData.disableTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
     
    }

    private IEnumerator AnimateScaleOn()
    {
        yield return new WaitForSeconds(uiData.disableTime);

        float elapsed = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        while (elapsed < uiData.disableTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / uiData.disableTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }


}
