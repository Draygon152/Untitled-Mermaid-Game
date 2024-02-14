using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        timer.Init(() => { StartCoroutine(RestartMinigame()); });
        timer.SetTimerActive(true);
    }

    private void FixedUpdate()
    {
        timer.Tick();
    }

    private IEnumerator RestartMinigame()
    {
        foreach (TrappedCreature creature in trappedCreatures)
        {
            creature.ResetCreature();
        }

        creaturesFreed = 0;

        yield return new WaitForSeconds(timeToWait);

        timer.ResetTimer();
        timer.SetTimerActive(true);
    }

    private void EndMinigame()
    {
        Debug.Log("MINIGAME ENDED");
        timer.SetTimerActive(false);

        EventManager.instance.Unsubscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);
        PersistentSceneManager.instance.UnloadSceneAsync( 3 ); // TODO: REPLACE WITH SCENE INDEX
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