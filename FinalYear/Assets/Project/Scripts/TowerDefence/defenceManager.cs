using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defenceManager : MonoBehaviour
{
    [Header("Game Logic")]
    public float score;
    public float round;
    public float killCount;
    public float neededKills;
    private bool roundFlag = false;

    [Header("Scripts")]
    public enemyManager enemyManager;
    public towerHealth towerHealth;
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        killCount = 0;
        score = 0;
        neededKills = 10;
    }

    // Update is called once per frame
    void Update()
    {
        neededKills = neededKills * (round / 2); // calculates needed kills for each round using the round counter for infinite increasing
        if(killCount == neededKills) // detects when the round is complete and sets up for the next round
        {
            round++;
            updateScore();
            roundFlag = false;
            towerHealth.setRound(round); // sets round so tower can indentify what health it should have
        }
        if(roundFlag)
        {
            enemyManager.Spawn(); // calls spawn function in enemyManager to start enemy spawning
        }
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
