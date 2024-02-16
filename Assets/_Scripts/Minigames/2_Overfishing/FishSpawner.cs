using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnTime = 3f;
    [SerializeField] float maxSpawnTime = 7f;
    [SerializeField] GameObject fishPrefab;
    [SerializeField] int poolSize = 5;
    [SerializeField] GameObject parent;
    private List<GameObject> fishPool = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        InitializePool();
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

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fish = Instantiate(fishPrefab);
            fish.SetActive(false);
            fishPool.Add(fish);
            fish.transform.SetParent(parent.transform);
        }
    }

    GameObject GetFishFromPool()
    {
        foreach (GameObject fish in fishPool)
        {
            if (!fish.activeInHierarchy)
            {
                return fish;
            }
        }

        GameObject newFish = Instantiate(fishPrefab);
        newFish.SetActive(false);
        fishPool.Add(newFish);
        return newFish;
    }

    void SpawnFish()
    {
        float randomY = Random.Range(-Camera.main.orthographicSize, 0);
        Vector2 pos = new Vector2(transform.position.x, randomY);

        GameObject fish = GetFishFromPool();
        fish.transform.position = pos;
        fish.SetActive(true);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }
}