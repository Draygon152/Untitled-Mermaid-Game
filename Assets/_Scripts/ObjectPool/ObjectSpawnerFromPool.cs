using System.ComponentModel;
using UnityEngine;

public abstract class ObjectSpawnerFromPool : MonoBehaviour
{
    [Description(
        " You can spawn an object assigned to the pool based on the object Tag \n You can override SpawnObject() to spawn with custom behavior."
    )]
    [SerializeField]
    protected ObjectPool objectPool;

    [Tooltip("The object you want to get from the")]
    [SerializeField]
    protected string spawnObjectTag;
    [SerializeField]
    [Tooltip("Random Min Spawn Time")]
    protected float spawnerCounterMinTime = 5f;

    [Tooltip("Random Max Spawn Time")]
    [SerializeField]
    protected float spawnerCounterMaxTime = 20f;

    [Tooltip("how many object will be spawn at once")]
    private float spawnerCounter = 0f;



    private void Start()
    {
        if (!objectPool)
        {
            objectPool = GameObject.Find("objectPool").GetComponent<ObjectPool>();
        }
    }

    private void Update()
    {
        spawnerCounter -= Time.deltaTime;
        if (spawnerCounter < 0f)
        {
            SpawnObject();
            spawnerCounter = Random.Range(spawnerCounterMinTime, spawnerCounterMaxTime);
        }
    }

    public virtual void SpawnObject()
    {
        Vector3 pos = transform.position;
        objectPool.SpawnFromPool(spawnObjectTag, pos, Quaternion.identity);
    }
}
