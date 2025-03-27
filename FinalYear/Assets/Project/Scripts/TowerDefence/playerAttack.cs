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

    [Header("Scripts")]
    public enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        getTypeAndLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Shoot");
            attack();
        }
    }

    public void attack()
    {
        RaycastHit hit;

        if(Physics.Raycast(shootPosition.position, shootPosition.forward, out hit, range))
        {
            Debug.DrawRay(shootPosition.position, shootPosition.forward * hit.distance, Color.red, 1f);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                enemy.takeDamage(damage);
            }
        }
    }

    public void getTypeAndLevel()
    {
        // logic to get current character and current characters stats
    }

}
