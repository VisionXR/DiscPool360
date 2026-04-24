using UnityEngine;
using UnityEngine.InputSystem;

public class UITest : MonoBehaviour
{
    [Header("Assign Multiple UIAnim Components")]
    [SerializeField] private UIAnime[] anims;

    [Header("Key Bindings")]
    [SerializeField] private Key showKey = Key.S;
    [SerializeField] private Key hideKey = Key.H;
    [SerializeField] private Key toggleKey = Key.T;
    [SerializeField] private Key resetShownKey = Key.Digit1;
    [SerializeField] private Key resetHiddenKey = Key.Digit2;

    private bool isShown = true;

    private void Update()
    {
        if (anims == null || anims.Length == 0)
        {
            return;
        }

        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // SHOW ALL
        if (keyboard[showKey].wasPressedThisFrame)
        {
            foreach (var anim in anims)
            {
                if (anim != null)
                {
                    anim.PlayShow();
                }
            }
            isShown = true;
        }

        // HIDE ALL
        if (keyboard[hideKey].wasPressedThisFrame)
        {
            foreach (var anim in anims)
            {
                if (anim != null)
                {
                    anim.PlayHide();
                }
            }
            isShown = false;
        }

        // TOGGLE ALL
        if (keyboard[toggleKey].wasPressedThisFrame)
        {
            isShown = !isShown;

            foreach (var anim in anims)
            {
                if (anim != null)
                {
                    anim.Play(isShown);
                }
            }
        }

        // FORCE SHOWN STATE (NO ANIMATION)
        if (keyboard[resetShownKey].wasPressedThisFrame)
        {
            foreach (var anim in anims)
            {
                if (anim != null)
                {
                    anim.SetToShownState();
                }
            }
            isShown = true;
        }

        // FORCE HIDDEN STATE (NO ANIMATION)
        if (keyboard[resetHiddenKey].wasPressedThisFrame)
        {
            foreach (var anim in anims)
            {
                if (anim != null)
                {
                    anim.SetToHiddenState();
                }
            }
            isShown = false;
        }
    }
}