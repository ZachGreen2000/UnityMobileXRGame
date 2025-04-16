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
        public int speed;

        [Header("Other")]
        public towerHealth towerHealth;
        public enemyManager enemyManager;

        //private
        private bool isReleased = false;
        private bool isMoving = true;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        void Update()
        {
           /* if (health <= 0 && !isReleased)
            {
                isReleased = true;
                isMoving = false;
                enemyManager.Pool.Release(this.GetComponent<enemy>());
                Debug.Log("enemy has been hit and is returning to pool");
            }*/
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (isMoving)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
            }
            
            
        }

        public void setStats(int h, int s) // this function will set the stats based on the round
        {
            health = health + h;
            speed = speed + s;
        }
        // this is called when an emey is taken from the pool
        public void resetStats(int h, int s)
        {
            health = h;
            speed = s;
            Debug.Log("Speed stat set to: " + speed);
            GetComponent<Collider>().enabled = true;
            isReleased = false;
            isMoving = true;
        }

        public void damageEnemy()
        {
            Debug.Log("enemy damaged");
            health = health - 1;
            Debug.Log("enemy health: " + health);
            if (health <= 0 && !isReleased)
            {
                isReleased = true;
                isMoving = false;
                enemyManager.Pool.Release(this);
                Debug.Log("enemy has been hit and is returning to pool");
            }
        }
    }
}

