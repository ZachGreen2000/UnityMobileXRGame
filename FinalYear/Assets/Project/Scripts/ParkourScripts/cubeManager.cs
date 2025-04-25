using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class cubeManager : MonoBehaviour
{
    [Header("Variables")]
    public IObjectPool<GameObject> cubePool;
    public GameObject cubePrefab;
    public Vector3 spawnPos;
    public bool collectionChecks;
    public int maxPoolSize;

    public static cubeManager Instance;
    // setting up the object pool for the cube
    public IObjectPool<GameObject> Pool
    {
        get // this can be called to activate all functions that control pool to pool a new cube to the world
        {
            if (cubePool == null)
            {
                cubePool = new ObjectPool<GameObject>(CreatePooledItemW, OnTakeFromPoolW, OnReturnedToPoolW, OnDestroyPoolObjectW, collectionChecks, 100, maxPoolSize); // keeps pool size under certain size for memory
            }
            return cubePool;
        }
    }

    private GameObject CreatePooledItemW() // creates the item to be pooled, in this case it is cube.
    {
        GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.LookRotation(Vector3.up));
        cube.transform.rotation = Quaternion.Euler(0, 90, 0);
        cube.SetActive(false);
        return cube;
    }

    private void OnTakeFromPoolW(GameObject cube)//sets cube to be true as a spawn when called, also adds to list for iteration
    {
        cube.SetActive(true);
    }

    public void OnReturnedToPoolW(GameObject cube)// called when cube is returned to pool and sets to false, also removes from list 
    {
        cube.SetActive(false);
    }

    private void OnDestroyPoolObjectW(GameObject cube) // destroys pooled cube
    {
        Destroy(cube);
    }
    // initialised pool and script instance
    void Awake()
    {
        var _ = Pool;
        Instance = this;
    }
    // called from manager script to provide next spawn position for cube
    public void setNextSpawn(Vector3 pos)
    {
        spawnPos = pos;
    }
}
