using System;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
///     Management class responsible for handling minigame logic for Plankton Platoon
/// </summary>
public class PlanktonPlatoonManager : SceneSingleton<PlanktonPlatoonManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private float timeToWait = 5f; // Amount of time to wait after minigame fail before restarting
    [Space]
    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;
    [Space]
    [SerializeField] private Timer timer = null;
    [SerializeField] private TMP_Text collectedDisplay = null;
    [Space]
    [SerializeField] private Transform dragNet = null;
    [SerializeField] private ObjectDetection dragNetDetector = null;
    [SerializeField] private float moveSpeed = 5f;
    private static int MOVESPEED_MULTIPLIER = 10;
    [Space]
    [SerializeField] private int targetPlanktonToCollect = 30;
    private int planktonCollected = 0;



    private void Start()
    {
        StartMinigame();
    }

    private void FixedUpdate()
    {
        timer.Tick();
        MoveDragNet();
    }

    private Coroutine StartMinigame()
    {
        canvasGroup.alpha = 0f;
        collectedDisplay.text = $"PLANKTON COLLECTED: {planktonCollected}";

        timer.Init( () =>
        {
            if (planktonCollected < targetPlanktonToCollect)
            {
                dragNetDetector.viewDistance = 0f;
                StartCoroutine(RestartMinigame());
            }

            else
            {
                EndMinigame();
            }
        } );

        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(0f, 1f, lerp); };
        Action onCompleteCallback = () =>
        {
            //canvas.worldCamera = GameCameraManager.instance.gameCamera;

            EventManager.instance.Subscribe(EventManager.EventTypes.PlanktonCollected, OnPlanktonCollected);

            canvasGroup.interactable = true;
            timer.SetTimerActive(true);
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private IEnumerator RestartMinigame()
    {
        canvasGroup.interactable = false;

        yield return new WaitForSeconds(timeToWait);

        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.PlanktonPlatoon,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.PlanktonPlatoon);
                                                                         } ));
    }

    private Coroutine EndMinigame()
    {
        Action<float> tweenAction = lerp => { canvasGroup.alpha = Mathf.Lerp(1f, 0f, lerp); };
        Action onCompleteCallback = () =>
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameEnd);
            EventManager.instance.Unsubscribe(EventManager.EventTypes.PlanktonCollected, OnPlanktonCollected);
            StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.PlanktonPlatoon) );
        };

        return this.DoTween(tweenAction, onCompleteCallback, fadeDuration, fadeStartDelay, EaseType.linear, true);
    }

    private void MoveDragNet()
    {
        // Grab mouse position along the y-axis
        Vector3 mousePosition = canvas.worldCamera.ScreenToWorldPoint(PlayerInputManager.instance.GetMouseAxisVector());
        mousePosition.x = dragNet.position.x;
        mousePosition.z = 0f;

        // Remove sprite jitter when position delta is too small
        if (Vector3.Distance(mousePosition, dragNet.position) > 10)
        {
            Vector3 movementDirection = (mousePosition - dragNet.position).normalized;
            movementDirection.z = 0f; // Ensure movement direction can never leave the xy-plane

            dragNet.position += movementDirection * (moveSpeed * MOVESPEED_MULTIPLIER) * Time.fixedDeltaTime;
        }
    }

    private void OnPlanktonCollected()
    {
        planktonCollected++;
        collectedDisplay.text = $"PLANKTON COLLECTED: {planktonCollected}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.instance.Unsubscribe(EventManager.EventTypes.PlanktonCollected, OnPlanktonCollected);
    }
}