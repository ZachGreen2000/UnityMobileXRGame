using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class catchManager : MonoBehaviour
{
    [Header("Pool Variables")]
    public IObjectPool<GameObject> foodPool;
    public GameObject foodPrefab;
    public int maxPoolSize;
    public bool collectionChecks;
    private Vector3 spawnPos;
    public List<Sprite> foodSprites;
    public List<string> tags;

    private Sprite active;
    private string tag;

    public static catchManager Instance;

    // setting up the object pool for the food
    public IObjectPool<GameObject> Pool
    {
        get // this can be called to activate all functions that control pool to pool a new food to the world
        {
            if (foodPool == null)
            {
                foodPool = new ObjectPool<GameObject>(CreatePooledItemW, OnTakeFromPoolW, OnReturnedToPoolW, OnDestroyPoolObjectW, collectionChecks, 100, maxPoolSize); // keeps pool size under certain size for memory
            }
            return foodPool;
        }
    }

    private GameObject CreatePooledItemW() // creates the item to be pooled, in this case it is food.
    {
        GameObject food = Instantiate(foodPrefab, spawnPos, Quaternion.LookRotation(Vector3.up));
        food.SetActive(false);
        return food;
    }

    private void OnTakeFromPoolW(GameObject food)//sets food to be true as a spawn when called, also adds to list for iteration
    {
        SpriteRenderer fs = food.GetComponent<SpriteRenderer>();
        fs.sprite = active;
        food.tag = tag;
        food.transform.rotation = Quaternion.Euler(0, 90, 0);
        food.SetActive(true);
    }

    public void OnReturnedToPoolW(GameObject food)// called when food is returned to pool and sets to false, also removes from list 
    {
        food.SetActive(false);
    }

    private void OnDestroyPoolObjectW(GameObject food) // destroys pooled food
    {
        Destroy(food);
    }
    // initialised pool and script instance
    void Awake()
    {
        var _ = Pool;
        Instance = this;
    }

    // called from manager script to provide next spawn position for food
    public void setNextSpawn(Vector3 pos)
    {
        spawnPos = pos;
    }

    public void setType(int rand)
    {
        active = foodSprites[rand];
        tag = tags[rand];
    }
}
