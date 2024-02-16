using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSpawner : MonoBehaviour
{
    public GameObject hookPrefab;
    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHook();
            nextSpawnTime = Time.time + Random.Range(1f, 5f);
        }
    }

    void SpawnHook()
    {
        float randomX = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect);
        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);

        Instantiate(hookPrefab, spawnPosition, Quaternion.identity);
    }
}