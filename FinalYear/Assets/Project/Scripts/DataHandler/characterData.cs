using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class characterData
{
    public List<characterSingleton> characters = new List<characterSingleton> (); // creates list to handle multiple characters
    public string currentCharacterID;
    // convert the character data to json
    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }
    // loads character data from json
    public static characterData FromJson(string json)
    {
        return JsonUtility.FromJson<characterData>(json);
    }
    // save character data to json
    public void SaveToFile()
    {
        string path = Application.persistentDataPath + "/characterData.json";
        System.IO.File.WriteAllText(path, ToJson());
        Debug.Log("Character data saved to: " + path);
    }
    // load character data from json
    public static characterData LoadFromFile()
    {
        string path = Application.persistentDataPath + "/characterData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            return FromJson(json);
        }else
        {
            Debug.Log("No character data found");
            return CreateDefaultFile();
        }
    }

    //creates a default json file when game first loaded
    public static characterData CreateDefaultFile()
    {
        characterData newData = new characterData();
        newData.SaveToFile();
        return newData;
    }

    // this function allows the adding of a new character but checks if character exists first
    public void UnlockCharacter(characterSingleton newCharacter)
    {
        if (!characters.Exists(c => c.characterID == newCharacter.characterID)) // c is lambda expression that is used to loop through json objects to find ID
        {
            characters.Add(newCharacter);
            currentCharacterID = newCharacter.characterID;
            SaveToFile();
            Debug.Log("New character unlocked");
        } else
        {
            Debug.Log("Character already unlocked");
        }
    }

    //this function will retrieve correct character for use in game
    public void SetCurrentCharacter(string characterID)
    {
        if (characters.Exists(c => c.characterID == characterID))
        {
            currentCharacterID = characterID;
            characterSingleton character = characters.Find(c => c.characterID == characterID);
            gameManager.CharacterManager.ActiveCharacter.UpdateFrom(character);
            SaveToFile();
            Debug.Log("Current character set to: " + characterID);
        }
        else
        {
            Debug.Log("Character ID not found.");
        }
    }

    public void UpdateCharacterInList(characterSingleton updatedCharacter)
    {
        int index = characters.FindIndex(c => c.characterID == updatedCharacter.characterID);
        if (index != null)
        {
            characters[index] = new characterSingleton(
                updatedCharacter.characterID,
                updatedCharacter.characterName,
                updatedCharacter.characterLevel,
                updatedCharacter.characterBool,
                updatedCharacter.englishScore,
                updatedCharacter.biologyScore,
                updatedCharacter.mathsScore);
            SaveToFile();
        }
        else
        {
            Debug.Log("Character ID not found!");
        }
    }
}
