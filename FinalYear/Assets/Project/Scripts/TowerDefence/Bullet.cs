using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class Bullet : MonoBehaviour
{
    private bulletManager bulletManager;
    private enemyManager enemyManager;
    private defenceManager defenceManager;
    private ParticleSystem ps;
    private Coroutine lifeTimer;
    // the following functions are used to inject the needed scripts at runtime as prefabs cant have singleton scripts in the scene attached
    public void SetupManager(defenceManager defman)
    {
        defenceManager = defman;
    }
    public void SetupEnemyManager(enemyManager enman)
    {
        enemyManager = enman;
    }
    public void SetupBulletManager(bulletManager bulman)
    {
        bulletManager = bulman;
    }
    void Start()
    {
        if (TryGetComponent<ParticleSystem>(out ps))// gets particle system
        {
            Debug.Log("ParticleSystem found");
        }
        lifeTimer = StartCoroutine(destroyDelay());
    }
    // this calls when the bullet colliders with enemy and damages enemy
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("enemy hit");
            //obj.GetComponent<enemy>().isMoving = false;
            //obj.GetComponent<Animator>().SetBool("Moving", false);
            //enemyManager.Pool.Release(obj.GetComponent<enemy>());
            obj.GetComponent<enemy>().DmgNDead(defenceManager.playerDamage);
            defenceManager.updateKillCount();
            //returns bullet to pool on collision
            if (this.gameObject.CompareTag("girlyBullet"))
            {
                bulletManager.gPool.Release(this.gameObject);
            }
            else if (this.gameObject.CompareTag("waterBullet"))
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
            ps.Play();
            Debug.Log("ParticleSystem found");
        }
        // checks and stops coroutines to prevent stacking
        if (lifeTimer != null)
        {
            StopCoroutine(lifeTimer);
        }
        lifeTimer = StartCoroutine(destroyDelay());
    }
    // this returns the bullet to its pool after a certain amount of time
    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(2);
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
