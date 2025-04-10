using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class towerHealth : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public int type;
    public float round;

    [Header("Scripts")]
    public enemy enemy;
    public enemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        float targetHealth = health / round; // calculates health to be smaller each round
        if (targetHealth > 1) // controls minimum health
        {
            health = targetHealth;
        }else
        {
            health = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            endGame();
        }
    }
    // this function is called from the enemy file when an enemy collides with the tower
    public void damage()
    {
        health--;
    }
    // this is called when the health reaches 0 and ends the game
    public void endGame()
    {
        // game end logic
    }

    public void setRound(float rnd)
    {
        round = rnd;
    }

    public void OnTriggerEnter(Collider obj) // detects collision from enemy and returns enemy to pool
    {
        Debug.Log("Tower Hit");
        if (obj.CompareTag("Enemy"))
        {
            damage();
            enemyManager.enemyPool.Release(obj.GetComponent<enemy>());
        }
    }
}
