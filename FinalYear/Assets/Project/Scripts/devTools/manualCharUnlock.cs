using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class manualCharUnlock
{
    public static void unlockCharacter(string characterID)
    {
        string knightPath = Application.persistentDataPath + "/knightUnlock.json";
        string waterPath = Application.persistentDataPath + "/waterUnlock.json";
        string girlyPath = Application.persistentDataPath + "/girlyUnlock.json";

        if (characterID == "1")
        {
            string jsonData = LoadFromFile(knightPath);
            characterSingleton character = JsonUtility.FromJson<characterSingleton>(jsonData);
            characterData database = characterData.LoadFromFile();
            database.UnlockCharacter(character);
        }else if (characterID == "2")
        {
            string jsonData = LoadFromFile(waterPath);
            characterSingleton character = JsonUtility.FromJson<characterSingleton>(jsonData);
            characterData database = characterData.LoadFromFile();
            database.UnlockCharacter(character);
        }
        else if (characterID == "3")
        {
            string jsonData = LoadFromFile(girlyPath);
            characterSingleton character = JsonUtility.FromJson<characterSingleton>(jsonData);
            characterData database = characterData.LoadFromFile();
            database.UnlockCharacter(character);
        }
    }

    public static string LoadFromFile(string path)
    {
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            return json;
        }
        else
        {
            Debug.Log("No character data found");
            return null;
        }
    }
}
