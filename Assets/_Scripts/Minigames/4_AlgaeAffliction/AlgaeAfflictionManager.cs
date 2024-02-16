using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic for Algae Affliction
/// </summary>
public class AlgaeAfflictionManager : SceneSingleton<AlgaeAfflictionManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private float timeToWait = 5f; // Amount of time to wait after minigame fail before restarting
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;
    [Space]
    [SerializeField] private int numAlgaeToSpawn = 5;
    [SerializeField] private List<Scrubbable> algaeList = null;

    private int algaeScrubbed = 0;



    private void Start()
    {
        foreach (Scrubbable algae in algaeList)
        {
            algae.gameObject.SetActive(false);
        }

        StartMinigame();
    }

    private void FixedUpdate()
    {
        timer.Tick();
    }

    private Coroutine StartMinigame()
    {
        SelectAlgae();
        canvasGroup.alpha = 0f;
        timer.Init( () => { StartCoroutine(RestartMinigame()); } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            canvas.worldCamera = GameCameraManager.instance.gameCamera;

            EventManager.instance.Subscribe(EventManager.EventTypes.AlgaeScrubbed, OnAlgaeScrubbed);

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
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.AlgaeScrubbed, OnAlgaeScrubbed);
            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.AlgaeAffliction) );
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private void SelectAlgae()
    {
        if (numAlgaeToSpawn > algaeList.Count)
        {
            Debug.LogError($"[AlgaeAfflictionManager Error]: Selected number of algae to spawn '{numAlgaeToSpawn}' exceeds the number of algae in the list '{algaeList.Count}'");
            return;
        }

        System.Random rand = new System.Random();
        List<Scrubbable> activeAlgae = algaeList.OrderBy(index => rand.Next()).ToList();

        for (int i = 0; i < numAlgaeToSpawn; i++)
        {
            activeAlgae[i].gameObject.SetActive(true);
        }
    }

    private void OnAlgaeScrubbed()
    {
        algaeScrubbed++;

        if (algaeScrubbed == numAlgaeToSpawn)
        {
            Debug.Log("ENDING");
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