using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mainScreen : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera Main;
    public GameObject waypoints;
    public AudioSource click;
    public GameObject joystick;

    [Header("UI")]
    public GameObject homeScreen;
    public Button playButton;
    public Button backToHome;
    public GameObject levelUpScreen;

    [Header("CameraLocations")]
    [SerializeField] private Vector3 startLocation;
    public Vector3 endLocation;
    public Vector3 endLocationLevel;

    [Header("CameraVariables")]
    public float correctionSpeed;

    [Header("Scripts")]
    public homePlayer homePlayer;
    public levelUpScreen levelUpScript;

    // Flags
    private bool isMoving = false;

    // private variables
    private int childCount; // stores value of no. waypoints

    // Start is called before the first frame update
    void Start()
    {
        startLocation = Main.transform.position;

        if (playButton != null)
        {
            playButton.onClick.AddListener(playClicked); // adds listener to button to call function when clicked
        }

        if (waypoints != null) // gets amount of waypoints and stores it
        {
            childCount = waypoints.transform.childCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // UI
    void playClicked() // activates on play button clicked
    {
        click.Play();
        if (!isMoving) // checks camera still before activating and stating coroutine
        {
            homePlayer.moveToWayPoint();
            homeScreen.SetActive(false);
            StartCoroutine(moveCamera());
            backToHome.gameObject.SetActive(true);
#if UNITY_ANDROID 
            joystick.SetActive(true);
#endif
        }
    }

    IEnumerator moveCamera() // moves camera smoothly from one position to the other without update function for performance
    {
        isMoving = true;
        float t = 0f;
        while (t < 1f) // runs until camera in correct position as adding correction speed to t each iteration
        {
            t += Time.deltaTime * correctionSpeed;
            Main.transform.position = Vector3.Slerp(startLocation, endLocation, t);
            yield return null;
        }
        isMoving = false; // sets to false for camera movement again
    }
    // this function is called when level up is clicked and works the same as play button click
    public void levelClicked()
    {
        click.Play();
        if (!isMoving)
        {
            homeScreen.SetActive(false);
            StartCoroutine(moveCamToLevel());
            levelUpScript.OnLevelScreen();
            levelUpScreen.SetActive(true);
            backToHome.gameObject.SetActive(true);
        }
    }
    // this enumeration works the same as above but with different end location for level up screen
    IEnumerator moveCamToLevel()
    {
        isMoving = true;
        float t = 0f;
        while (t < 1f) // runs until camera in correct position as adding correction speed to t each iteration
        {
            t += Time.deltaTime * correctionSpeed;
            Main.transform.position = Vector3.Slerp(startLocation, endLocationLevel, t);
            yield return null;
        }
        isMoving = false; // sets to false for camera movement again
    }
}
