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
    public GameObject playerBase;
    private string currentChar;

    [Header("Scripts")]
    public enemyManager enemyManager;
    public towerHealth towerHealth;
    public enemy enemy;

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        killCount = 0;
        score = 0;
        neededKills = 10;
        maxEnemies = 50;
        // calling the set player and passing correct prefab based on active characters ID so character in use is used
        if (gameManager.CharacterManager.ActiveCharacter.characterID == "1")
        {
            setPlayer(knight);
        }else if (gameManager.CharacterManager.ActiveCharacter.characterID == "2")
        {
            setPlayer(water);
        }else if (gameManager.CharacterManager.ActiveCharacter.characterID == "3")
        {
            setPlayer(girly);
        }else
        {
            Debug.Log("No active character");
        }
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
            //enemy.setStats(5, 1);
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
    // this function is the same or at least similar to that in gameManager where in which is sets the prefab to the correct pos
    public void setPlayer(GameObject currentChar)
    {
        Debug.Log("Player has no childs");
        GameObject newChar = Instantiate(currentChar, playerBase.transform.position, Quaternion.Euler(0, 90, 0) * playerBase.transform.rotation);
        newChar.transform.SetParent(playerBase.transform);
        if (currentChar == water)
        {
            newChar.transform.localPosition = new Vector3(0f, -6f, 0f);
        }
        else if (currentChar == girly)
        {
            newChar.transform.localPosition = new Vector3(0f, 8.5f, 0f);
        }
        else if (currentChar == knight)
        {
            newChar.transform.localPosition = new Vector3(0f, 4f, 0f);
        }
    }
}
