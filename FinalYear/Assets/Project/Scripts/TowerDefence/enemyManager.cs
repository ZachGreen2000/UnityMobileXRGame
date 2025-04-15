using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Enemies;

public class enemyManager : MonoBehaviour
{
    [Header("Enemy Variables")] // variables for enemys for the spawning and target destination for enemies
    public enemy enemyPrefab;
    public GameObject spawnLocation;
    
    [Header("Enemy Pool")] // variables for the object pooling of enemies. Pooling is a technique used for performance to help lower CPU usage.
    public IObjectPool<enemy> enemyPool;
    public bool collectionChecks;
    public int maxPoolSize;
    public List<enemy> enemyList;

    [Header("Other")]
    public GameObject noSpawn;
    public GameObject tower;
    // setting up the object pool for the enemies
    public IObjectPool<enemy> Pool
    {
        get // this can be called to activate all functions that control pool to pool a new enemy to the world
        {
            if (enemyPool == null)
            {
                enemyPool = new ObjectPool<enemy>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 1000, maxPoolSize); // keeps pool size under certain size for memory
            }
            return enemyPool;
        }
    }

    private enemy CreatePooledItem() // creates the item to be pooled, in this case it is our enemy.
    {
        enemy newEnemy = Instantiate (enemyPrefab, spawnLocation.transform.position, Quaternion.LookRotation(Vector3.down));
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
    }

    private void OnTakeFromPool(enemy pooledEnemy)//sets enemy to be true as a spawn when called, also adds to list for iteration
    {
        pooledEnemy.resetStats(1, 2);
        pooledEnemy.gameObject.SetActive(true);
        pooledEnemy.tag = "Enemy";
        pooledEnemy.gameObject.layer = LayerMask.NameToLayer("enemyLayer");
        enemyList.Add(pooledEnemy);
        Debug.Log($"Collider: {pooledEnemy.GetComponent<Collider>().enabled}");
        int layerIndex = LayerMask.NameToLayer("enemyLayer");
        Debug.Log($"enemyLayer index: " + layerIndex);
    }

    public void OnReturnedToPool(enemy pooledEnemy)// called when enemy is returned to pool and sets to false, also removes from list 
    {
        //Debug.Log("Enemy retrurned to pool: " + pooledEnemy);
        pooledEnemy.gameObject.SetActive(false);
        //Debug.Log("Enemy is: " + pooledEnemy.gameObject.activeSelf);
        enemyList.Remove(pooledEnemy);
        Debug.Log("On returned to pool");
    }

    private void OnDestroyPoolObject(enemy pooledEnemy) // destroys pooled enemy
    {
        Destroy(pooledEnemy.gameObject);
    } 
    // Awake is called before the first frame update
    void Awake()
    {
        var _ = Pool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // this function controls the spawning of the enemies by checking the location is spawnable and then using a do while loop to ensure they do not overlap each other when spawning
    public void Spawn()
    {
        Vector3 spawnPosition;
        bool canSpawn = false;
        int maxAttempts = 10;
        int attempts = 0;
        float noSpawnRadius = noSpawn.transform.localScale.x / 2;
        if (spawnLocation.CompareTag("Spawnable"))
        {
            do
            {
                float randx = Random.Range(spawnLocation.transform.position.x - 20, spawnLocation.transform.position.x + 20); // calculates a random range for the ememies to spawn in
                float randz = Random.Range(spawnLocation.transform.position.z - 20, spawnLocation.transform.position.z + 20); 
                spawnPosition = new Vector3(randx, 2, randz);
                float distanceCentre = Vector3.Distance(tower.transform.position, spawnPosition); // calculates no spawn zone
               // Debug.Log("Distance to centre: " +  distanceCentre);
                //Debug.Log("No Spawn radius: " + noSpawnRadius);
                if (distanceCentre >= noSpawnRadius)
                {
                   // Debug.Log("Can Spawn at position");
                    canSpawn = true;
                    foreach (enemy e in enemyList) // iterates through list of spawned enemies to check spawnable posiitons to ensure no overlap
                    {
                        if (Vector3.Distance(e.transform.position, spawnPosition) < 2.0f)
                        {
                           // Debug.Log("Enemy overlapping");
                            canSpawn = false;
                            break;
                        }
                    }
                }
                attempts++;
            } while (!canSpawn && attempts < maxAttempts); 

            if (canSpawn)
            {
                if (enemyPool == null)
                {
                    //Debug.Log("enemyPool is null");
                }
                var poolItem = Pool.Get(); // retrives the pool to set the transform position
                poolItem.transform.position = spawnPosition;
                Debug.Log("Enemy spawned at");
            } else
            {
                Debug.Log("Failed to find spawn position after " + maxAttempts);
                Debug.Log("Items in pool: " + enemyList.Count);
            }
        }
    }
}
