using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private int poolSize = 6;
    [SerializeField] GameObject parent;
    private float nextSpawnTime;
    private List<GameObject> hooksPool = new();
    private bool validSpawn = false;

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHook();
            nextSpawnTime = Time.time + Random.Range(4f, 8f);
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(hookPrefab);
            obj.SetActive(false);
            hooksPool.Add(obj);
            obj.transform.SetParent(parent.transform);
        }
    }

    GameObject GetHookFromPool()
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

    void SpawnHook()
    {
        float randomX = 0.0f;
        validSpawn = false;
        while (!validSpawn)
        {
            validSpawn = true;
            randomX = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect);
            foreach (GameObject hook in hooksPool)
            {
                if (hook.activeInHierarchy && Mathf.Abs(hook.transform.position.x - randomX) < 0.5f)
                {
                    validSpawn = false;
                    break;
                }
            }
        }
        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);

        GameObject thisHook = GetHookFromPool();
        thisHook.transform.position = spawnPosition;
        thisHook.SetActive(true);
    }
}