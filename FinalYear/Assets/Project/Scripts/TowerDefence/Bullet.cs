using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class Bullet : MonoBehaviour
{
    public enemy enemy;
    public bulletManager bulletManager;
    private ParticleSystem ps;

    void Start()
    {
        TryGetComponent(out ps);
        StartCoroutine(destroyDelay());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            enemy.damageEnemy();
        }
    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(5);
        if (this.gameObject.CompareTag("girlyBullet"))
        {
            bulletManager.gPool.Release(this.gameObject);
        }else if (this.gameObject.CompareTag("waterBullet"))
        {
            bulletManager.wPool.Release(ps);
        }
    }
}
