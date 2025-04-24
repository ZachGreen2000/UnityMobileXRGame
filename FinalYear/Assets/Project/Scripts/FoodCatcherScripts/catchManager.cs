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

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    [Header("Variables")]
    public float health;
    public string currentTarget;
    public List<string> foodTypes;

    private float timer;
    private float spawnInterval = 2f;
    private bool isRound = false;
    private string caughtFood;
    private float targetAmount = 0;
    private float currentAmount= 0;
    private int scoreint;

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
            endScreen.SetActive(true);
            isRound = false;
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

    // this button call is for when the quit button is pressed
    public void quit()
    {
        if (!isRound)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
