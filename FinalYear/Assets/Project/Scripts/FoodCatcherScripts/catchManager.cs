using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catchManager : MonoBehaviour
{
    [Header("Objects")]
    public Transform player;
    public List<Transform> spawnPoints;

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    private float timer;
    private float spawnInterval = 2f;
    private bool isRound = true;

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
        if (isRound)
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

    }
}
