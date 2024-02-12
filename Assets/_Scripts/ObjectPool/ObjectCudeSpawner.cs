using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCudeSpawner : ObjectSpawnerFromPool
{

    [Tooltip("Spawn in Cude Area")]
    public Vector3 spawnAreaSize;
    private Vector3 spawnCenterLocation;
    
    public override void SpawnObject()
    {
        float x = Random.Range(spawnCenterLocation.x - spawnAreaSize.x / 2, spawnCenterLocation.x + spawnAreaSize.x / 2);
        float y = Random.Range(spawnCenterLocation.y - spawnAreaSize.y / 2, spawnCenterLocation.y + spawnAreaSize.y / 2);
        float z = Random.Range(spawnCenterLocation.z - spawnAreaSize.z / 2, spawnCenterLocation.z + spawnAreaSize.z / 2);

        Vector3 pos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        objectPool.SpawnFromPool(spawnObjectTag, pos, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

}
