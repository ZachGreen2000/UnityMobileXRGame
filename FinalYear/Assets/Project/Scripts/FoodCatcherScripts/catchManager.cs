using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class catchManager : MonoBehaviour
{
    [Header("Objects")]
    public Transform player;
    public List<Transform> spawnPoints;
    public GameObject endScreen;
    public TMP_Text score;
    public TMP_Text title;
    public TMP_Text healthT;
    public TMP_Text endScore;
    public TMP_Text highScore;
    public GameObject quitOptions;

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    [Header("Variables")]
    public float health;
    public string currentTarget;
    public List<string> foodTypes;

    [Header("Scripts")]
    public playerController playerController;

    private float timer;
    private float spawnInterval = 2f;
    private bool isRound = false;
    private string caughtFood;
    private float targetAmount = 5;
    private float currentAmount= 0;
    private int scoreint;
    private float playerLevelFloat;
    private string playerLevel;
    private float playerSpeed = 2f;
    private float playerHealth = 4f;
    private float playerDamage = 3f;

    public static catchManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        // calling the set player and passing correct prefab based on active characters ID so character in use is used
        if (gameManager.CharacterManager.ActiveCharacter.characterID == "1")
        {
            setPlayer(knight);
        }
        else if (gameManager.CharacterManager.ActiveCharacter.characterID == "2")
        {
            setPlayer(water);
        }
        else if (gameManager.CharacterManager.ActiveCharacter.characterID == "3")
        {
            setPlayer(girly);
        }
        score.text = "Score: " + scoreint.ToString();
        
        // set players stats for miniGame
        playerLevel = gameManager.CharacterManager.ActiveCharacter.characterLevel;
        float.TryParse(playerLevel, out playerLevelFloat);
        playerDamage = calcStat(playerDamage);
        playerSpeed = calcStat(playerSpeed);
        playerHealth = calcStat(playerHealth);
        playerController.setMovementVariables(playerSpeed);
        health = playerHealth;
        healthT.text = "Health: " + health;
    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // within this function food is spawned when round active based on spawn interval
        // then a random int is used to get a random point in the list which is passed as the next spawn point
        if (isRound && currentAmount < targetAmount)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                int randPoint = Random.Range(0, spawnPoints.Count);
                int randPoint2 = Random.Range(0, 5);
                Transform pnt = spawnPoints[randPoint];
                foodManager.Instance.setNextSpawn(pnt.position);
                foodManager.Instance.setType(randPoint2);
                foodManager.Instance.Pool.Get();
                timer = 0f;
            }
        } else if (isRound && currentAmount >= targetAmount)// stops round at target
        {
            scoreint = scoreint + 5;
            score.text = "Score: " + scoreint.ToString();
            isRound = false;
            if (spawnInterval > 1)
            {
                spawnInterval--;
            }
            currentAmount = 0;
            targetAmount++;
        }
        // game fails when player loses all health
        if (health <= 0)
        {
            isRound = false;
            endScreenDisplay();
        }
    }

    // this function is the same or at least similar to that in gameManager where in which is sets the prefab to the correct pos
    public void setPlayer(GameObject currentChar)
    {
        Debug.Log("Player has no childs");
        GameObject newChar = Instantiate(currentChar, player.position, Quaternion.Euler(0, -90, 0) * player.rotation);
        newChar.transform.SetParent(player);
        if (currentChar == water)
        {
            newChar.transform.localPosition = new Vector3(0f, -6f, 0f);

        }
        else if (currentChar == girly)
        {
            newChar.transform.localPosition = new Vector3(0f, 6f, 0f);

        }
        else if (currentChar == knight)
        {
            newChar.transform.localPosition = new Vector3(0f, 1.9f, 0f);

        }
    }

    // this function will handle the caught food and the outcome
    public void foodCaught(string tag)
    {
        caughtFood = tag;
        if (caughtFood == currentTarget)
        {
            currentAmount++;
            scoreint++;
            score.text = "Score: " + scoreint.ToString();
        }
        else
        {
            health--;
            healthT.text = "Health: " + health.ToString();
        }
    }

    // this button call is for when the play button is pressed
    public void play()
    {
        isRound = true;
        int rand = Random.Range(0, foodTypes.Count);
        string type = foodTypes[rand];
        title.text = "Catch Only: " + type;
        currentTarget = type;
    }

    // this button call is called when quit is pressed but only runs when round now playing
    public void initialQuit()
    {
        if (!isRound)
        {
            quitOptions.SetActive(true);
        }
    }

    // this button call is called when the player cancels their quit press
    public void cancelQuit()
    {
        quitOptions.SetActive(false);
    }

    // this button call is for when the quit confirm button is pressed
    public void quit()
    {
        SceneManager.LoadScene("Main");
    }

    // this function when user dies and end screen activates to display their score and show high score and save if new high score
    public void endScreenDisplay()
    {
        endScore.text = "Score: " + scoreint;
        accountData account = accountData.LoadFromFile();
        if (account.foodHighScore < scoreint)
        {
            account.foodHighScore = scoreint;
        }
        highScore.text = "High Score: " + account.foodHighScore;
        account.SaveToFile();
        endScreen.SetActive(true);
    }

    // the purpose of this function is to calculate stats based on the level for tower defence use
    public float calcStat(float stat)
    {
        Debug.Log("Calculating stat");
        if (stat == playerSpeed)
        {
            float calculatedStat = stat * playerLevelFloat + 1;
            Debug.Log("Speed is:" + calculatedStat);
            return calculatedStat;
        }
        else if (stat == playerHealth)
        {
            float calculatedStat = stat * playerLevelFloat + 3;
            return calculatedStat;
        }
        else if (stat == playerDamage)
        {
            float calculatedStat = stat * playerLevelFloat + 2;
            return calculatedStat;
        }
        else
        {
            return 0f;
        }
    }
}
