using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class enemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        public enemy enemyPrefab;
        public int health;
        public GameObject target;
        public int speed;

        [Header("Other")]
        public towerHealth towerHealth;
        public enemyManager enemyManager;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
            
        }

        public void setStats(int h, int s) // this function will set the stats based on the round
        {
            health = health + h;
            speed = speed + s;
        }
    }
}

