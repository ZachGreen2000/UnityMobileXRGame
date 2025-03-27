using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class defenceManager : MonoBehaviour
{
    [Header("Game Logic")]
    public float score;
    public float round;
    public float killCount;
    public float neededKills;
    private bool roundFlag = false;
    public float maxEnemies;
    public float spawnTime;

    [Header("Scripts")]
    public enemyManager enemyManager;
    public towerHealth towerHealth;
    public enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        killCount = 0;
        score = 0;
        neededKills = 10;
        maxEnemies = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if(killCount >= neededKills) // detects when the round is complete and sets up for the next round
        {
            round++;
            updateScore();
            roundFlag = false;
            towerHealth.setRound(round); // sets round so tower can indentify what health it should have
            killCount = 0;
            enemyManager.enemyList.Clear();
            setNeededKills();
            enemy.setStats(5, 1);
        }
        if(roundFlag && enemyManager.enemyList.Count <= maxEnemies)
        {
            enemyManager.Spawn(); // calls spawn function in enemyManager to start enemy spawning
        }
    }

    public void setNeededKills() // this is called for needed kills to complete round
    {
        neededKills = neededKills * (round / 2); // calculates needed kills for each round using the round counter for infinite increasing
        maxEnemies = maxEnemies + neededKills + 50;
    }
    // updates kills count based on kills
    public void updateKillCount()
    {
        killCount++;
    }
    //updates score when round is complete
    public void updateScore()
    {
        score = score + 10;
    }
    // this will be calle on button click
    public void onPlay()
    {
        roundFlag = true;
    } 
}
