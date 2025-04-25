using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class accountData
{
    public int starCount;
    public float defenceHighRound;
    public float defenceHighScore;
    public int foodHighScore;
    public float parkourHeight;

    public accountData(int starCount, float defenceHighRound, float defenceHighScore, int foodHighScore, float parkourHeight)
    {
        this.starCount = starCount;
        this.defenceHighRound = defenceHighRound;
        this.defenceHighScore = defenceHighScore;
        this.foodHighScore = foodHighScore;
        this.parkourHeight = parkourHeight;
    }
    // the following code is to save and pull to and from json respectively
    // it works by converting the data from json to account data or the opposite
    // the functions can read or write to json and has the option to create a new json file if one doesnt exist
    public string ToJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    public static accountData FromJson(string json)
    {
        return JsonUtility.FromJson<accountData>(json);
    }

    public void SaveToFile()
    {
        string path = Application.persistentDataPath + "/accountData.json";
        File.WriteAllText(path, ToJson());
        Debug.Log("Account data saved to: " + path);
    }

    public static accountData LoadFromFile()
    {
        string path = Application.persistentDataPath + "/accountData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return FromJson(json);
        }
        else
        {
            Debug.Log("No file, creating new file");
            return CreateDefaultFile();
        }
    }

    public static accountData CreateDefaultFile()
    {
        accountData defaultData = new accountData(0, 0, 0, 0, 0);
        defaultData.SaveToFile();
        return defaultData;
    }
}
