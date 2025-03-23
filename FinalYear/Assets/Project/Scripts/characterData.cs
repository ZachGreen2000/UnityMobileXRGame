using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class characterData : MonoBehaviour
{
    public string characterID;
    public string characterName;
    public string characterLevel;
    public string characterBool;
    // data for the current character in use
    public characterData(string id, string name, string level, string unlocked)
    {
        this.characterID = id;
        this.characterName = name;
        this.characterLevel = level;
        this.characterBool = unlocked;
    }
    // convert the character data to json
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
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
            return null;
        }

    }
}
