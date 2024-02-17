using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fishPrefab = null;
    [SerializeField] private int poolSize = 8;
    [SerializeField] private GameObject parent = null;
    [SerializeField] private bool facingLeft = true;
    [Space]
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 7f;
    
    private List<GameObject> fishPool = new List<GameObject>();
    private float nextSpawnTime = 0f;



    private void Start()
    {
        InitializePool();
        SetNextSpawnTime();
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime >= nextSpawnTime)
        {
            SpawnFish();
            SetNextSpawnTime();
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fish = Instantiate(fishPrefab);

            if (!facingLeft)
            {
                Vector3 newScale = fish.transform.localScale;
                newScale.x = -newScale.x;
                fish.transform.localScale = newScale;
            } 

            fish.SetActive(false);
            fishPool.Add(fish);
            fish.transform.SetParent(parent.transform);
        }
    }

    private GameObject GetFishFromPool()
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

    private void SpawnFish()
    {
        Camera gameCamera = FishyFreedomManager.instance.canvas.worldCamera;
        float randomY = Random.Range(-gameCamera.orthographicSize, 0);
        Vector2 pos = new Vector2(transform.position.x, randomY);

        GameObject fish = GetFishFromPool();
        fish.transform.position = pos;
        fish.SetActive(true);
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.fixedTime + Random.Range(minSpawnTime, maxSpawnTime);
    }
}