using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{    
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 10f;
    [SerializeField] private GameObject[] trashObjs;
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
        GameObject trash = GetRandomObj();
        Instantiate(trash, pos, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    private GameObject GetRandomObj()
    {
        if (trashObjs.Length > 0)
        {
            int index = Random.Range(0, trashObjs.Length); 
            return trashObjs[index];
        }
        else return null;
    }
}