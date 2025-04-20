using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class bulletManager : MonoBehaviour
{
    [Header("Enemy Pool")] // variables for the object pooling of enemies. Pooling is a technique used for performance to help lower CPU usage.
    public IObjectPool<ParticleSystem> waterPool;
    public IObjectPool<GameObject> girlyPool;
    public bool collectionChecks;
    public int maxPoolSize;

    [Header("GameObjects")]
    public GameObject spawnPoint;
    public ParticleSystem waterBulletPrefab;
    public GameObject girlyBulletPrefab;

    [Header("Var")]
    public string currentCharacterID; 

    public static bulletManager Instance; // static reference for global use

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // setting up the object pool for the bullet
    public IObjectPool<ParticleSystem> wPool
    {
        get // this can be called to activate all functions that control pool to pool a new bullet to the world
        {
            if (waterPool == null)
            {
                waterPool = new ObjectPool<ParticleSystem>(CreatePooledItemW, OnTakeFromPoolW, OnReturnedToPoolW, OnDestroyPoolObjectW, collectionChecks, 100, maxPoolSize); // keeps pool size under certain size for memory
            }
            return waterPool;
        }
    }

    private ParticleSystem CreatePooledItemW() // creates the item to be pooled, in this case it is bullet.
    {
        ParticleSystem newBullet = Instantiate(waterBulletPrefab, spawnPoint.transform.position, Quaternion.LookRotation(Vector3.down));
        newBullet.gameObject.SetActive(false);
        return newBullet;
    }

    private void OnTakeFromPoolW(ParticleSystem pooledBullet)//sets bullet to be true as a spawn when called, also adds to list for iteration
    {
        pooledBullet.gameObject.SetActive(true);
        Bullet bullet = pooledBullet.GetComponent<Bullet>();
        bullet.SetupManager(defenceManager.Instance); // following code is for injecting instances of scripts at runtime
        bullet.SetupEnemyManager(enemyManager.Instance);
        bullet.SetupBulletManager(bulletManager.Instance);
        bullet.onReuse();
    }

    public void OnReturnedToPoolW(ParticleSystem pooledBullet)// called when bullet is returned to pool and sets to false, also removes from list 
    {
        pooledBullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObjectW(ParticleSystem pooledBullet) // destroys pooled bullet
    {
        Destroy(pooledBullet.gameObject);
    }

    // girly bullet pool 
    // setting up the object pool for the bullet
    public IObjectPool<GameObject> gPool
    {
        get // this can be called to activate all functions that control pool to pool a new bullet to the world
        {
            if (girlyPool == null)
            {
                girlyPool = new ObjectPool<GameObject>(CreatePooledItemG, OnTakeFromPoolG, OnReturnedToPoolG, OnDestroyPoolObjectG, collectionChecks, 100, maxPoolSize); // keeps pool size under certain size for memory
            }
            return girlyPool;
        }
    }

    private GameObject CreatePooledItemG() // creates the item to be pooled, in this case it is bullet.
    {
        GameObject newBullet = Instantiate(girlyBulletPrefab, spawnPoint.transform.position, Quaternion.LookRotation(Vector3.down));
        newBullet.gameObject.SetActive(false);
        return newBullet;
    }

    private void OnTakeFromPoolG(GameObject pooledBullet)//sets bullet to be true as a spawn when called, also adds to list for iteration
    {
        pooledBullet.gameObject.SetActive(true);
        Bullet bullet = pooledBullet.GetComponent<Bullet>();
        bullet.SetupManager(defenceManager.Instance); // following code is for injecting instances of scripts at runtime
        bullet.SetupEnemyManager(enemyManager.Instance);
        bullet.SetupBulletManager(bulletManager.Instance);
        bullet.onReuse();
    }

    public void OnReturnedToPoolG(GameObject pooledBullet)// called when bullet is returned to pool and sets to false, also removes from list 
    {
        pooledBullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObjectG(GameObject pooledBullet) // destroys pooled bullet
    {
        Destroy(pooledBullet.gameObject);
    }
    // Awake is called before the first frame update
    void Awake()
    {
        var _ = wPool;
        var __ = gPool;
        Instance = this;
    }
}
