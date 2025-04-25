using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class parkourManager : MonoBehaviour
{
    [Header("Variables")]
    public float radius;
    public float maxRadius;
    public float angle;
    public float heightIncrease;
    public Transform player;
    public float spawnDistance;
    public Transform lastCube;
    public List<GameObject> cubeCount;
    public TMP_Text heightText;
    public bool mobileBuild;
    public GameObject joystick;
    public GameObject jumpBtn;
    public Rigidbody rb;

    [Header("Player Score")]
    public float heightScore;

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    [Header("Scripts")]
    public playerController playerController;

    private float playerDamage = 3f;
    private float playerSpeed = 2f;
    private float playerHealth = 4f;
    private string playerLevel;
    private float playerLevelFloat;
    private Animator anim;

    public static parkourManager Instance;

    void Awake()
    {
        Instance = this;
    }
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
        else
        {
            Debug.Log("No active character");
        }
        // sets player stats
        playerLevel = gameManager.CharacterManager.ActiveCharacter.characterLevel;
        float.TryParse(playerLevel, out playerLevelFloat);
        playerDamage = calcStat(playerDamage);
        playerSpeed = calcStat(playerSpeed);
        playerHealth = calcStat(playerHealth);
        playerController.setMovementVariables(playerSpeed);

        // first cube
        getCubePos();

        if (mobileBuild)
        {
            joystick.SetActive(true);
            jumpBtn.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, lastCube.position);
        if (distance < spawnDistance)
        {
            getCubePos();
        }
        if (heightScore < player.position.y)
        {
            heightScore = player.position.y;
            heightText.text = "Height: " + heightScore;
        }
        animationCall();
    }
    // this function uses simple maths to create a procedural spawning system that spawns cubes in a spiral
    // the gap between the cubes increases with each spawn to increase the difficulty of the parkour.
    public void getCubePos()
    {
        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);
        float y = heightIncrease * angle;

        angle += 0.1f;
        if (radius <= maxRadius)
        {
            radius += 4f;
        }
  
        Vector3 spawnPos = new Vector3 (x, y + 0.1f, z);
        cubeManager.Instance.setNextSpawn(spawnPos);
        GameObject cube = cubeManager.Instance.Pool.Get();
        lastCube = cube.transform;
        cubeCount.Add(cube);
    }

    // this function is the same or at least similar to that in gameManager where in which is sets the prefab to the correct pos
    public void setPlayer(GameObject currentChar)
    {
        Debug.Log("Player has no childs");
        GameObject newChar = Instantiate(currentChar, player.position, Quaternion.Euler(0, 90, 0) * player.rotation);
        newChar.transform.SetParent(player);
        if (currentChar == water)
        {
            newChar.transform.localPosition = new Vector3(0f, -2.5f, 0f);
            anim = newChar.GetComponent<Animator>();
        }
        else if (currentChar == girly)
        {
            newChar.transform.localPosition = new Vector3(0f, 0.7f, 0f);
            anim = newChar.GetComponent<Animator>();
        }
        else if (currentChar == knight)
        {
            newChar.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            anim = newChar.GetComponent<Animator>();
        }
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
    // this is called on exit and saves the players new score if its a new personal best
    public void saveScore()
    {
        accountData account = accountData.LoadFromFile();
        if (account.parkourHeight < heightScore)
        {
            account.parkourHeight = heightScore;
        }
        account.SaveToFile();
    }
    // this function will manage the characters animations
    public void animationCall()
    {
        if (playerController.isJumping)
        {
            anim.SetBool("jump", true);
        }else if (rb.velocity.magnitude > 0.1f)
        {
            anim.SetBool("walking", true);
            anim.SetBool("jump", false);
        }else
        {
            anim.SetBool("walking", false);
            anim.SetBool("jump", false);
        }
    }
}
