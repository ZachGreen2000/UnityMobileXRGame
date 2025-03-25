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
        private Vector3 pos;
        // Start is called before the first frame update
        void Start()
        {
            Vector3 pos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            pos = Vector3.Lerp(pos, target.transform.position, speed * Time.deltaTime);
        }

        public void OnTriggerEnter(Collider obj)
        {
            if (obj.CompareTag("Tower"))
            {
                towerHealth.damage();
                health = 0;
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
            if (health > 0)
            {
                enemyManager.OnReturnedToPool(this);
            }
        }
    }
}

