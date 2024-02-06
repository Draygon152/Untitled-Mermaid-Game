using System;
using UnityEngine;

/// <summary>
///     Base class for management of all Menu objects that will live in their own scenes
/// </summary>
public class MenuBase : MonoBehaviour
{
    [Header("Canvas Groups")]
    [SerializeField] protected CanvasGroup mainCG = null;

    [Header("Fade Tween Delays")]
    [SerializeField] protected float fadeStartDelay = 0f;
    [SerializeField] protected float fadeDuration = 0.4f;

    // Flag for if the screen is occupied with a fade, prevents input conflicts
    protected bool isScreenBusy = false;
    protected bool isInitialized = false;



    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    ///     <para>
    ///         Handles Menu initialization upon scene loads and performs initial fade-in
    ///     </para>
    ///     
    ///     Add button listeners here
    /// </summary>
    public virtual void Init()
    {
        if (isInitialized)
        {
            Debug.Log($"[{gameObject.name} Error]: Already initialized");
            return;
        }
        isInitialized = true;
        FadeIn(mainCG, fadeDuration, fadeStartDelay, EaseType.linear);
    }

    public virtual Coroutine FadeIn(CanvasGroup cg, float duration, float delay, EaseType easing, Action argOnComplete = null)
    {
        OnTransitionStarted();
        cg.alpha = 0f;

        Action<float> tweenAction = lerp => { cg.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            argOnComplete.InvokeNullCheck();
            ToggleMenuInteraction(cg, true);
            OnTransitionFinished();
        };

        return this.DoTween(tweenAction, onCompleteCallback, duration, delay, easing, true);
    }

    public virtual Coroutine FadeOut(CanvasGroup cg, float duration, float delay, EaseType easing, Action argOnComplete = null)
    {
        OnTransitionStarted();

        Action<float> tweenAction = lerp => { cg.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            argOnComplete.InvokeNullCheck();
            ToggleMenuInteraction(cg, true);
            OnTransitionFinished();
        };

        return this.DoTween(tweenAction, onCompleteCallback, duration, delay, easing, true);
    }

    protected virtual void OnTransitionStarted()
    {
        isScreenBusy = true;
        ToggleMenuInteraction(mainCG, false);
    }

    protected virtual void OnTransitionFinished()
    {
        isScreenBusy = false;
        ToggleMenuInteraction(mainCG, true);
    }

    protected void ToggleMenuInteraction(CanvasGroup cg, bool isInteractible)
    {
        cg.interactable = isInteractible;
    }
}