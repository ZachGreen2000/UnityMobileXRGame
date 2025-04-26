using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointTrigger : MonoBehaviour
{
    public petMovement petMovement;
    public homePlayer homePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Pet"))
        {
            //petMovement.changeDestination();           
        }else
        {
            homePlayer.stopWalking();
        }
    }
}
