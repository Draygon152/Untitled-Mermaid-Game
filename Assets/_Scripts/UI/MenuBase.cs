using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuBase : SceneSingleton<MenuBase>
{
    [Header("Canvas Groups")]
    [SerializeField] protected CanvasGroup mainCanvasGroup = null;

    [Header("Fade Tween Delays")]
    [SerializeField] protected float fadeStartDelay = 0f;
    [SerializeField] protected float fadeDuration = 0.4f;

    [SerializeField] protected Button returnButton = null;

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
        FadeIn(mainCanvasGroup, fadeDuration, fadeStartDelay, EaseType.linear);
    }

    public virtual Coroutine FadeIn(CanvasGroup cg, float duration, float delay, EaseType easing, Action argOnComplete = null)
    {
        OnTransitionStarted();
        mainCanvasGroup.alpha = 0f;

        Action<float> tweenAction = lerp => { mainCanvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            argOnComplete.InvokeNullCheck();
            ToggleScreenInteraction(mainCanvasGroup, true);
            OnTransitionFinished();
        };

        return this.DoTween(tweenAction, onCompleteCallback, duration, delay, easing, true);
    }

    public virtual Coroutine FadeOut(CanvasGroup cg, float duration, float delay, EaseType easing, Action argOnComplete = null)
    {
        OnTransitionStarted();

        Action<float> tweenAction = lerp => { mainCanvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            argOnComplete.InvokeNullCheck();
            ToggleScreenInteraction(mainCanvasGroup, true);
            OnTransitionFinished();
        };

        return this.DoTween(tweenAction, onCompleteCallback, duration, delay, easing, true);
    }

    protected virtual void OnTransitionStarted()
    {
        isScreenBusy = true;
        ToggleScreenInteraction(mainCanvasGroup, false);
    }

    protected virtual void OnTransitionFinished()
    {
        isScreenBusy = false;
        ToggleScreenInteraction(mainCanvasGroup, true);
    }

    protected void ToggleScreenInteraction(CanvasGroup cg, bool isInteractible)
    {
        cg.interactable = isInteractible;
    }

    protected override void OnDestroy()
    {
        returnButton.onClick.RemoveAllListeners();
        base.OnDestroy();
    }
}