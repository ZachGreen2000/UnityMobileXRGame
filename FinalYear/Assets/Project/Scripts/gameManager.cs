using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera Main;
    public TMP_InputField input;
    public Button button;
    public GameObject playerBase;

    [Header("Characters")]
    public string character;
    public GameObject knight;
    public GameObject water;
    public GameObject girly;
    
    // Start is called before the first frame update
    void Start()
    {
        Main.gameObject.SetActive(true);
#if UNITY_EDITOR // stops developer buttons being in build version
        input.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
#endif
    }

    void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    // this is called on a button only available in the unity editor for the development process
    public void devButtonClick()
    {
        string text = input.text;
        if (text == "1")
        {
            switchCharacter(knight);
        }else if (text == "2")
        {
            switchCharacter(water);
        }else if (text == "3")
        {
            switchCharacter(girly);
        }else
        {
            Debug.Log("No character found");
        }
    }
    // this function will control which character is on screen and active for the user to use
    //it gets a player object and instantiates and prefab accoording to which character is selected. It then makes the instantiated model a child of the player
    public void switchCharacter(GameObject currentChar)
    {
        if (playerBase.transform.childCount > 0)
        {
            Transform currentChild = playerBase.transform.GetChild(0);
            GameObject childObj = currentChild.gameObject;
            if (childObj != null)
            {
                Destroy(childObj);
                GameObject newChar = Instantiate(currentChar, playerBase.transform.position, playerBase.transform.rotation);
                newChar.transform.SetParent(playerBase.transform);
                if (currentChar == water) // the following statements adjust position offset to account for misalignment 
                {
                    newChar.transform.localPosition = new Vector3(0f, -9.5f, 0f);
                }else if (currentChar == girly)
                {
                    newChar.transform.localPosition = new Vector3(0f, 8.5f, 0f);
                }else if (currentChar == knight)
                {
                    newChar.transform.localPosition = new Vector3(0f, 4f, 0f);
                }
            }
        }else
        {
            Debug.Log("Player has no childs");
            GameObject newChar = Instantiate(currentChar, playerBase.transform.position, playerBase.transform.rotation);
            newChar.transform.SetParent(playerBase.transform);
            if (currentChar == water)
            {
                newChar.transform.localPosition = new Vector3(0f, -9.5f, 0f);
            }
            else if (currentChar == girly)
            {
                newChar.transform.localPosition = new Vector3(0f, 8.5f, 0f);
            }
            else if (currentChar == knight)
            {
                newChar.transform.localPosition = new Vector3(0f, 4f, 0f);
            }
        }
    }
}
