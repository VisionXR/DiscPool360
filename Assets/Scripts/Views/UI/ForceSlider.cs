using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using com.VisionXR.ModelClasses;

[RequireComponent(typeof(Slider))]
public class ForceSlider : MonoBehaviour, IPointerUpHandler
{
    [Header("Scriptable Objects")]
    public InputDataSO inputData; // Reference to your ScriptableObject for input data
    public AnimationCurve StrikeCurve;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 1. Capture the force (the current value of the slider)
        float releaseForce = slider.value;

        if (releaseForce > 0)
        {
            FireSlingshot(releaseForce);
        }

        // 2. Reset the slider to zero
        slider.value = 0;
    }

    private void FireSlingshot(float force)
    {
        force = force * force;

        inputData.FireStrike(force);
        // Trigger your game logic here!
        // Example: myPlayer.Launch(force);
    }

    public void SetForce(float force)
    {
        force = force * force;
        inputData.StrikerForceChanged(force);
    }
}