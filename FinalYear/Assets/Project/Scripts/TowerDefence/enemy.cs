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
        void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
            destroyEnemy();
        }

        public void OnTriggerEnter(Collider obj)
        {
            Debug.Log("Tower Hit");
            if (obj.CompareTag("Tower"))
            {
                towerHealth.damage();
                health = 0;
                Debug.Log("Enemy is dead");
            }
        }

        public void takedamage(int damage)
        {
            health = health - damage;
        }

        public void setStats(int h, int s)
        {
            health = h;
            speed = s;
        }

        public void destroyEnemy()
        {
            if (health == 0)
            {
                enemyManager.OnReturnedToPool(this);
                Debug.Log("Enemy destroyed");
            }
        }
    }
}

