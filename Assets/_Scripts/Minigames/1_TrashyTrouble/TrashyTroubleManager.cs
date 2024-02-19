using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic for Trashy Trouble
/// </summary>
public class TrashyTroubleManager : SceneSingleton<TrashyTroubleManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private Canvas backgroundCanvas = null;
    [SerializeField] private FailScreen failScreen = null;
    [SerializeField] private SuccessScreen successScreen = null;
    [SerializeField] private TutorialUIManager tutorialManager = null;
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;
    [Space]
    [SerializeField] private List<TrappedCreature> trappedCreatures = null;
    
    private int creaturesFreed = 0;



    private void Start()
    {
        EventManager.instance.Subscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);

        Cursor.SetCursor(GameManager.instance.HandCursor, GameManager.instance.hotSpot, GameManager.instance.cursorMode);

        failScreen.Init(RestartMinigame);
        successScreen.Init(RestartMinigame, () =>
        {
            EndMinigame();
        } );

        failScreen.gameObject.SetActive(false);
        successScreen.gameObject.SetActive(false);

        tutorialManager.Init( () =>
        {
            timer.SetTimerActive(true);
        } );

        canvas.worldCamera = GameCameraManager.instance.gameCamera;
        backgroundCanvas.worldCamera = GameCameraManager.instance.gameCamera;

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
            ShowFailScreen();
        } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            canvasGroup.interactable = true;
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    public void RestartMinigame()
    {
        canvasGroup.interactable = false;

        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.TrashyTrouble,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.TrashyTrouble);
                                                                         } ));
    }

    public Coroutine EndMinigame()
    {
        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);

            GameManager.instance.IncrementScore(creaturesFreed);

            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.TrashyTrouble) );
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private Coroutine ShowFailScreen()
    {
        failScreen.gameObject.SetActive(true);

        Action<float> tweenAction = lerp =>
        {
            failScreen.canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp);

            foreach (TrappedCreature creature in trappedCreatures)
            {
                creature.trapSR.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerp));
                creature.creature.spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerp));
            }
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

    private void OnCreatureFreed()
    {
        creaturesFreed++;

        // If all creatures freed, end minigame
        if (creaturesFreed == trappedCreatures.Count)
        {
            timer.SetTimerActive(false);

            ShowSuccessScreen();
        }
    }

    protected override void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);

        if (instance == this)
        {
            instance = null;
        }
    }
}