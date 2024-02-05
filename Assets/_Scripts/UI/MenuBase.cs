using System;
using UnityEngine;

public class MenuBase<T> : SceneSingleton<MenuBase<T>> where T : MonoBehaviour
{
    [Header("Canvas Groups")]
    [SerializeField] protected CanvasGroup mainCG = null;

    [Header("Fade Tween Delays")]
    [SerializeField] protected float fadeStartDelay = 0f;
    [SerializeField] protected float fadeDuration = 0.4f;

    // Flag for if the screen is occupied with a fade, prevent input conflicts
    protected bool isScreenBusy = false;
    protected bool isInitialized = false;



    // Add return button listeners here in inherited classes
    public virtual void Init()
    {
        if (isInitialized)
        {
            Debug.Log($"[{instance.name} Error]: Already initialized");
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}