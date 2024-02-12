using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectCreator : MonoBehaviour
{

    [SerializeField]
    protected ObjectPool objectPool;

    public string target_tag;
    public int size;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        if (!objectPool)
        {
            objectPool = GameObject.Find("objectPool").GetComponent<ObjectPool>();
        }
        objectPool.CreateNewObjectToPool(target_tag, size, prefab);
    }
}
