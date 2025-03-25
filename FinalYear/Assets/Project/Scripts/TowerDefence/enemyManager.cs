using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Enemies;

public class enemyManager : MonoBehaviour
{
    [Header("Enemy Variables")] // variables for enemys for the spawning and target destination for enemies
    public enemy enemyPrefab;
    public int maxEnemy;
    public int enemySpawnSpeed;
    public GameObject spawnLocation;
    
    [Header("Enemy Pool")] // variables for the object pooling of enemies. Pooling is a technique used for performance to help lower CPU usage.
    [SerializeField] private IObjectPool<enemy> enemyPool;
    public bool collectionChecks;
    public int maxPoolSize;
    // setting up the object pool for the enemies
    public IObjectPool<enemy> Pool
    {
        get // this can be called to activate all functions that control pool to pool a new enemy to the world
        {
            if (enemyPool == null)
            {
                enemyPool = new ObjectPool<enemy>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
            }
            return enemyPool;
        }
    }

    private enemy CreatePooledItem()
    {
        enemy newEnemy = Instantiate (enemyPrefab, spawnLocation.transform.position, Quaternion.identity);
        newEnemy.gameObject.SetActive(false);
        return newEnemy;
    }

    private void OnTakeFromPool(enemy pooledEnemy)
    {
        pooledEnemy.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(enemy pooledEnemy)
    {
        pooledEnemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(enemy pooledEnemy)
    {
        Destroy(pooledEnemy.gameObject);
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // this function controls the spawning of the enemies by checking the location is spawnable and then using a do while loop to ensure they do not overlap each other when spawning
    public void Spawn()
    {
        Vector3 spawnPosition;
        bool canSpawn;
        if (spawnLocation.CompareTag("Spawnable"))
        {
            do
            {
                float randx = Random.Range(spawnLocation.transform.position.x - 2, spawnLocation.transform.position.x + 2); // calculates a random range for the ememies to spawn in
                float randy = Random.Range(spawnLocation.transform.position.y - 2, spawnLocation.transform.position.y + 2);
                spawnPosition = new Vector3(randx, randy, 0);
                canSpawn = true;
                /*foreach (enemy e in enemyPool.GetCollection())
                {
                    if (Vector3.Distance(e.transform.position, spawnPosition) < 2.0f)
                    {
                        canSpawn = false; 
                        break;
                    }
                }*/
            } while (!canSpawn); 

            var poolItem = enemyPool.Get();
            poolItem.transform.position = spawnPosition;
        }
    }
}
