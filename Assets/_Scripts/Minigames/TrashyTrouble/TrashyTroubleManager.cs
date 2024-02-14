using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Management class responsible for handling minigame logic
/// </summary>
public class TrashyTroubleManager : SceneSingleton<TrashyTroubleManager>
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private Timer timer = null;
    [SerializeField] private float timeToWait = 5f; // Amount of time to wait after minigame fail before restarting
    [Space]
    [SerializeField] private List<TrappedCreature> trappedCreatures = null;
    
    private int creaturesFreed = 0;



    private void Start()
    {
        canvas.worldCamera = GameCameraManager.instance.gameCamera;

        EventManager.instance.Subscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);

        timer.Init(RestartMinigame);
        timer.SetTimerActive(true);
    }

    private void FixedUpdate()
    {
        timer.Tick();
    }

    private void RestartMinigame()
    {
        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.TrashyTrouble,
                                                                         () =>
                                                                         {
                                                                             GameManager.instance.StartMinigame(PersistentSceneManager.SceneIndices.TrashyTrouble);
                                                                         } ));
    }

    private void EndMinigame()
    {
        Debug.Log("MINIGAME ENDED");
        timer.SetTimerActive(false);

        EventManager.instance.Unsubscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);
        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync((int)PersistentSceneManager.SceneIndices.TrashyTrouble));
    }

    private void OnCreatureFreed()
    {
        creaturesFreed++;

        // If all creatures freed, end minigame
        if (creaturesFreed == trappedCreatures.Count)
        {
            EndMinigame();
        }
    }

    protected override void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}