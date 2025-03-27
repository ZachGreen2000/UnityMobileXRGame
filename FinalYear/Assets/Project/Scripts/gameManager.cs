using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera Main;

    [Header("Characters")]
    public int character;
    
    // Start is called before the first frame update
    void Start()
    {
        Main.gameObject.SetActive(true);
        
    }

    void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
