using System;
using System.Collections;
using UnityEngine;

public static class CoroutineExtensions
{
    public static Coroutine DoTween(this MonoBehaviour invokedOn, Action<float> tweenAction, Action onCompleteCallback = null, float duration = 0f, float delay = 0f, EaseType easeType = EaseType.linear, bool useUnscaledTime = false)
    {
        return invokedOn.StartCoroutine(CoDoTween(tweenAction, onCompleteCallback, duration, delay, easeType, useUnscaledTime));
    }

    public static IEnumerator CoDoTween(Action<float> tweenAction, Action onCompleteCallback = null, float duration = 0f, float delay = 0f, EaseType easeType = EaseType.linear, bool useUnscaledTime = false)
    {
        if (delay > 0f)
        {
            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
        }

        // Get the easing function for this EaseType.
        TweenEasing.EasingFunction easeFunc = TweenEasing.GetEasingFunction(easeType);
        if (easeFunc == null)
        {
            // Default on linear easing.
            easeFunc = TweenEasing.GetEasingFunction(EaseType.linear);
        }

        // Run the tween over the duration.
        float timer = 0f;
        while (timer < duration)
        {
            timer += (useUnscaledTime) ? Time.unscaledDeltaTime : Time.deltaTime;

            // Run the tween function for 
            tweenAction(easeFunc(0, 1, timer / duration));

            yield return null;
        }

        // Call the callback, if assigned.
        onCompleteCallback?.Invoke();
    }
}