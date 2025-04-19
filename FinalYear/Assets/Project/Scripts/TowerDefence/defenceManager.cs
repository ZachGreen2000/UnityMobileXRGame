using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using TMPro;
using UnityEngine.SceneManagement;

public class defenceManager : MonoBehaviour
{
    [Header("Game Logic")]
    public float score;
    public float round;
    public float killCount;
    public float neededKills;
    private bool roundFlag = false;
    public float maxEnemies;
    public GameObject playerBase;
    private string currentChar;
    public float spawnInterval;
    private float spawnTimer;
    public float enemyCount;

    [Header("Scripts")]
    public enemyManager enemyManager;
    public towerHealth towerHealth;
    public enemy enemy;
    public playerAttack playerAttack;

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text roundText;
    public GameObject confirmation;
    public TMP_Text roundStart;
    public TMP_Text roundEnd;
    public GameObject endScreen;
    public TMP_Text endRound;
    public TMP_Text endScore;
    public TMP_Text highRound;
    public TMP_Text highScore;
    public TMP_Text enemiesStopped;
    public GameObject joystick;

    [Header("Audio")]
    public AudioSource backingtrack;
    public AudioSource click;
    public AudioSource celebration;

    public static defenceManager Instance; // static reference for global use
 
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        towerHealth.setRound(round);
        killCount = 0;
        score = 0;
        neededKills = 10;
        maxEnemies = 50;
        enemyCount = 0;
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
        scoreText.text = ("Score: " + score);
        roundText.text = ("Round: " + round);
        enemiesStopped.text = ("Enemies Stopped: " + killCount);
#if UNITY_ANDROID
        joystick.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if(killCount >= neededKills) // detects when the round is complete and sets up for the next round
        {
            round++;
            updateScore();
            clearEnemies();
            roundFlag = false;
            towerHealth.setRound(round); // sets round so tower can indentify what health it should have
            killCount = 0;
            enemyCount = 0;
            enemyManager.enemyList.Clear();
            setNeededKills();
            if (spawnInterval >= 1)
            {
                spawnInterval = spawnInterval - 1;
            }
            roundEnd.gameObject.SetActive(true);
            StartCoroutine(PopUp(roundEnd.gameObject));
        }
        if (roundFlag)
        {
            spawnTimer += Time.deltaTime; // adds time each frame
            if (spawnTimer >= spawnInterval && enemyCount <= maxEnemies)
            {
                enemyManager.Spawn(); // calls spawn function in enemyManager to start enemy spawning
                spawnTimer = 0f;           
            }
        }
    }
    public void clearEnemies() // called at fail or round end to wipe remaining enemies
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.activeSelf)
            {
                enemyManager.Pool.Release(obj.GetComponent<enemy>());
            }
        }
        backingtrack.Stop();
    }
    public void updateEnemyCount() // updates enemyCount when emenies spawn
    {
        enemyCount++;
    }   
    public void setNeededKills() // this is called for needed kills to complete round
    {
        neededKills = neededKills * (round / 2); // calculates needed kills for each round using the round counter for infinite increasing
        maxEnemies = maxEnemies + neededKills + 50;
    }
    // updates kills count based on kills
    public void updateKillCount()
    {
        Debug.Log("updating kill count and score");
        killCount++;
        Debug.Log("Killcount: " + killCount);
        updateScore();
    }
    //updates score when round is complete
    public void updateScore()
    {
        score = score + 3;
        Debug.Log("Score is now: " + score);
        // changes text display on screen
        scoreText.text = ("Score: " + score);
        roundText.text = ("Round: " + round);
        enemiesStopped.text = ("Enemies Stopped: " + killCount);
    }
    // this will be calle on button click
    public void onPlay()
    {
        click.Play();
        backingtrack.Play();
        roundFlag = true;
        roundStart.gameObject.SetActive(true);
        StartCoroutine(PopUp(roundStart.gameObject));
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
            playerAttack.setComponents();
        }
        else if (currentChar == girly)
        {
            newChar.transform.localPosition = new Vector3(0f, 6f, 0f);
            playerAttack.setComponents();
        }
        else if (currentChar == knight)
        {
            newChar.transform.localPosition = new Vector3(0f, 4f, 0f);
            playerAttack.setComponents();
        }
    }
    // this button call is for when the user wants to quit the game
    public void onQuit()
    {
        if (!roundFlag)
        {
            click.Play();
            confirmation.SetActive(true);
        }
    }
    // this button call allows a user to reverse their choice to quit
    public void onCancel()
    {
        click.Play();
        confirmation.SetActive(false);
    }
    // this button call allows the user to confirm the quit
    public void onConfirmQuit()
    {
        click.Play();
        SceneManager.LoadScene("Main");
    }
    // this will run for the pop ups and deavtivate them after a while
    IEnumerator PopUp(GameObject type)
    {
        yield return new WaitForSeconds(2f);
        type.SetActive(false);
    }

    public void endGame()
    {
        clearEnemies();
        endScreen.SetActive(true);
        celebration.Play();
        Time.timeScale = 0;
        accountData account = accountData.LoadFromFile();
        endRound.text = ("Final Round: " + round);
        endScore.text = ("Final Score: " + score);
        if (account.defenceHighRound < round)
        {
            account.defenceHighRound = round;
        }
        if (account.defenceHighScore < score)
        {
            account.defenceHighScore = score;
        }
        highRound.text = ("Personal high Round: " + account.defenceHighRound);
        highScore.text = ("Personal High Score" + account.defenceHighScore);
        account.SaveToFile();
    }

    public void endScreenTryAgain()
    {
        click.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void endScreenQuit()
    {
        click.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene("main");
    }
}


