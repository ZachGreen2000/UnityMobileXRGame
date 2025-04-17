using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class enemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        public int health;
        public GameObject target;
        public float speed;

        //private
        private bool isReleased = false;
        public bool isMoving = true;
        
        void Start ()
        {
            Animator anim = this.GetComponent<Animator>();
            anim.SetBool("Moving", true);
        }
        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
            }
        }

        // this is called when an emey is taken from the pool
        public void resetStats(int h, float s)
        {
            health = h;
            speed = s;
            Debug.Log("Speed stat set to: " + speed);
            GetComponent<Collider>().enabled = true;
            GetComponent<Animator>().enabled = true;
            isReleased = false;
            isMoving = true;
            Animator anim = this.GetComponent<Animator>();
            anim.SetBool("Moving", true);
        }
    }
}

