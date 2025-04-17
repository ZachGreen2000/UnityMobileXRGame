using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class enemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        //public enemy enemyPrefab;
        public int health;
        public GameObject target;
        public float speed;

        [Header("Other")]
        public towerHealth towerHealth;
        public enemyManager enemyManager;

        //private
        private bool isReleased = false;
        public bool isMoving = true;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        void Update()
        {
           
        }
        // Update is called once per frame
        void FixedUpdate()
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
        }
    }
}

