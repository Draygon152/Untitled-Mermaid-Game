using System.Collections.Generic;
using UnityEngine;

public class HookSpawner : SceneSingleton<HookSpawner>
{
    [SerializeField] private GameObject hookPrefab = null;
    [SerializeField] private int poolSize = 6;
    [Space]
    [SerializeField] private float minSpawnInterval = 4f;
    [SerializeField] private float maxSpawnInterval = 8f;
    [SerializeField] private float minHookSpawnDistance = 60f;

    private List<GameObject> hooksPool = null;
    private float nextSpawnTime = 0f;
    private float lastHookX = 0f;
    private bool validSpawn = false;



    protected override void Awake()
    {
        base.Awake();

        hooksPool = new List<GameObject>();
    }

    private void Start()
    {
        InitializePool();
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void FixedUpdate()
    {
        nextSpawnTime -= Time.fixedDeltaTime;
        if (nextSpawnTime <= 0f)
        {
            SpawnHook();
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(hookPrefab);
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

        GameObject newHook = Instantiate(hookPrefab);
        newHook.SetActive(false);
        hooksPool.Add(newHook);

        return newHook;
    }
}