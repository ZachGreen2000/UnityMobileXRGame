using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitDoor : MonoBehaviour
{
    void OnTriggerEnter(Collider obj)
    {
        parkourManager.Instance.saveScore();
        SceneManager.LoadScene("Main");
    }
}
