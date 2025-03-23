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
    // data for the current character in use
    public characterSingleton(string id, string name, string level, string unlocked)
    {
        this.characterID = id;
        this.characterName = name;
        this.characterLevel = level;
        this.characterBool = unlocked;
    }
}
