using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic for Algae Affliction
/// </summary>
public class AlgaeAfflictionManager : SceneSingleton<AlgaeAfflictionManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private Canvas bgCanvas = null;
    [SerializeField] private FailScreen failScreen = null;
    [SerializeField] private SuccessScreen successScreen = null;
    [SerializeField] private TutorialUIManager tutorialManager = null;
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;
    [Space]
    [SerializeField] private int numAlgaeToSpawn = 5;
    [SerializeField] private List<Coral> coralList = null;

    private int coralCleaned = 0;
    private Action onMinigameFail = null;



    private void Start()
    {
        Cursor.SetCursor(GameManager.instance.ScrubberCursor, GameManager.instance.hotSpot, GameManager.instance.cursorMode);
        onMinigameFail += () =>
        {
            timer.SetTimerActive(false);
            ShowFailScreen();
        };
        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);
        EventManager.instance.Subscribe(EventManager.EventTypes.CoralCleaned, OnCoralCleaned);

        failScreen.Init(RestartMinigame);
        successScreen.Init(RestartMinigame, () => { EndMinigame(); });

        failScreen.gameObject.SetActive(false);
        successScreen.gameObject.SetActive(false);

        tutorialManager.Init(() =>
        {
            foreach (Coral coral in coralList)
            {
                coral.Init();
            }

            timer.SetTimerActive(true);
        });

        canvas.worldCamera = GameCameraManager.instance.gameCamera;
        bgCanvas.worldCamera = GameCameraManager.instance.gameCamera;

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
            foreach (Coral coral in coralList)
            {
                coral.KillCoral();
            }

            ShowFailScreen();
        } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            canvasGroup.interactable = true;
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private void RestartMinigame()
    {
        canvasGroup.interactable = false;

        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);

        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.AlgaeAffliction,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.AlgaeAffliction);
                                                                         } ));
    }

    private Coroutine EndMinigame()
    {
        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.CoralCleaned, OnCoralCleaned);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);

            GameManager.instance.IncrementScore(coralCleaned);

            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.AlgaeAffliction) );
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

    private void OnCoralCleaned()
    {
        coralCleaned++;

        if (coralCleaned == coralList.Count)
        {
            timer.SetTimerActive(false);
            ShowSuccessScreen();
        }
    }

    protected override void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.CoralCleaned, OnCoralCleaned);
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, onMinigameFail);

        if (instance == this)
        {
            instance = null;
        }
    }
}