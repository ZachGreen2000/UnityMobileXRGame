using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Player Score")]
    public float heightScore;

    [Header("Characters")]
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

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
        getCubePos();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, lastCube.position);
        if (distance < spawnDistance)
        {
            getCubePos();
        }

        heightScore = player.position.y;
    }

    public void getCubePos()
    {
        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);
        float y = heightIncrease * angle;

        angle += 0.1f;
        if (radius <= maxRadius)
        {
            radius += 0.5f;
        }
  
        Vector3 spawnPos = new Vector3 (x, y, z);
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
}
