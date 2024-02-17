using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic for Trashy Trouble
/// </summary>
public class TrashyTroubleManager : SceneSingleton<TrashyTroubleManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;
    [SerializeField] private float timeToWait = 5f; // Amount of time to wait after minigame fail before restarting
    [Space]
    [SerializeField] private List<TrappedCreature> trappedCreatures = null;
    
    private int creaturesFreed = 0;



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
        timer.Init( () => { StartCoroutine(RestartMinigame()); });

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            //canvas.worldCamera = GameCameraManager.instance.gameCamera;

            EventManager.instance.Subscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);

            canvasGroup.interactable = true;
            timer.SetTimerActive(true);
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private IEnumerator RestartMinigame()
    {
        canvasGroup.interactable = false;

        yield return new WaitForSeconds(timeToWait);

        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.TrashyTrouble,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.TrashyTrouble);
                                                                         } ));
    }

    private Coroutine EndMinigame()
    {
        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);
            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.TrashyTrouble) );
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
            EndMinigame();
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