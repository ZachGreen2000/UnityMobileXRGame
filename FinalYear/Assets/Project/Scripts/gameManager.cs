using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.EventSystems;

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
    public GameObject joystick;
    public GameObject homeScreen;
    public GameObject selectionBorder;
    public GameObject accountStatsScreen;
    public GameObject settingsScreen;
    public TMP_Text starCountm;
    public TMP_Text defenceRoundStat;
    public TMP_Text defenceScoreStat;
    public TMP_Text foodCatchScoreStat;
    public TMP_Text parkourScoreStat;
    public List<Transform> petWayPoints;

    [Header("Characters")]
    public string character;
    public GameObject knight;
    public GameObject water;
    public GameObject girly;

    [Header("Mini Pets")]
    public GameObject miniKnight;
    public GameObject miniWater;
    public GameObject miniGirly;

    [Header("Scripts")]
    public NFCManager NFCManager;

    [Header("Variables")]
    public int currentStarStore;
    public float idleTimeThreshold;

    //variables
    private string characterSelected;
    private accountData account;
    private characterData charData;
    private string currentCharacterID;
    private List<string> unlockedCharacterIDs = new List<string>();
    private float lastInputTime;

    // this is a global class for the active characters data to use for updating variables and overwritting to list
    // this creates a temporary instance of the characterSingleton data class that will act as the current character
    public static class CharacterManager
    {
        public static characterSingleton ActiveCharacter = new characterSingleton("","","","","","","");
        public static string ToJson() { return JsonUtility.ToJson(ActiveCharacter, true); } // called for nfc writing
    }
    // Start is called before the first frame update
    void Start()
    {
        Main.gameObject.SetActive(true);
#if UNITY_EDITOR // stops developer buttons being in build version
        input.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
#endif
        playerBase.transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        // this loads the data from the account json file for the star count for the user
        account = accountData.LoadFromFile(); 
        currentStarStore = account.starCount;
        // this loads the data from the character data file for which characters are unlocked and their stats
        charData = characterData.LoadFromFile();
        currentCharacterID = charData.currentCharacterID;
        // the below called both get the current character or character last used and load which characters are unlocked
        detectCurrentChatacter();
        checkForUnlockedCharacters();
        miniPets();
    }
    // called for instantiating correct character based on current Id retrieved from saved data
    public void detectCurrentChatacter()
    {
        if (currentCharacterID == "1")
        {
            Debug.Log("current character is: Knight");
            switchCharacter(knight);
            charData.SetCurrentCharacter("1");
            selectionBorder.SetActive(true);
            selectionBorder.transform.position = lockedKnightImg.transform.position;
        }
        else if (currentCharacterID == "2")
        {
            Debug.Log("current character is: Water");
            switchCharacter(water);
            charData.SetCurrentCharacter("2");
            selectionBorder.SetActive(true);
            selectionBorder.transform.position = lockedWaterImg.transform.position;
        }
        else if (currentCharacterID == "3")
        {
            Debug.Log("current character is: Girly");
            switchCharacter(girly);
            charData.SetCurrentCharacter("3");
            selectionBorder.SetActive(true);
            selectionBorder.transform.position = lockedGirlyImg.transform.position;
        }
        else
        {
            Debug.Log("No character found");
        }
    }
    // this will be called to change current character ID when user is switched characters
    public void setCurrentCharacterID(string ID)
    {
        currentCharacterID = ID;
        detectCurrentChatacter();
    }
    // this function will check which characters are unlocked already at the start of the game for display in swap char screen
    public void checkForUnlockedCharacters()
    {
        Debug.Log("Checking");
        unlockedCharacterIDs.Clear();
        // dictionary for storage allows for easy scale for future characters
        Dictionary<string, GameObject> lockedImages = new Dictionary<string, GameObject>
        {
            {"1", lockedKnightImg},
            {"2", lockedWaterImg},
            {"3", lockedGirlyImg}
        };
        bool anyUnlocked = false;
        //iterates through to deactive locked wall
        foreach(var character in charData.characters)
        {
            Debug.Log("Iterating through data");
            string id = character.characterID;
            unlockedCharacterIDs.Add(id);
            if (lockedImages.ContainsKey(id))
            {
                Debug.Log("Character unlocked: " + id);
                lockedImages[id].SetActive(false);
                anyUnlocked = true;
            }else
            {
                Debug.Log("Character id isnt in dictionairy");
            }
        }
        if (!anyUnlocked)
        {
            Debug.Log("No characters unlocked yet");
        }
    }

    void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        // checks for ui canvas events
        if (canvasEvent())
        {
            lastInputTime = Time.time; // resets timer
        }
        if (Time.time - lastInputTime > idleTimeThreshold && homeScreen.activeSelf) // after enough time passes the idle random is played
        {
            idleRandomPlay();
            lastInputTime = Time.time; // reset timer
        }
    }
    // this is called on a button only available in the unity editor for the development process
    // this function also calls the loading json functions for manual character unlock to simulate the nfc scanning when in dev mode
    public void devButtonClick()
    {
        string text = input.text;
        if (text == "1")
        {
            switchCharacter(knight);
            manualCharUnlock.unlockCharacter("1");
            lockedKnightImg.SetActive(false);
        }else if (text == "2")
        {
            switchCharacter(water);
            manualCharUnlock.unlockCharacter("2");
            lockedWaterImg.SetActive(false);
        }else if (text == "3")
        {
            switchCharacter(girly);
            manualCharUnlock.unlockCharacter("3");
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
        click.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // sets temp selection to knight
    public void knightSelect()
    {
        characterSelected = "Knight";
        Debug.Log("Knight selected");
        click.Play();
        selectionBorder.transform.position = lockedKnightImg.transform.position;
    }
    //sets temp selection to girly
    public void girlySelect()
    {
        characterSelected = "Girly";
        click.Play();
        selectionBorder.transform.position = lockedGirlyImg.transform.position;
    }
    //sets temp selection to water
    public void waterSelect()
    {
        characterSelected = "Water";
        click.Play();
        selectionBorder.transform.position = lockedWaterImg.transform.position;
    }
    //triggers once button pressed to confirm switch character by taking into account temp selection
    public void confirmSwitch()
    {
        click.Play();
        if (characterSelected == "Knight")
        {
            setCurrentCharacterID("1");
            selectScreen.gameObject.SetActive(false);
            menuScreen.gameObject.SetActive(false);
        }
        else if (characterSelected == "Girly")
        {
            setCurrentCharacterID("3");
            selectScreen.gameObject.SetActive(false);
            menuScreen.gameObject.SetActive(false);
        }
        else if (characterSelected == "Water")
        {
            setCurrentCharacterID("2");
            selectScreen.gameObject.SetActive(false);
            menuScreen.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No character selected");
        }
        homeScreen.gameObject.SetActive(true);
        miniPets();
    }
    //this function will enable menu ui
    public void menuOn()
    {
        menuScreen.gameObject.SetActive(true);
        homeScreen.gameObject.SetActive(false);
        click.Play();
        detectCurrentChatacter();
    }
    // this function called on button clicked closes menu window
    public void menuBack()
    {
        menuScreen.gameObject.SetActive(false);
        nfcScreen.gameObject.SetActive(false);
        selectScreen.gameObject.SetActive(false);
        accountStatsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        homeScreen.gameObject.SetActive(true);
        click.Play();
        charData = characterData.LoadFromFile();
        currentCharacterID = charData.currentCharacterID;
        detectCurrentChatacter();
        miniPets();
    }
    // this button call will enable the nfc screen and also start the nfc reader session
    public void unlockCharacterBtn()
    {
        nfcScreen.gameObject.SetActive(true);
        selectScreen.gameObject.SetActive(false);
        accountStatsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        click.Play();
        NFCManager.StartNFCReading();
    }
    // this button call will enable the nfc screen and also start the nfc writer session
    public void updateCharacterBtn()
    {
        nfcScreen.gameObject.SetActive(true);
        selectScreen.gameObject.SetActive(false);
        accountStatsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        click.Play();
        NFCManager.StartNFCWriting();
    }
    // this button call will enable the select screen
    public void selectCharacterBtn()
    {
        charData = characterData.LoadFromFile();
        checkForUnlockedCharacters();
        selectScreen.gameObject.SetActive(true);
        nfcScreen.gameObject.SetActive(false);
        accountStatsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        click.Play();
    }
    // this button enables account stats screen
    public void accountStatsBtn()
    {
        account = accountData.LoadFromFile();
        starCountm.text = "Current Stars: " + account.starCount;
        defenceRoundStat.text = "Highest Round: " + account.defenceHighRound;
        defenceScoreStat.text = "Highest Score: " + account.defenceHighScore;
        foodCatchScoreStat.text = "Highest Score: " + account.foodHighScore;
        parkourScoreStat.text = "Highest Score: " + account.parkourHeight;
        selectScreen.gameObject.SetActive(false);
        nfcScreen.gameObject.SetActive(false);
        settingsScreen.SetActive(false);
        accountStatsScreen.SetActive(true);
    }
    // this button enables the settings screen
    public void settingsScreenBtn()
    {
        selectScreen.gameObject.SetActive(false);
        nfcScreen.gameObject.SetActive(false);
        accountStatsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }
    // this button call enables stars menu
    public void starBtn()
    {
        starsScreen.gameObject.SetActive(true);
        homeScreen.gameObject.SetActive(false);
        setStar();
        click.Play();
    }
    // this button call sets star menu inactive
    public void starBackBtn()
    {
        starsScreen.gameObject.SetActive(false);
        homeScreen.gameObject.SetActive(true);
        click.Play();
    }

    //this button call checks and sets the current star amounts
    public void setStar()
    {
        currentStars.text = currentStarStore.ToString();
        account.starCount = currentStarStore;
        account.SaveToFile(); // saves star count to file each time it changes
    }
    // this is called when the subject score needs updating for a character
    public void saveSubjectScore()
    {
        charData.UpdateCharacterInList(gameManager.CharacterManager.ActiveCharacter);
    }
    // this function will be called when a character is scanned and trigger an animation of said character
    public void playCelebration()
    {
        if (playerBase.transform.childCount > 0)
        {
            Transform currentChild = playerBase.transform.GetChild(0);
            GameObject childObj = currentChild.gameObject;
            if (childObj != null)
            {
                Animator anim = childObj.GetComponent<Animator>();
                anim.SetBool("celebration", true);
                StartCoroutine(stopPlayerCelebration(childObj));
            }
        }
    }
    // stops celebration as to not repeat animation
    IEnumerator stopPlayerCelebration(GameObject childObj)
    {
        yield return new WaitForSeconds(2f);
        Animator anim = childObj.GetComponent<Animator>();
        anim.SetBool("celebration", false);
    }
    // stops idle random animation as to not repeat
    IEnumerator stopPlayerIdleRandom(GameObject childObj)
    {
        yield return new WaitForSeconds(2f);
        Animator anim = childObj.GetComponent<Animator>();
        anim.SetBool("idleRandom", false);
    }

    // this function handles the spawning and mangement of the mini pets in the background
    // only the mini pets that arent current will spawn in and walk around
    public void miniPets()
    {
        if (currentCharacterID != "1")
        {
            int rand = UnityEngine.Random.Range(0, petWayPoints.Count); // unity engine stops ambiguous reference
            Transform spwnPt = petWayPoints[rand];
            Vector3 offset = new Vector3(0f, 1f, 0);
            miniKnight.SetActive(true);
            miniKnight.transform.position = spwnPt.position + offset;
            miniKnight.GetComponent<Animator>().SetBool("walking", true);
        } else
        {
            miniKnight.SetActive(false);
        }
        if (currentCharacterID != "2")
        {
            int rand = UnityEngine.Random.Range(0, petWayPoints.Count);
            Transform spwnPt = petWayPoints[rand];
            Vector3 offset = new Vector3(0f, 1f, 0);
            miniWater.SetActive(true);
            miniWater.transform.position = spwnPt.position + offset;
            miniWater.GetComponent<Animator>().SetBool("walking", true);
        } else
        {
            miniWater.SetActive(false);
        }
        if (currentCharacterID != "3")
        {
            int rand = UnityEngine.Random.Range(0, petWayPoints.Count);
            Transform spwnPt = petWayPoints[rand];
            Vector3 offset = new Vector3(0f, 2f, 0);
            miniGirly.SetActive(true);
            miniGirly.transform.position = spwnPt.position + offset;
            miniGirly.GetComponent<Animator>().SetBool("walking", true);
        } else
        {
            miniGirly.SetActive(false);
        }
        if (currentCharacterID != "1" && currentCharacterID != "2" && currentCharacterID != "3")
        {
            miniKnight.SetActive(false);
            miniWater.SetActive(false);
            miniGirly.SetActive(false);
        }
    }
    // this function runs on call and returns if there has been a canvas event 
    public bool canvasEvent()
    {
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        } else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
    // this function runs an idle random animation that plays if the home screen is active without an event for a set time
    public void idleRandomPlay()
    {
        if (playerBase.transform.childCount > 0)
        {
            Transform currentChild = playerBase.transform.GetChild(0);
            GameObject childObj = currentChild.gameObject;
            if (childObj != null)
            {
                Animator anim = childObj.GetComponent<Animator>();
                anim.SetBool("idleRandom", true);
                StartCoroutine(stopPlayerIdleRandom(childObj));
            }
        }
    }
}
