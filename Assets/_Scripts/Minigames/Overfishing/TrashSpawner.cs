using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 10f;
    [SerializeField] private GameObject[] trashObjs;
    [SerializeField] GameObject parent; 
    private float nextSpawnTime;
    private Dictionary<GameObject, List<GameObject>> trashPools = new Dictionary<GameObject, List<GameObject>>();
    [SerializeField] private int initialPoolSize = 1;

    void Start()
    {
        InitializePools();
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

    void InitializePools()
    {
        foreach (GameObject obj in trashObjs)
        {
            List<GameObject> pool = new List<GameObject>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject pooledObj = Instantiate(obj);
                pooledObj.SetActive(false);
                pooledObj.transform.SetParent(parent.transform);
                pool.Add(pooledObj);
            }
            trashPools[obj] = pool;
        }
    }

    GameObject GetTrashFromPool(GameObject prefab)
    {
        if (!trashPools.ContainsKey(prefab) || trashPools[prefab].Count == 0)
        {
            return null; 
        }

        foreach (GameObject obj in trashPools[prefab])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        SpawnTrash();
        return null;
    }

    void SpawnTrash()
    {
        GameObject prefab = GetRandomObj();
        if (prefab != null)
        {
            float randomY = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
            Vector2 pos = new Vector2(transform.position.x, randomY);
            GameObject trash = GetTrashFromPool(prefab);
            if (trash != null)
            {
                trash.transform.position = pos;
                trash.SetActive(true);
            }
        }
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
