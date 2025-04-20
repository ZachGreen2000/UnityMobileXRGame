using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class enemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        public float health;
        public GameObject target;
        public float speed;

        //private
        private bool isReleased = false;
        public bool isMoving = true;
        private enemyManager enemyManager;
        private defenceManager defenceManager;

        // injects sccript at runtime
        public void SetupEnemyManager(enemyManager enemyMng)
        {
            enemyManager = enemyMng;
        }
        // injects sccript at runtime
        public void SetUpDefenceManager(defenceManager defMan)
        {
            defenceManager = defMan;
        }

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
            health = h + defenceManager.round;
            speed = s;
            Debug.Log("Speed stat set to: " + speed);
            Debug.Log("Health stat set to: " + health);
            GetComponent<Collider>().enabled = true;
            GetComponent<Animator>().enabled = true;
            isReleased = false;
            isMoving = true;
            Animator anim = this.GetComponent<Animator>();
            anim.SetBool("Moving", true);
        }
        // calls when damaged and returns enemy to pool when needed
        public void DmgNDead(float dmg)
        {
            health -= dmg;
            Debug.Log("Enemy damaged: " + health);
            if (health <= 0)
            {
                defenceManager.updateKillCount();
                enemyManager.Pool.Release(this);
            }
        }
    }
}

