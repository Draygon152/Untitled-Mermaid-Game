using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic for Algae Affliction
/// </summary>
public class AlgaeAfflictionManager : SceneSingleton<AlgaeAfflictionManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;
    [SerializeField] private float timeToWait = 5f; // Amount of time to wait after minigame fail before restarting
    [Space]
    [SerializeField] private List<Scrubbable> algaeList = null;

    private int algaeScrubbed = 0;



    private void Start()
    {
        StartMinigame();
    }

    private Coroutine StartMinigame()
    {
        canvasGroup.alpha = 0f;
        timer.Init( () => { StartCoroutine(RestartMinigame()); } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            canvas.worldCamera = GameCameraManager.instance.gameCamera;

            EventManager.instance.Subscribe(EventManager.EventTypes.CreatureFreed, OnAlgaeScrubbed);

            canvasGroup.interactable = true;
            timer.SetTimerActive(true);
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private IEnumerator RestartMinigame()
    {
        canvasGroup.interactable = false;

        yield return new WaitForSeconds(timeToWait);

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
            EventManager.instance.Unsubscribe(EventManager.EventTypes.AlgaeScrubbed, OnAlgaeScrubbed);
            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.AlgaeAffliction) );
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private void OnAlgaeScrubbed()
    {
        algaeScrubbed++;

        if (algaeScrubbed == algaeList.Count)
        {
            timer.SetTimerActive(false);
            EndMinigame();
        }
    }

    protected override void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.AlgaeScrubbed, OnAlgaeScrubbed);

        if (instance == this)
        {
            instance = null;
        }
    }
}