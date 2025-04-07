using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homePlayer : MonoBehaviour
{

    [Header("Variables")]
    public float speed;

    [Header("GameObjects")]
    public GameObject target;

    //flags 
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToWayPoint()
    {
        this.transform.rotation = Quaternion.Euler(0f, 90f, 0);
        Transform currentChar = this.transform.GetChild(0);
        GameObject child = currentChar.gameObject;
        Animator anim = child.GetComponent<Animator>();
        anim.SetBool("walking", true);
        StartCoroutine(move());
    }

    IEnumerator move() // this controls movement of current pet to play location
    {
        isMoving = true;
        float t = 0f;
        while (t < 1f) // runs until player in correct position as adding correction speed to t each iteration
        {
            t += Time.deltaTime * speed;
            this.transform.position = Vector3.Slerp(this.transform.position, target.transform.position, t);
            yield return null;
        }
        isMoving = false;
    }
}
