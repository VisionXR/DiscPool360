using com.VisionXR.ModelClasses;
using UnityEngine;
using UnityEngine.UI;

public class ZoomInZoomOut : MonoBehaviour
{
    public Slider zoomSlider;
    public InputDataSO inputData;


    private void OnEnable()
    {
        if (zoomSlider != null)
        {
            zoomSlider.minValue = 0f;
            zoomSlider.maxValue = 1f;
            zoomSlider.value = 0.5f;
            zoomSlider.onValueChanged.AddListener(OnZoomSliderChanged);
        }
    }

    private void OnDisable()
    {
        if (zoomSlider != null)
        {
            zoomSlider.onValueChanged.RemoveListener(OnZoomSliderChanged);
        }
    }

    private void OnZoomSliderChanged(float value)
    {
        inputData.ZoomStarted();
        inputData.ZoomChanged(value);
    }
}
