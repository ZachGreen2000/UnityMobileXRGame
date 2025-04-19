using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class playerController : MonoBehaviour
{
    [Header("Pick Camera Mode")] // gives ability to chose camera mode for different games
    public bool firstNThirdPerspective;
    public bool topDownPerspective;
    public bool pcInputSystem;
    public bool mobileInputSystem;

    [Header("Player")] // player object
    public GameObject player;

    [Header("Components")] // stores components
    public Rigidbody playerRigidbody;
    public CapsuleCollider playerCapsuleCollider;

    [Header("Cameras")] // camera
    public Camera player1stCamera;
    public Camera player3rdCamera;
    public Camera topDownCamera;
    public float thirdCameraSensitivity;
    public float firstCameraSensitivity;
    [SerializeField] private int activeCamera = 0;
    public float minCameraPitch;
    public float maxCameraPitch;
    [SerializeField] private Vector3 cameraOffset;
    public float cameraCorrectionSpeed;
    private float currentPitchY;
    private float currentPitchX;

    [Header("Movement Variables")] // variables to control movement of player
    public float playerCurrentSpeed;
    public float playerSprintSpeed;
    public float playerCrouchSpeed;
    public float playerRotationSpeed;
    public float playerJumpForce;
    public float playerFallForce;
    public float minVelocityForFall;
    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    [SerializeField] private bool isMovingBack = false; // serialized so they are visible in inspector
    [SerializeField] private int jumpCount;
    [SerializeField] private bool isMoving = false;
    private bool moveForward;
    private bool moveRight;
    private bool moveLeft;
    private bool moveBack;
    private bool moveUp;

    [Header("Misc")] // generic features to be manipulated
    public bool contraintAxis;
    public bool playerGrounded;
    public string tagForGround;
    private Vector3 directionToPlayer;

    [Header("KeyBinds")]
    public string keyForForward;
    public string keyForBackward;
    public string keyForRight;
    public string keyForLeft;
    public string keyForJump;
    public string keyForSprint;
    public string keyForCamChange;

    [Header("Mobile Input System")]
    [SerializeField] private Joystick joystick;
    
    // Start is called before the first frame update
    void Start()
    {
        // get components of player
        if (player != null) 
        {
            playerCapsuleCollider = GetComponent<CapsuleCollider>();
            playerRigidbody = GetComponent<Rigidbody>();

            // locks the rotation of player so they do not 'fall over'
            if (contraintAxis)
            {
                playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
        }
        if ((firstNThirdPerspective && topDownPerspective) || (pcInputSystem && mobileInputSystem)) // checks if more than once perspective or input system is selected for debugging
        {
            Debug.Log("Two perspective or input Booleans selected at once");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            if (player1stCamera != null && player3rdCamera != null && topDownCamera != null)
            {
                if (firstNThirdPerspective)
                {
                    player1stCamera.gameObject.SetActive(false);
                    player3rdCamera.gameObject.SetActive(true);
                    topDownCamera.gameObject.SetActive(false);
                }
                else if (topDownPerspective)
                {
                    player1stCamera.gameObject.SetActive(false);
                    player3rdCamera.gameObject.SetActive(false);
                    topDownCamera.gameObject.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pcInputSystem)
        {
            handlePCInput();
        } else if (mobileInputSystem)
        {
            handleMobileInput();
        }
        if (firstNThirdPerspective)
        {
            cameraMovement();
        } else if (topDownPerspective)
        {
            topDownCamMovement();
        }
    }

    void FixedUpdate() // calls in sync with physics systsyem
    {
        if (firstNThirdPerspective)
        {
            Movement();
            jump();
            adjustSpeedForFall();
        } else if (topDownPerspective)
        {
            topDownMovvement();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagForGround))
        {
            playerGrounded = true;
            jumpCount = 0;
        }
    }

    void handlePCInput() // handles pc input seperately to allow for different builds
    {
        if (Input.GetKey(keyForForward.ToLower())) // Move forward 
        {
            moveForward = true;
        }else { moveForward = false; }
        if (Input.GetKey(keyForBackward.ToLower())) // Move back
        {
            moveBack = true;
        }else { moveBack = false; }
        if (Input.GetKey(keyForLeft.ToLower())) // Move left
        {
            moveLeft = true;
        }else { moveLeft = false; }
        if (Input.GetKey(keyForRight.ToLower())) // Move right
        {
            moveRight = true;
        }else { moveRight = false; }
    }

    void handleMobileInput()
    {
        Vector2 input = joystick.Direction;

        moveForward = input.y > 0.1f;
        moveBack = input.y < -0.1f;
        moveRight = input.x > 0.1f;
        moveLeft = input.x < -0.1f;
    }

    void Movement() // controls vector for player movement
    {
        currentVelocity = playerRigidbody.velocity;

        moveDirection = Vector3.zero; // creates Vector and sets to zero for no movement

        // get the forward angle of the camera for player rotation when initial movement from still
        Vector3 cameraForward = player3rdCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        // get the right angle of camera for player rotation 
        Vector3 cameraRight = player3rdCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        // get players forward direction
        Vector3 playerForward = player.transform.forward;
        playerForward.Normalize();

        // Check for direction and set moveDirection accordingly
        if (moveForward) // Move forward 
        {
            moveDirection += cameraForward;
            isMovingBack = false;
        }
        if (moveBack) // Move back
        {
            moveDirection -= cameraForward;
            isMovingBack = true;
        }
        if (moveLeft) // Move left
        {
            moveDirection -= cameraRight;
            isMovingBack = false;
        }
        if (moveRight) // Move right
        {
            moveDirection += cameraRight;
            isMovingBack = false;
        }

        moveDirection = moveDirection.normalized; // avoids diagonal speed boost

        if (moveDirection != Vector3.zero && activeCamera != 1) // rotates player based on direction moving in
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(keyForSprint.ToLower()) && !isMovingBack) // checks player is not moving backwards
        {
            playerRigidbody.velocity = (moveDirection * playerSprintSpeed * Time.fixedDeltaTime + Vector3.up * currentVelocity.y); // adds movement to rigidbody using velocity
        } else
        {
            playerRigidbody.velocity = (moveDirection * playerCurrentSpeed * Time.fixedDeltaTime + Vector3.up * currentVelocity.y); // adds movement to rigidbody using velocity
        }

        if (playerRigidbody.velocity == new Vector3(0,0,0)) // checks wether the player is moving and changes a bool to be used as a flag for other methods
        {
            isMoving = false;
        }else
        {
            isMoving = true;
        }
    }

    void jump() // Handles logic for jump by adding force to rigid body as long as charcter is grounded
    {
       if (Input.GetKeyDown(keyForJump.ToLower()) && playerGrounded)
        {
            playerRigidbody.velocity = (Vector3.up * playerJumpForce); // applies force upwards for jump making use of impulse mode for quick pulse upwards
            playerGrounded = false;
            jumpCount++;
        }else if (Input.GetKeyDown(keyForJump.ToLower()) && !playerGrounded && jumpCount < 2) // checks if character in the air and if the jump count is below 2 for double jump
        {
            playerRigidbody.velocity = (Vector3.up * playerJumpForce); 
            jumpCount++;
        }
    }

    void adjustSpeedForFall() // adds downward force using local gravity to make player fall faster using force mode acceleration to make player feel heavier
    {
        if (playerRigidbody.velocity.y < minVelocityForFall)
        {
            playerRigidbody.AddForce(Vector3.up * Physics.gravity.y * (playerFallForce - 1) * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    void cameraMovement() // function for controlling camera movement
    {
        if (Input.GetKey(keyForCamChange.ToLower())) // switches between third and first perspective
        {
            if (activeCamera == 0)
            {
                activeCamera = 1;
                player1stCamera.gameObject.SetActive(true);
                player3rdCamera.gameObject.SetActive(false);
            } else
            {
                activeCamera = 0;
                player1stCamera.gameObject.SetActive(false);
                player3rdCamera.gameObject.SetActive(true);
            }
        }

        float mouseX = Input.GetAxis("Mouse X"); // gets input of mouse movement
        float mouseY = Input.GetAxis("Mouse Y");

        // calculating rotations
        float rotationX = mouseX * thirdCameraSensitivity * Time.deltaTime;
        float rotationY = -mouseY * thirdCameraSensitivity * Time.deltaTime;
        float rotation1stX = mouseX * firstCameraSensitivity * Time.deltaTime;
        float rotation1stY = mouseY * firstCameraSensitivity * Time.deltaTime;

        Vector3 playerPosition = player.transform.position; // takes the players position and stores it in a vector
        Vector3 currentCamPos = player3rdCamera.transform.position; // gets current camera position

        // clamped camera for when player is moving
        currentPitchY = rotationY;
        currentPitchX = rotationX;
        float clampedCameraX = Mathf.Clamp(currentPitchY, minCameraPitch, maxCameraPitch);
        float clampedCameraY = Mathf.Clamp(currentPitchX, minCameraPitch, maxCameraPitch); 

        // direction of player
        directionToPlayer = playerPosition - currentCamPos;
        directionToPlayer.Normalize(); // Normalize to prevent scaling issues
        Quaternion lookAtPlayer = Quaternion.LookRotation(directionToPlayer);

        if (activeCamera == 1) // checks which perspective is active
        {
            player.transform.Rotate(0, rotation1stX, 0); // for first person
        }else // for third person
        {
            if (!isMoving) // rotate camera around when character not moving for entire view of character
            {
                player3rdCamera.transform.RotateAround(playerPosition, Vector3.up, rotationX);
                player3rdCamera.transform.rotation = Quaternion.Slerp(player3rdCamera.transform.rotation, lookAtPlayer, cameraCorrectionSpeed * Time.deltaTime);
                cameraOffset = playerPosition - currentCamPos; // calculates the offset for when moving
            }
            else if (isMoving) // more natural camera movement for when player is in action
            {
                // ----- this is the problem? --- camera not clamping, set rotation of character to match x when moving, stop camera rolling on Z ///
                Vector3 desiredCamPos = playerPosition - cameraOffset; // calculates camera position for when following
                player3rdCamera.transform.position = Vector3.Lerp(currentCamPos, desiredCamPos, cameraCorrectionSpeed * Time.deltaTime); // for camera follow
                player3rdCamera.transform.Rotate(clampedCameraX, clampedCameraY, 0); // applies rotation on x and y axis in bounds of clamping
            }
        }
    }
    // top down code
    void topDownMovvement()
    {
        currentVelocity = playerRigidbody.velocity;

        moveDirection = Vector3.zero; // creates Vector and sets to zero for no movement

        // get the forward angle of the camera for player rotation when initial movement from still
        Vector3 cameraForward = topDownCamera.transform.up;
        cameraForward.y = 0;
        cameraForward.Normalize();
        // get the right angle of camera for player rotation 
        Vector3 cameraRight = topDownCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        // get players forward and right direction
        Vector3 playerForward = player.transform.forward;
        playerForward.Normalize();
        Vector3 playerRight = player.transform.right;
        playerRight.Normalize();

        // Check for input and set moveDirection accordingly
        if (Input.GetKey(keyForForward.ToLower())) // Move forward 
        {
            moveDirection += cameraForward;
            isMovingBack = false;
        }
        if (Input.GetKey(keyForBackward.ToLower())) // Move back
        {
            moveDirection -= cameraForward;
            isMovingBack = true;
        }
        if (Input.GetKey(keyForLeft.ToLower())) // Move left
        {
            moveDirection -= cameraRight;
            isMovingBack = false;
        }
        if (Input.GetKey(keyForRight.ToLower())) // Move right
        {
            moveDirection += cameraRight;
            isMovingBack = false;
        }

        moveDirection = moveDirection.normalized; // avoids diagonal speed boost

        // rotates player based on direction moving in
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(keyForSprint.ToLower()) && !isMovingBack) // checks player is not moving backwards
        {
            playerRigidbody.velocity = (moveDirection * playerSprintSpeed * Time.fixedDeltaTime + Vector3.up * currentVelocity.y); // adds movement to rigidbody using velocity
        }
        else
        {
            playerRigidbody.velocity = (moveDirection * playerCurrentSpeed * Time.fixedDeltaTime + Vector3.up * currentVelocity.y); // adds movement to rigidbody using velocity
        }

        if (playerRigidbody.velocity == new Vector3(0, 0, 0)) // checks wether the player is moving and changes a bool to be used as a flag for other methods
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    void topDownCamMovement()
    {
        Vector3 playerPosition = player.transform.position; // takes the players position and stores it in a vector
        Vector3 currentCamPos = topDownCamera.transform.position; // gets current camera position
        if (!isMoving)
        {
            cameraOffset = playerPosition - currentCamPos; // calculates the offset for when moving
        } else
        {
            Vector3 desiredCamPos = playerPosition - cameraOffset; // calculates camera position for when following
            topDownCamera.transform.position = desiredCamPos;
        }
    }
}
