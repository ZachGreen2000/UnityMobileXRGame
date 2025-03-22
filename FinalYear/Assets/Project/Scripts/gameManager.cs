using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera Main;
    
    // Start is called before the first frame update
    void Start()
    {
        Main.gameObject.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
