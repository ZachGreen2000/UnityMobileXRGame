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
    public defenceManager defenceManager;

    [Header("Audio")]
    public AudioSource boyDmg;
    public AudioSource girlDmg;

    private string currentCharacterID;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentCharacterID = gameManager.CharacterManager.ActiveCharacter.characterID;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && dead == false)
        {
            Debug.Log("Tower is dead: " + health);
            defenceManager.endGame();
            dead = true;
        }
    }
    // this function is called from the enemy file when an enemy collides with the tower
    public void damage()
    {
        health--;
        Debug.Log("Health is now: " + health);
        if (currentCharacterID == "1" || currentCharacterID == "2")
        {
            boyDmg.Play();
        }else if (currentCharacterID == "3")
        {
            girlDmg.Play();
        }
    }

    public void setRound(float rnd)
    {
        round = rnd;
        float targetHealth = health + (defenceManager.playerHealth / round); // calculates health to be smaller each round
        if (targetHealth > 1) // controls minimum health
        {
            health = targetHealth;
        }
        else
        {
            health = 1;
        }
    }

    public void OnTriggerEnter(Collider obj) // detects collision from enemy and returns enemy to pool
    {
        Debug.Log("Tower Hit");
        if (obj.CompareTag("Enemy"))
        {
            damage();
            enemyManager.Pool.Release(obj.GetComponent<enemy>());
        }
    }
}
