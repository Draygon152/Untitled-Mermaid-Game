using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnTime = 3f;
    [SerializeField] float maxSpawnTime = 7f;
    public GameObject fishPrefab; 
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
        float randomY = Random.Range(-Camera.main.orthographicSize, 0);
        Vector2 pos = new Vector2(transform.position.x, randomY);
        Instantiate(fishPrefab, pos, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }
}