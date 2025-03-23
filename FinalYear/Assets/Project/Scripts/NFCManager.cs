using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class NFCManager : MonoBehaviour
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void startScanning();
    [DllImport("__Internal")]
    private static extern void stopScanning();
    [DllImport("__Internal")]
    private static extern void startWriting(string data);
#endif

#if UNITY_ANDROID
    private static AndroidJavaObject NFCAndroidPlugin;
#endif
    public static NFCManager Instance { get; private set; }
    public characterData currentCharacter;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

#if UNITY_ANDROID
        NFCAndroidPlugin = new AndroidJavaClass("com.DefaultCompany.FinalYear.NFCPlugin").CallStatic<AndroidJavaObject>("getInstance", GetUnityActivity());
#endif
    }

#if UNITY_ANDROID
    private static AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.unityPlayer))
        {
            return unityPlayer.GetStatuc<AndroidJavaObject>("currentActivity");
        }
    }
#endif

    // This function, when called, will start the NFC reading for the selected character
    public void StartNFCReading()
    {
        Debug.Log("Starting reading session");
#if UNITY_IOS && !UNITY_EDITOR
        startScanning();
#elif UNITY_ANDROID
        NFCAndroidPlugin.Call("startReading");
#else
        Debug.Log("NFC read only available on mobile");
#endif
    }

    //this function, when called, will start the writing session to update character model with json data
    public void StartNFCWriting()
    {
        Debug.Log("Writting session has begun");

        string jsonData = currentCharacter.ToJson();
#if UNITY_IOS && !UNITY_EDITOR
        startWriting(jsonData);
#elif UNITY_ANDROID
        NFCAndroidPlugin.Call("writeToNFC", jsonData);
#else 
        Debug.Log("NFC write only available on mobile");
#endif
    }

    //this function is called once NFC tag has been read and data retrived
    public void OnNFCRead(string jsonData)
    {
        currentCharacter = characterData.FromJson(jsonData);
        currentCharacter.SaveToFile();
        Debug.Log($"Character data: ID={currentCharacter.characterID}, Name={currentCharacter.characterName}, Level={currentCharacter.characterLevel}, Unlocked={currentCharacter.characterBool}");
    }

    private void OnNFCError(string message)
    {
        Debug.Log("Error: " + message);
    }
}
