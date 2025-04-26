using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petMovement : MonoBehaviour
{
    public GameObject[] wayPoints;
    public float speed;
    private GameObject target;
    private Vector3 offset = new Vector3(1f, 1f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, wayPoints.Length);
        target = wayPoints[rand];
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position != target.transform.position + offset)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
            this.transform.LookAt(target.transform);
        } else
        {
            changeDestination();
        }
    }

    public void changeDestination()
    {
        int rand = Random.Range(0, wayPoints.Length);
        target = wayPoints[rand];  
    }
}
