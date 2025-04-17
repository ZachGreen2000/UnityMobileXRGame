using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class Bullet : MonoBehaviour
{
    public enemy enemy;
    public bulletManager bulletManager;
    public enemyManager enemyManager;
    public defenceManager defenceManager;
    private ParticleSystem ps;

    void Start()
    {
        if (TryGetComponent<ParticleSystem>(out ps))// gets particle system
        {
            Debug.Log("ParticleSystem found");
        }
        StartCoroutine(destroyDelay());
    }
    // this calls when the bullet colliders with enemy and returns enemy to the pool
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy hit");
            obj.GetComponent<Animator>().enabled = false;
            obj.GetComponent<enemy>().isMoving = false;
            obj.gameObject.SetActive(false);
            enemyManager.Pool.Release(obj.GetComponent<enemy>());
            defenceManager.updateKillCount();
            //returns bullet to pool on collision
            if (this.gameObject.CompareTag("girlyBullet"))
            {
                bulletManager.gPool.Release(this.gameObject);
            }
            else if (this.gameObject.CompareTag("waterBullet"))
            {
                this.gameObject.SetActive(false);
                bulletManager.wPool.Release(ps);
            }
        }
    }
    // called on reuse
    public void onReuse()
    {
        if (TryGetComponent<ParticleSystem>(out ps))// gets particle system
        {
            Debug.Log("ParticleSystem found");
        }
        StartCoroutine(destroyDelay());
    }
    // this returns the bullet to its pool after a certain amount of time
    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(5);
        if (this.gameObject != null)
        {
            if (this.gameObject.CompareTag("girlyBullet"))
            {
                bulletManager.gPool.Release(this.gameObject);
            }
            else if (this.gameObject.CompareTag("waterBullet"))
            {
                bulletManager.wPool.Release(ps);
            }
        }
    }
}
