using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parkourManager : MonoBehaviour
{
    [Header("Variables")]
    public float radius;
    public float angle;
    public float heightIncrease;
    public Transform player;
    public float spawnDistance;
    public Transform lastCube;

    public static parkourManager Instance;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        getCubePos();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, lastCube.position);
        if (distance > spawnDistance)
        {
            getCubePos();
        }
    }

    public void getCubePos()
    {
        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);
        float y = heightIncrease * angle;

        angle += 0.1f;
        radius += 0.05f;

        Vector3 spawnPos = new Vector3 (x, y, z);
        cubeManager.Instance.setNextSpawn(spawnPos);
        GameObject cube = cubeManager.Instance.Pool.Get();
        lastCube = cube.transform;
    }
}
