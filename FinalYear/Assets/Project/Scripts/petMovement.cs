using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petMovement : MonoBehaviour
{
    public GameObject[] wayPoints;
    public float speed;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, wayPoints.Length);
        target = wayPoints[rand];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Waypoint"))
        {
            int rand = Random.Range(0, wayPoints.Length);
            target = wayPoints[rand];
        }
    }
}
