using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homePlayer : MonoBehaviour
{

    [Header("Variables")]
    public float speed;
    public float walkSpeed;

    [Header("GameObjects")]
    public GameObject target;
    public Camera mainCam;

    //flags 
    private bool isMoving;
    private bool controllable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (controllable)
        {
            zoomCam();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controllable)
        {
            movement();
        }
    }
    // this is called on play button press and moves character to first waypoint
    public void moveToWayPoint()
    {
        this.transform.rotation = Quaternion.Euler(0f, 90f, 0);
        Transform currentChar = this.transform.GetChild(0);
        GameObject child = currentChar.gameObject;
        Animator anim = child.GetComponent<Animator>();
        anim.SetBool("walking", true);
        StartCoroutine(move(new Vector3(0f, 1f, 0f)));
    }

    IEnumerator move(Vector3 offset) // this controls movement of current pet to play location
    {
        isMoving = true;
        float t = 0f;
        Vector3 endPos = target.transform.position + offset;
        Vector3 startPos = this.transform.position;
        while (t <= 1f) // runs until player in correct position as adding correction speed to t each iteration
        {
            t += Time.deltaTime * speed;
            this.transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(t));
            yield return null;
        }
        Debug.Log("finsihed movement");
        this.transform.position = endPos;
        isMoving = false;
    }

    public void stopWalking() // this stops character walking and forces idle when character is at waypoint
    {
        Transform currentChar = this.transform.GetChild(0);
        GameObject child = currentChar.gameObject;
        Animator anim = child.GetComponent<Animator>();
        anim.SetBool("walking", false);
        controllable = true;
        Debug.Log("Pet is now controllable");
    }
    //this function is for the basic movement on the home screen
    public void movement()
    {
        Vector3 cameraForward = mainCam.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCam.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 currentVelocity = this.gameObject.GetComponent<Rigidbody>().velocity;
        //pc
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) // Move forward 
        {
            moveDirection += cameraForward;
        }
        if (Input.GetKey(KeyCode.S)) // Move back
        {
            moveDirection -= cameraForward;
        }
        if (Input.GetKey(KeyCode.A)) // Move left
        {
            moveDirection -= cameraRight;
        }
        if (Input.GetKey(KeyCode.D)) // Move right
        {
            moveDirection += cameraRight;
        }

        moveDirection = moveDirection.normalized;

        if (moveDirection != Vector3.zero)
        {
            Transform currentChar = this.transform.GetChild(0);
            GameObject child = currentChar.gameObject;
            Animator anim = child.GetComponent<Animator>();
            anim.SetBool("walking", true);

            this.gameObject.GetComponent<Rigidbody>().velocity = (moveDirection * walkSpeed + Vector3.up * currentVelocity.y);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation *= Quaternion.Euler(0, 90, 0);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 10f * Time.deltaTime);
        }else
        {
            Transform currentChar = this.transform.GetChild(0);
            GameObject child = currentChar.gameObject;
            Animator anim = child.GetComponent<Animator>();
            anim.SetBool("walking", false);
        }
        
        //mobile
    }

    public void zoomCam()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        mainCam.fieldOfView -= scroll * 10f;
        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, 20f, 60f);
    }
}
