using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject trashPrefab;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 10f;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnTrash();
            SetNextSpawnTime();
        }
    }

    void SpawnTrash()
    {
        float randomY = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
        Vector2 pos = new Vector2(transform.position.x, randomY);
        Instantiate(trashPrefab, pos, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }
}