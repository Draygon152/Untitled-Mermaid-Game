using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Management class responsible for handling minigame logic
/// </summary>
public class TrashyTroubleManager : SceneSingleton<TrashyTroubleManager>
{
    [SerializeField] private List<TrappedCreature> trappedCreatures = null;
    [SerializeField] private Timer timer = null;

    private int creaturesFreed = 0;



    private void Start()
    {
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
        foreach (TrappedCreature creature in trappedCreatures)
        {
            creature.ResetCreature();
        }

        timer.ResetTimer();
        timer.SetTimerActive(true);
    }

    private void EndMinigame()
    {
        Debug.Log("MINIGAME ENDED");
        EventManager.instance.Unsubscribe(EventManager.EventTypes.CreatureFreed, OnCreatureFreed);
        AdditiveSceneManager.instance.UnloadSceneAsync( (int)AdditiveSceneManager.SceneIndices.GameScene );
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