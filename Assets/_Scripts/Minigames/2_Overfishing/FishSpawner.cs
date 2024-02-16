using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab; 
    public float minSpawnTime = 3f;
    public float maxSpawnTime = 7f;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnFish();
            SetNextSpawnTime();
        }
    }

    void SpawnFish()
    {
        Instantiate(fishPrefab, transform.position, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }
}