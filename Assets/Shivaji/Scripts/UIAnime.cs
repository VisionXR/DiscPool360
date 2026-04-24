using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIAnime : MonoBehaviour
{
    public enum AnimationType
    {
        None,
        Slide,
        Fade,
        Scale,
        Rotate,
        SlideFade,
        SlideScale,
        FadeScale,
        SlideFadeScale
    }

    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        EaseOutBack,
        EaseInBack
    }

    public enum PlayMode
    {
        Show,
        Hide
    }

    [Header("References")]
    [SerializeField] private RectTransform target;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation Setup")]
    [SerializeField] private AnimationType animationType = AnimationType.SlideFadeScale;
    [SerializeField] private Direction direction = Direction.Right;
    [SerializeField] private EaseType easeType = EaseType.EaseOut;
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private float delay;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Auto Play")]
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private PlayMode playOnEnableMode = PlayMode.Show;

    [Header("Slide")]
    [SerializeField] private float slideDistance = 300f;

    [Header("Fade")]
    [SerializeField][Range(0f, 1f)] private float hiddenAlpha = 0f;
    [SerializeField][Range(0f, 1f)] private float shownAlpha = 1f;

    [Header("Scale")]
    [SerializeField] private Vector3 hiddenScale = new Vector3(0.8f, 0.8f, 1f);
    [SerializeField] private Vector3 shownScale = Vector3.one;

    [Header("Rotate")]
    [SerializeField] private float hiddenRotationZ = -20f;
    [SerializeField] private float shownRotationZ = 0f;

    [Header("Behaviour")]
    [SerializeField] private bool disableRaycastWhileAnimating = true;
    [SerializeField] private bool disableObjectAfterHide;

    private Coroutine currentRoutine;
    private Vector2 shownAnchoredPos;
    private Vector2 hiddenAnchoredPos;
    private bool initialized;

    private void Reset()
    {
        target = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        Initialize();

        if (playOnEnable)
        {
            if (playOnEnableMode == PlayMode.Show)
            {
                PlayShow();
            }
            else
            {
                PlayHide();
            }
        }
    }

    private void Initialize()
    {
        if (initialized)
        {
            return;
        }

        if (target == null)
        {
            target = GetComponent<RectTransform>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        shownAnchoredPos = target.anchoredPosition;
        hiddenAnchoredPos = shownAnchoredPos + GetDirectionOffset();

        initialized = true;
    }

    private Vector2 GetDirectionOffset()
    {
        switch (direction)
        {
            case Direction.Left:
                return new Vector2(-slideDistance, 0f);

            case Direction.Right:
                return new Vector2(slideDistance, 0f);

            case Direction.Up:
                return new Vector2(0f, slideDistance);

            case Direction.Down:
                return new Vector2(0f, -slideDistance);

            default:
                return Vector2.zero;
        }
    }

    public void RefreshShownPosition()
    {
        Initialize();
        shownAnchoredPos = target.anchoredPosition;
        hiddenAnchoredPos = shownAnchoredPos + GetDirectionOffset();
    }

    public void PlayShow()
    {
        Initialize();
        gameObject.SetActive(true);
        StartAnimation(true);
    }

    public void PlayHide()
    {
        Initialize();
        StartAnimation(false);
    }

    public void Play(bool show)
    {
        if (show)
        {
            PlayShow();
        }
        else
        {
            PlayHide();
        }
    }

    public void StopAnimation()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }

        SetInteractableState(true);
    }

    public void SetToShownState()
    {
        Initialize();
        StopAnimation();

        target.anchoredPosition = shownAnchoredPos;
        target.localScale = shownScale;
        target.localRotation = Quaternion.Euler(0f, 0f, shownRotationZ);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = shownAlpha;
        }

        SetInteractableState(true);
        gameObject.SetActive(true);
    }

    public void SetToHiddenState()
    {
        Initialize();
        StopAnimation();

        target.anchoredPosition = hiddenAnchoredPos;
        target.localScale = hiddenScale;
        target.localRotation = Quaternion.Euler(0f, 0f, hiddenRotationZ);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = hiddenAlpha;
        }

        SetInteractableState(false);
    }

    private void StartAnimation(bool show)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(AnimateRoutine(show));
    }

    private IEnumerator AnimateRoutine(bool show)
    {
        if (delay > 0f)
        {
            if (useUnscaledTime)
            {
                float elapsedDelay = 0f;
                while (elapsedDelay < delay)
                {
                    elapsedDelay += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
        }

        SetInteractableState(false);

        Vector2 fromPos = target.anchoredPosition;
        Vector2 toPos = target.anchoredPosition;

        Vector3 fromScale = target.localScale;
        Vector3 toScale = target.localScale;

        float fromAlpha = canvasGroup != null ? canvasGroup.alpha : 1f;
        float toAlpha = fromAlpha;

        float fromRot = target.localEulerAngles.z;
        float toRot = fromRot;

        if (UsesSlide())
        {
            fromPos = show ? hiddenAnchoredPos : shownAnchoredPos;
            toPos = show ? shownAnchoredPos : hiddenAnchoredPos;
            target.anchoredPosition = fromPos;
        }

        if (UsesScale())
        {
            fromScale = show ? hiddenScale : shownScale;
            toScale = show ? shownScale : hiddenScale;
            target.localScale = fromScale;
        }

        if (UsesFade() && canvasGroup != null)
        {
            fromAlpha = show ? hiddenAlpha : shownAlpha;
            toAlpha = show ? shownAlpha : hiddenAlpha;
            canvasGroup.alpha = fromAlpha;
        }

        if (UsesRotate())
        {
            fromRot = show ? hiddenRotationZ : shownRotationZ;
            toRot = show ? shownRotationZ : hiddenRotationZ;
            target.localRotation = Quaternion.Euler(0f, 0f, fromRot);
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = EvaluateEase(t, easeType);

            if (UsesSlide())
            {
                target.anchoredPosition = Vector2.LerpUnclamped(fromPos, toPos, easedT);
            }

            if (UsesScale())
            {
                target.localScale = Vector3.LerpUnclamped(fromScale, toScale, easedT);
            }

            if (UsesFade() && canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.LerpUnclamped(fromAlpha, toAlpha, easedT);
            }

            if (UsesRotate())
            {
                float z = Mathf.LerpUnclamped(fromRot, toRot, easedT);
                target.localRotation = Quaternion.Euler(0f, 0f, z);
            }

            yield return null;
        }

        if (UsesSlide())
        {
            target.anchoredPosition = toPos;
        }

        if (UsesScale())
        {
            target.localScale = toScale;
        }

        if (UsesFade() && canvasGroup != null)
        {
            canvasGroup.alpha = toAlpha;
        }

        if (UsesRotate())
        {
            target.localRotation = Quaternion.Euler(0f, 0f, toRot);
        }

        SetInteractableState(show);

        currentRoutine = null;

        if (!show && disableObjectAfterHide)
        {
            gameObject.SetActive(false);
        }
    }

    private bool UsesSlide()
    {
        return animationType == AnimationType.Slide ||
               animationType == AnimationType.SlideFade ||
               animationType == AnimationType.SlideScale ||
               animationType == AnimationType.SlideFadeScale;
    }

    private bool UsesFade()
    {
        return animationType == AnimationType.Fade ||
               animationType == AnimationType.SlideFade ||
               animationType == AnimationType.FadeScale ||
               animationType == AnimationType.SlideFadeScale;
    }

    private bool UsesScale()
    {
        return animationType == AnimationType.Scale ||
               animationType == AnimationType.SlideScale ||
               animationType == AnimationType.FadeScale ||
               animationType == AnimationType.SlideFadeScale;
    }

    private bool UsesRotate()
    {
        return animationType == AnimationType.Rotate;
    }

    private void SetInteractableState(bool state)
    {
        if (canvasGroup == null)
        {
            return;
        }

        if (disableRaycastWhileAnimating)
        {
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }

    private float EvaluateEase(float t, EaseType selectedEase)
    {
        switch (selectedEase)
        {
            case EaseType.Linear:
                return t;

            case EaseType.EaseIn:
                return t * t * t;

            case EaseType.EaseOut:
                return 1f - Mathf.Pow(1f - t, 3f);

            case EaseType.EaseInOut:
                return t < 0.5f
                    ? 4f * t * t * t
                    : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;

            case EaseType.EaseOutBack:
                {
                    float c1 = 1.70158f;
                    float c3 = c1 + 1f;
                    return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
                }

            case EaseType.EaseInBack:
                {
                    float c1 = 1.70158f;
                    float c3 = c1 + 1f;
                    return c3 * t * t * t - c1 * t * t;
                }

            default:
                return t;
        }
    }
}