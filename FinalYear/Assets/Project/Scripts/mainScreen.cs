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

    [Header("UI")]
    public GameObject homeScreen;
    public Button playButton;

    [Header("CameraLocations")]
    [SerializeField] private Vector3 startLocation;
    public Vector3 endLocation;

    [Header("CameraVariables")]
    public float correctionSpeed;

    [Header("Scripts")]
    public homePlayer homePlayer;

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
        if (!isMoving) // checks camera still before activating and stating coroutine
        {
            homePlayer.moveToWayPoint();
            homeScreen.SetActive(false);
            StartCoroutine(moveCamera());
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

    void starsClicked() // activates on stars button clicked
    {

    }

    void shopClicked() // activates on shop button clicked
    {

    }

    void menuClicked() // activates on menu button clicked
    {

    }
     // Pet movement
    void petAiMovement()
    {

    }
}
