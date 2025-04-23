using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class miniGameTrigger : MonoBehaviour
{
    public GameObject miniGameTower;
    public GameObject miniGameParkour;
    public GameObject miniGameFood;
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
                colliderable = false;
                if (this.gameObject.CompareTag("tower"))
                {
                    miniGameTower.gameObject.SetActive(true);
                }
                else if (this.gameObject.CompareTag("foodCatch"))
                {
                    miniGameFood.gameObject.SetActive(true);
                }
                else
                {
                    miniGameParkour.gameObject.SetActive(true);
                }
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
        miniGameFood.gameObject.SetActive(false);
        miniGameParkour.gameObject.SetActive(false);
        miniGameTower.gameObject.SetActive(false);
    }
}
