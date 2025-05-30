using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class levelUpScreen : MonoBehaviour
{
    [Header("UI Objects")]
    public Button increase;
    public Button decrease;
    public Button confirm;
    public TMP_Text currentStars;
    public TMP_Text useStars;
    public TMP_Text level;
    public TMP_Text speed;
    public TMP_Text damage;
    public TMP_Text health;

    // data scripts
    private accountData accountData;
    private characterData characterData;

    [Header("GameObjects")]
    public GameObject player;
    public AudioSource click;
    public AudioSource celebration;

    //private
    private string currentLevel = "0";
    private string starsToUse = "0";
    private string speedCalc = "2";
    private string damageCalc = "3";
    private string healthCalc = "4";
    private int tempCurrentStars;
    private int tempStarsToUse;
    private int tempLevel;

    // Start is called before the first frame update
    public void OnLevelScreen()
    {
        accountData = accountData.LoadFromFile();
        characterData = characterData.LoadFromFile();
        currentLevel = gameManager.CharacterManager.ActiveCharacter.characterLevel;
        int.TryParse(currentLevel, out tempLevel);
        setUIText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // this will be called when the level screen is activated to apply starting values for ui text
    public void setUIText()
    {
        currentLevel = gameManager.CharacterManager.ActiveCharacter.characterLevel;
        currentStars.text = accountData.starCount.ToString();
        int.TryParse(currentStars.text, out tempCurrentStars);
        useStars.text = starsToUse;
        tempStarsToUse = 0;
        level.text = currentLevel;
        speed.text = calcStat(speedCalc);
        health.text = calcStat(healthCalc);
        damage.text = calcStat(damageCalc);
    }
    // this will be called to update text when the user is manipulating starts to spend on level up
    public void updateUIText()
    {
        level.text = tempLevel.ToString();
        useStars.text = tempStarsToUse.ToString();
        speed.text = calcStat(speedCalc);
        health.text = calcStat(healthCalc);
        damage.text = calcStat(damageCalc);
    }
    // the purpose of this function is to calculate stats based on the level for ui display
    public string calcStat(string stat)
    {
        Debug.Log("Calculating stat");
        if (stat == speedCalc)
        {
            int statInt;
            int.TryParse(stat, out statInt);
            int calculatedStat = statInt * tempLevel + 1;
            Debug.Log("Speed is:" + calculatedStat);
            return calculatedStat.ToString();
        }
        else if (stat == healthCalc)
        {
            int statInt; 
            int.TryParse(stat, out statInt);
            int levelInt;
            int.TryParse(currentLevel, out levelInt);
            int calculatedStat = statInt * tempLevel + 3;
            Debug.Log("Health is:" + calculatedStat);
            return calculatedStat.ToString();
        }
        else if (stat == damageCalc)
        {
            int statInt;
            int.TryParse(stat, out statInt);
            int levelInt;
            int.TryParse(currentLevel, out levelInt);
            int calculatedStat = statInt * tempLevel + 2;
            Debug.Log("Damage is:" + calculatedStat);
            return calculatedStat.ToString();
        }else
        {
            return null;
        }
    }
    // called on increase star to use arrow to and updates UI to show user the effects of the level increase
    public void increaseStarBtn()
    {
        click.Play();
        if (tempCurrentStars > tempStarsToUse)
        {
            tempStarsToUse++;
            tempLevel++;
            updateUIText();
        }
    }
    // opposite of increase button
    public void decreaseStarBtn()
    {
        click.Play();
        if (tempStarsToUse > 0)
        {
            tempStarsToUse--;
            tempLevel--;
            updateUIText();
        }
    }
    // once this is pressed it confirms the users level up and writes the data to json
    // this function also triggers the character celebration animation
    public void confirmBtn()
    {
        celebration.Play();
        int starUpdate = tempCurrentStars - tempStarsToUse;
        accountData.starCount = starUpdate;
        accountData.SaveToFile();
        gameManager.CharacterManager.ActiveCharacter.characterLevel = tempLevel.ToString();
        characterData.UpdateCharacterInList(gameManager.CharacterManager.ActiveCharacter);
        setUIText();
        Transform currentChar = player.transform.GetChild(0);
        GameObject child = currentChar.gameObject;
        Animator anim = child.GetComponent<Animator>();
        anim.SetBool("celebration", true);
        StartCoroutine(resetCelebration(anim));
    }

    IEnumerator resetCelebration(Animator anim)
    {
        yield return new WaitForSeconds(5f);
        anim.SetBool("celebration", false);
    }
}
