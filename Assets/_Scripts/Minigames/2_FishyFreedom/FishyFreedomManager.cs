using System;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic for FishyFreedom
/// </summary>
public class FishyFreedomManager : SceneSingleton<FishyFreedomManager>
{
    [SerializeField] private Canvas _canvas = null;
    public Canvas canvas => _canvas;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private FailScreen failScreen = null;
    [SerializeField] private SuccessScreen successScreen = null;
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;

    private Action onMinigameFail = null;
    private int score = 0;



    private void Start()
    {
        onMinigameFail += () =>
        {
            timer.SetTimerActive(false);
            ShowFailScreen();
        };
        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);
        EventManager.instance.Subscribe(EventManager.EventTypes.TrashPulledUp, OnTrashPulledUp);

        failScreen.Init(RestartMinigame);
        successScreen.Init(RestartMinigame, () => { EndMinigame(); });

        failScreen.gameObject.SetActive(false);
        successScreen.gameObject.SetActive(false);

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
            EventManager.instance.Notify(EventManager.EventTypes.MinigameSuccess);
            ShowSuccessScreen();
        } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            _canvas.worldCamera = GameCameraManager.instance.gameCamera;

            canvasGroup.interactable = true;
            timer.SetTimerActive(true);
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    public void RestartMinigame()
    {
        canvasGroup.interactable = false;

        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);
        EventManager.instance.Unsubscribe(EventManager.EventTypes.TrashPulledUp, OnTrashPulledUp);

        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.FishyFreedom,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.FishyFreedom);
                                                                         } ));
    }

    public Coroutine EndMinigame()
    {
        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.TrashPulledUp, OnTrashPulledUp);

            GameManager.instance.IncrementScore(score);

            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync((int)PersistentSceneManager.SceneIndices.FishyFreedom));
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private Coroutine ShowFailScreen()
    {
        failScreen.gameObject.SetActive(true);

        Action<float> tweenAction = lerp =>
        {
            failScreen.canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp);
        };
        Action onCompleteCallback = () =>
        {
            failScreen.canvasGroup.interactable = true;
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private Coroutine ShowSuccessScreen()
    {
        successScreen.gameObject.SetActive(true);

        Action<float> tweenAction = lerp =>
        {
            successScreen.canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp);
        };
        Action onCompleteCallback = () =>
        {
            successScreen.canvasGroup.interactable = true;
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private void OnTrashPulledUp()
    {
        score++;
    }

    protected override void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);
        EventManager.instance.Unsubscribe(EventManager.EventTypes.TrashPulledUp, OnTrashPulledUp);

        base.OnDestroy();
    }
}