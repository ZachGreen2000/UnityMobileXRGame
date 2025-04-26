using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class petMovement : MonoBehaviour
{
    public GameObject[] wayPoints;
    public float speed;
    private GameObject target;
    private Vector3 offset = new Vector3(1f, 0f, 1f);

    private float fixedHeight;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, wayPoints.Length);
        target = wayPoints[rand];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position + offset;
        fixedHeight = this.transform.position.y;
        targetPos.y = fixedHeight;
        if (Vector3.Distance(this.transform.position, targetPos) > 0.1f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
            
            Vector3 lookDirection = target.transform.position - this.transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                targetRotation *= Quaternion.Euler(0f, 90f, 0f);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
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
