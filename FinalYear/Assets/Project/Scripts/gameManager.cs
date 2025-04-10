using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera Main;
    public TMP_InputField input;
    public Button button;
    public GameObject playerBase;
    public Image knightImg;
    public Image girlyImg;
    public Image waterImg;
    public GameObject menuScreen;
    public GameObject nfcScreen;
    public GameObject selectScreen;
    public GameObject lockedKnightImg;
    public GameObject lockedGirlyImg;
    public GameObject lockedWaterImg;
    public GameObject starsScreen;
    public AudioSource click;
    public TMP_Text currentStars;

    [Header("Characters")]
    public string character;
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    [Header("Scripts")]
    public NFCManager NFCManager;

    //variables
    private string characterSelected;
    private accountData accountData;
    
    // Start is called before the first frame update
    void Start()
    {
        Main.gameObject.SetActive(true);
#if UNITY_EDITOR // stops developer buttons being in build version
        input.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
#endif
        playerBase.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        accountData = new accountData(0); // this needs later changes to a function that gets from json
        setStar();
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
            lockedKnightImg.SetActive(false);
        }else if (text == "2")
        {
            switchCharacter(water);
            lockedWaterImg.SetActive(false);
        }else if (text == "3")
        {
            switchCharacter(girly);
            lockedGirlyImg.SetActive(false);
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
    // this function triggers when home button is pressed to reload the scene 
    public void backToHomeButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        click.Play();
    }
    // sets temp selection to knight
    public void knightSelect()
    {
        characterSelected = "Knight";
        Debug.Log("Knight selected");
        click.Play();
    }
    //sets temp selection to girly
    public void girlySelect()
    {
        characterSelected = "Girly";
        click.Play();
    }
    //sets temp selection to water
    public void waterSelect()
    {
        characterSelected = "Water";
        click.Play();
    }
    //triggers once button pressed to confirm switch character by taking into account temp selection
    public void confirmSwitch()
    {
        click.Play();
        if (characterSelected == "Knight")
        {
            switchCharacter(knight);
            menuScreen.gameObject.SetActive(false);
        }
        else if (characterSelected == "Girly")
        {
            switchCharacter(girly);
            menuScreen.gameObject.SetActive(false);
        }
        else if (characterSelected == "Water")
        {
            switchCharacter(water);
            menuScreen.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No character selected");
        }
    }
    //this function will enable menu ui
    public void menuOn()
    {
        menuScreen.gameObject.SetActive(true);
        click.Play();
    }
    // this function called on button clicked closes menu window
    public void menuBack()
    {
        menuScreen.gameObject.SetActive(false);
        click.Play();
    }
    // this button call will enable the nfc screen and also start the nfc reader session
    public void unlockCharacterBtn()
    {
        nfcScreen.gameObject.SetActive(true);
        selectScreen.gameObject.SetActive(false);
        click.Play();
        NFCManager.StartNFCReading();
    }
    // this button call will enable the nfc screen and also start the nfc writer session
    public void updateCharacterBtn()
    {
        nfcScreen.gameObject.SetActive(true);
        selectScreen.gameObject.SetActive(false);
        click.Play();
        NFCManager.StartNFCWriting();
    }
    // this button call will enable the select screen
    public void selectCharacterBtn()
    {
        selectScreen.gameObject.SetActive(true);
        nfcScreen.gameObject.SetActive(false);
        click.Play();
    }
    // this button call enables stars menu
    public void starBtn()
    {
        starsScreen.gameObject.SetActive(true);
        click.Play();
    }
    // this button call sets star menu inactive
    public void starBackBtn()
    {
        starsScreen.gameObject.SetActive(false);
        click.Play();
    }

    //this button call checks and sets the current star amounts
    public void setStar()
    {
        currentStars.text = accountData.starCount.ToString();
    }
}
