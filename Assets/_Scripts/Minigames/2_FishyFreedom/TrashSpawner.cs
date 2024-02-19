using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] trashObjs = null;
    [SerializeField] private int poolSize = 8;
    [SerializeField] private GameObject parent = null;
    [SerializeField] private bool flipHorizontal = false;
    [Space]
    [SerializeField] private float minSpawnTime = 4f;
    [SerializeField] private float maxSpawnTime = 8f;

    private Dictionary<GameObject, List<GameObject>> trashPools = null;
    private float nextSpawnTime = 0f;

    private bool minigameStarted = false;
    private bool minigameOver = false;



    private void Start()
    {
        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameFail, OnMinigameEndScreen);
        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameSuccess, OnMinigameEndScreen);

        InitializePools();
    }

    private void FixedUpdate()
    {
        if (minigameStarted && !minigameOver && Time.fixedTime >= nextSpawnTime)
        {
            SpawnTrash();
            SetNextSpawnTime();
        }
    }

    public void OnMinigameStart()
    {
        minigameStarted = true;
    }

    private void InitializePools()
    {
        trashPools = new Dictionary<GameObject, List<GameObject>>();

        foreach (GameObject obj in trashObjs)
        {
            List<GameObject> pool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject pooledObj = Instantiate(obj);

                if (flipHorizontal)
                {
                    Vector3 newScale = pooledObj.transform.localScale;
                    newScale.x = -newScale.x;
                    pooledObj.transform.localScale = newScale;
                }

                pooledObj.SetActive(false);
                pooledObj.transform.SetParent(parent.transform);
                pool.Add(pooledObj);
            }
            trashPools[obj] = pool;
        }
    }

    private GameObject GetTrashFromPool(GameObject prefab)
    {
        if (!trashPools.ContainsKey(prefab) || trashPools[prefab].Count == 0)
        {
            Debug.LogError($"[TrashSpawner Error]: Trash item '{prefab.name}' not a part of trash pool");
            return null;
        }

        foreach (GameObject obj in trashPools[prefab])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newTrash = Instantiate(prefab);
        newTrash.SetActive(false);
        trashPools[prefab].Add(newTrash);

        return newTrash;
    }

    private void SpawnTrash()
    {
        GameObject prefab = GetRandomObj();
        if (prefab != null)
        {
            float randomY = Random.Range(-FishyFreedomManager.instance.canvas.worldCamera.orthographicSize, FishyFreedomManager.instance.canvas.worldCamera.orthographicSize);
            Vector2 pos = new Vector2(transform.position.x, randomY);

            GameObject trash = GetTrashFromPool(prefab);
            trash.transform.position = pos;
            trash.SetActive(true);
        }
    }

    private GameObject GetRandomObj()
    {
        if (trashObjs.Length > 0)
        {
            int index = Random.Range(0, trashObjs.Length);
            return trashObjs[index];
        }

        else
        {
            Debug.LogError($"[TrashSpawner Error]: List of trash objects is empty");
            return null;
        }
    }

    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.fixedTime + Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void OnMinigameEndScreen()
    {
        minigameOver = true;

        foreach (GameObject trashObj in trashObjs)
        {
            List<GameObject> curPool = trashPools[trashObj];

            foreach (GameObject obj in curPool)
            {
                Destroy(obj);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameFail, OnMinigameEndScreen);
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameSuccess, OnMinigameEndScreen);
    }
}