using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class characterSingleton
{
    public string characterID;
    public string characterName;
    public string characterLevel;
    public string characterBool;
    public string englishScore;
    public string biologyScore;
    public string mathsScore;
    // data for the current character in use
    public characterSingleton(string id, string name, string level, string unlocked, string english, string biology, string maths)
    {
        this.characterID = id;
        this.characterName = name;
        this.characterLevel = level;
        this.characterBool = unlocked;
        this.englishScore = english;
        this.biologyScore = biology;
        this.mathsScore = maths;
    }
    // the below functions are used when saving and retrieving data from NFC tags
    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public static characterSingleton FromJson(string json)
    {
        return JsonUtility.FromJson<characterSingleton>(json);
    }
}
