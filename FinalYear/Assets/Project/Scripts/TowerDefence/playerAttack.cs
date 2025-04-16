using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class playerAttack : MonoBehaviour
{
    [Header("Attack Variables")]
    public Transform shootPosition;
    public float range;
    public float gRange;
    public float wRange;
    public int damage;
    public int type;
    public float bulletSpeed;
    public LayerMask enemyLayer;

    [Header("Scripts")]
    public enemy enemy;
    public enemyManager enemyManager;
    public defenceManager defenceManager;
    public bulletManager bulletManager;

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
            updatedAttack();
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
    // this function will be called on attack and dependent on which character is active it triggers a different attack
    // the function calls the bullet pools for range attacks and adds force to them from the set shoot position
    public void updatedAttack()
    {
        Debug.Log("Updated shoot");
        if (gameManager.CharacterManager.ActiveCharacter.characterID == "1")
        {

        }else if (gameManager.CharacterManager.ActiveCharacter.characterID == "2")
        {
            Debug.Log("Water character detected: Shoot");
            Transform spawnPos = findChild(this.transform, "shootPoint");
            if (spawnPos != null)
            {
                var pooledBullet = bulletManager.wPool.Get();
                pooledBullet.transform.position = spawnPos.position;
                pooledBullet.transform.rotation = spawnPos.rotation;
                var velocityModule = pooledBullet.GetComponent<ParticleSystem>().velocityOverLifetime;
                velocityModule.enabled = true;
                velocityModule.x = -wRange;
            }else
            {
                Debug.Log("Cant find shootPoint");
            }
        }
        else if (gameManager.CharacterManager.ActiveCharacter.characterID == "3")
        {
            Transform spawnPos = this.transform.Find("shootPoint");
            Transform spawnPos2 = this.transform.Find("shootPoint(1)");
            var pooledBullet = bulletManager.gPool.Get();
            var pooledBullet2 = bulletManager.gPool.Get();
            pooledBullet.transform.position = spawnPos.position;
            pooledBullet2.transform.position = spawnPos2.position;
            Rigidbody rb = pooledBullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(spawnPos.forward * gRange, ForceMode.Impulse);
            Rigidbody rb2 = pooledBullet2.GetComponent<Rigidbody>();
            rb2.velocity = Vector3.zero;
            rb2.angularVelocity = Vector3.zero;
            rb2.AddForce(spawnPos2.forward * gRange, ForceMode.Impulse);
        }
    }
    // this function is here to iterate through the children of the player parent to find the child shootPoint
    // the reason for this function is so it can be reused for different characters
    public Transform findChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            Transform found = findChild(child, childName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    public void getTypeAndLevel()
    {
        // logic to get current character and current characters stats
    }

}
