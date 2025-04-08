using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class miniGameTrigger : MonoBehaviour
{
    public GameObject miniGames;
    private bool colliderable = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider obj)
    {
        Debug.Log("Trigger zone reached");
        if (colliderable)
        {
            if (obj.gameObject.CompareTag("Player"))
            {
                miniGames.gameObject.SetActive(true);
                colliderable = false;
            }
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        Debug.Log("Trigger zone exit");
        if (!colliderable)
        {
            colliderable = true;
        }
    }

    public void confirm()
    {
        if (this.gameObject.CompareTag("tower"))
        {
            SceneManager.LoadScene("topDownGame");
        }else if (this.gameObject.CompareTag("foodCatch"))
        {
            SceneManager.LoadScene("foodCatcher");
        }else
        {
            SceneManager.LoadScene("Parkour");
        }
    }

    public void cancel()
    {
        miniGames.gameObject.SetActive(false);
    }
}
