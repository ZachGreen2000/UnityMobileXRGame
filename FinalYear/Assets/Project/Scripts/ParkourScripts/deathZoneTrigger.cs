using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider obj)
    {
        parkourManager.Instance.saveScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
