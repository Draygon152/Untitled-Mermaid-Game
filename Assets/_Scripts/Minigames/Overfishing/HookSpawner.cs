using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;
    private float nextSpawnTime;
    public List<GameObject> hooks; 

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHook();
            nextSpawnTime = Time.time + Random.Range(4f, 8f);
        }
    }

    void SpawnHook()
    {
        float randomX = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize * Camera.main.aspect);
        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);

        GameObject thisHook = Instantiate(hookPrefab, spawnPosition, Quaternion.identity);
        hooks.Add(thisHook);
    }
}