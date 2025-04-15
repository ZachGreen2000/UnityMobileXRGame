using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class playerAttack : MonoBehaviour
{
    [Header("Attack Variables")]
    public Transform shootPosition;
    public float range;
    public int damage;
    public int type;
    public float bulletSpeed;
    public LayerMask enemyLayer;

    [Header("Scripts")]
    public enemy enemy;
    public enemyManager enemyManager;
    public defenceManager defenceManager;
    // Start is called before the first frame update
    void Start()
    {
        getTypeAndLevel();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // detects input
        {
            Debug.Log("Shoot");
            attack();
        }
    }

    public void attack() // this function is called on input to use raycast to damage enemies
    {
        RaycastHit hit;

        if(Physics.SphereCast(shootPosition.position, 5, shootPosition.forward, out hit, range, enemyLayer))
        {
            Debug.Log("RayCast hit: " + hit.collider);
            Debug.DrawRay(shootPosition.position, shootPosition.forward * hit.distance, Color.red, 1f);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit is tagged enemy");
                enemy hitEnemy = hit.collider.GetComponentInParent<enemy>();
                if (hitEnemy != null)
                {
                    Debug.Log("Hit enemy is not null");
                    hitEnemy.health -= damage;
                    if (hitEnemy.health <= 0)
                    {
                        hitEnemy.GetComponent<Collider>().enabled = false;
                        hitEnemy.gameObject.SetActive(false);
                        Debug.Log("Hit enemy has no health");
                        enemyManager.Pool.Release(hitEnemy.GetComponent<enemy>());
                        defenceManager.updateKillCount();
                    }
                }
            }
        }
    }

    public void getTypeAndLevel()
    {
        // logic to get current character and current characters stats
    }

}
