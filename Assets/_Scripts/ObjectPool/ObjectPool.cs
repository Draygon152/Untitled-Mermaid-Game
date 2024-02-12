using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

using System;

public class ObjectPool : MonoBehaviour
{

    [TextArea]

    public string aboutObjectPool =
     "Object pooling to Instantiate the object before the game start for better performance \n" +
     "usage:\n"+
     "1. add the prefab and tag(for dictionary) to the Pool in editor.\n"+
     "2. decide how many perfabs will be spawned to the pool.\n"+
     "3. ObjectPool.Instance.SpawnFromPool(string target_tag, Vector3 position, Quaternion rotation).\n"+
     "4. CreateNewObjectToPool(string tag, int size, GameObject prefab)\n";

    //Object pooling to Instantiate the object before the game start for better performance
    //usage:
    //1. add the prefab and tag(for dictionary) to the Pool in editor.
    //2. decide how many perfabs will be spawned to the pool.

    // functions:
    // ObjectPool.Instance.SpawnFromPool(string target_tag, Vector3 position, Quaternion rotation)

    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    // public static ObjectPool Instance { get; set; } //need to be private

    [System.Serializable]
    public class Pool
    {
        public string new_tag;
        public GameObject prefab;
        public int size;

        public Pool(string tag, int size, GameObject prefab){
            this.new_tag = tag;
            this.size = size;
            this.prefab = prefab;
        }
    }

    [Tooltip("You can Add a new object with tag, size and perfab object")]
    public List<Pool> Pools;
    private void Awake()
    {
    //Singleton
    //    if(Instance == null)
    //     {
    //         Debug.Log("only one Object pool instance available");
    //     }
    //    Instance = this;
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();


    }
    void Start(){
        foreach (Pool pool in Pools)
        {
            CreateNewObjectToPool(pool.new_tag, pool.size, pool.prefab);
        }
    }

    public void CreateNewObjectToPool(string tag, int size, GameObject prefab){

        if(PoolDictionary.ContainsKey(tag)) {
            Debug.Log("target pool tag has already exist");
            return;
        }

        Queue<GameObject> newObjectPooling = new Queue<GameObject>();
        // create a new object group for the pool
        GameObject objectTagFolder = new GameObject(tag + " group");
        objectTagFolder.transform.SetParent(this.transform);
        

        // Instantiate prefabs with size
        for (int i = 0; i < size; i++)
        {
            GameObject newGameObject = Instantiate(prefab, transform, true);
            newGameObject.transform.SetParent(objectTagFolder.transform);
            newGameObject.SetActive(false);
            newObjectPooling.Enqueue(newGameObject);
        }
        Pools.Add(new Pool(tag,size, prefab));

        Debug.Log("tag added: "+ tag);

        // Add to Pool
        PoolDictionary.Add(tag, newObjectPooling);
    }


    public GameObject SpawnFromPool(string target_tag, Vector3 position, Quaternion rotation)
    {
        //object not exist
        if (!PoolDictionary.ContainsKey(target_tag))
        {
            Debug.Log(target_tag + "not exist");
            return null;
        }
        // Debug.Log(target_tag + "is spawning");

        GameObject spawnObj = PoolDictionary[target_tag].Dequeue();

        //not active
        if(!spawnObj.gameObject.activeSelf){
            spawnObj.transform.position = position;
            spawnObj.transform.rotation = rotation;
            spawnObj.SetActive(true);
        }
        

        //Instantiate a new prefab if not enough size
        else{
            
            foreach (Pool pool in Pools)
            {
                if(pool.new_tag == target_tag){
                    Transform objectTagFolder = transform.Find(target_tag + " group");
                    GameObject newSpawnObj = Instantiate(pool.prefab, transform, true);
                    newSpawnObj.transform.position = position;
                    newSpawnObj.transform.rotation = rotation;
                    newSpawnObj.SetActive(true);
                    newSpawnObj.transform.SetParent(objectTagFolder);
                    PoolDictionary[target_tag].Enqueue(newSpawnObj);
                }
            }
        }

        PoolDictionary[target_tag].Enqueue(spawnObj);
        
        return spawnObj;
    }
}

