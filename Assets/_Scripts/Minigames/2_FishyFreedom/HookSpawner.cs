using System.Collections.Generic;
using UnityEngine;

public class HookSpawner : SceneSingleton<HookSpawner>
{
    [SerializeField] private List<GameObject> hookPrefabs = null;
    [SerializeField] private int poolSize = 6;
    [Space]
    [SerializeField] private float minSpawnInterval = 4f;
    [SerializeField] private float maxSpawnInterval = 8f;
    [Range(60f, 150f)]
    [SerializeField] private float minHookSpawnDistance = 60f;

    private List<GameObject> hooksPool = null;
    private float nextSpawnTime = 0f;
    private float lastHookX = 0f;
    private bool validSpawn = false;

    private bool minigameOver = false;



    protected override void Awake()
    {
        base.Awake();

        hooksPool = new List<GameObject>();
    }

    private void Start()
    {
        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameFail, OnMinigameEndScreen);
        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameSuccess, OnMinigameEndScreen);

        InitializePool();
        //nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void FixedUpdate()
    {
        if (!minigameOver)
        {
            nextSpawnTime -= Time.fixedDeltaTime;
            if (nextSpawnTime <= 0f)
            {
                SpawnHook();
            }
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(hookPrefabs[Random.Range(0, hookPrefabs.Count - 1)]);
            obj.SetActive(false);
            hooksPool.Add(obj);
            obj.transform.SetParent(transform);
        }
    }

    private void SpawnHook()
    {
        float randomX = 0.0f;
        validSpawn = false;
        while (!validSpawn)
        {
            Camera gameCamera = FishyFreedomManager.instance.canvas.worldCamera;
            randomX = Random.Range(-gameCamera.orthographicSize * gameCamera.aspect, gameCamera.orthographicSize * gameCamera.aspect);

            if (Mathf.Abs(randomX - lastHookX) >= minHookSpawnDistance)
            {
                Vector2 spawnPosition = new Vector2(randomX, transform.position.y);
                lastHookX = randomX;

                GameObject thisHook = GetHookFromPool();
                thisHook.transform.position = spawnPosition;
                thisHook.SetActive(true);

                validSpawn = true;
            }
        }

        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private GameObject GetHookFromPool()
    {
        foreach (GameObject hook in hooksPool)
        {
            if (!hook.activeInHierarchy)
            {
                return hook;
            }
        }

        GameObject newHook = Instantiate(hookPrefabs[Random.Range(0, hookPrefabs.Count - 1)]);
        newHook.SetActive(false);
        hooksPool.Add(newHook);

        return newHook;
    }

    private void OnMinigameEndScreen()
    {
        minigameOver = true;

        foreach (GameObject hook in hooksPool)
        {
            Destroy(hook);
        }
    }

    protected override void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, OnMinigameEndScreen);
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameSuccess, OnMinigameEndScreen);

        base.OnDestroy();
    }
}