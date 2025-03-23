using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class characterData
{
    public List<characterSingleton> characters = new List<characterSingleton> (); // creates list to handle multiple characters
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
            return new characterData();
        }
    }

    // this function allows the adding of a new character but checks if character exists first
    public void UnlockCharacter(characterSingleton newCharacter)
    {
        if (!characters.Exists(c => c.characterID == newCharacter.characterID)) // c is lambda expression that is used to loop through json objects to find ID
        {
            characters.Add(newCharacter);
            SaveToFile();
            Debug.Log("New character unlocked");
        } else
        {
            Debug.Log("Character already unlocked");
        }
    }
}
