using System;
using System.Collections;
using UnityEngine;

public class FishyFreedomManager : SceneSingleton<FishyFreedomManager>
{
    [SerializeField] private Canvas _canvas = null;
    public Canvas canvas => _canvas;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private float timeToWait = 5f; // Amount of time to wait after minigame fail before restarting
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;



    private void Start()
    {
        StartMinigame();
    }

    private void FixedUpdate()
    {
        timer.Tick();
    }

    private Coroutine StartMinigame()
    {
        canvasGroup.alpha = 0f;

        timer.Init( () =>
        {
            EndMinigame();
        } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            //_canvas.worldCamera = GameCameraManager.instance.gameCamera;

            canvasGroup.interactable = true;
            timer.SetTimerActive(true);
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private IEnumerator RestartMinigame()
    {
        canvasGroup.interactable = false;

        yield return new WaitForSeconds(timeToWait);

        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.FishyFreedom,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.FishyFreedom);
                                                                         } ));
    }

    private Coroutine EndMinigame()
    {
        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.FishyFreedom ));
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }
}