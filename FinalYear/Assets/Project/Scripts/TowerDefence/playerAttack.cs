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

    [Header("Audio")]
    public AudioSource shootW;
    public AudioSource shootG;
    public AudioSource attackK;

    private Animator anim;
    private Rigidbody rb;
    private bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        getTypeAndLevel();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = rb.velocity.magnitude > 0.1f; // true if ridigbody has velocity

        if (Input.GetKey(KeyCode.Mouse0) && !isMoving) // detects input
        {
            anim.SetBool("attack", true);
            Debug.Log("Shoot");
            //attack();
            updatedAttack();
        }else if (Input.GetKey(KeyCode.Mouse0) && isMoving)
        {
            anim.SetBool("attackWalking", true);
            updatedAttack();
        }else if (!Input.GetKey(KeyCode.Mouse0) && isMoving)
        {
            anim.SetBool("walking", true);
            storeWeapon();
        }else
        {
            anim.SetBool("walking", false);
            anim.SetBool("attackWalking", false);
            anim.SetBool("attack", false);
            shootG.Stop();
            shootW.Stop();
            attackK.Stop();
            storeWeapon();
        }
    }

    public void attack() // this function is now obsolete however it is left in as raycast is useful for testing
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
        if (gameManager.CharacterManager.ActiveCharacter.characterID == "1") // knight character
        {
            StartCoroutine(playAttackSound(attackK));
            Transform weaponTrans = findChild(this.transform, "weapon");
            GameObject weapon = weaponTrans.gameObject;
            Transform storePos = findChild(this.transform, "weaponBack");
            Transform spawnPos = findChild(this.transform, "equipPoint");
            weapon.transform.position = spawnPos.position;
            weapon.transform.rotation = spawnPos.transform.rotation * Quaternion.Euler(0, 15, 0);
        }
        else if (gameManager.CharacterManager.ActiveCharacter.characterID == "2") // water character
        {
            StartCoroutine(playAttackSound(shootW));
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
        else if (gameManager.CharacterManager.ActiveCharacter.characterID == "3") // girly character
        {
            StartCoroutine(playAttackSound(shootG));
            Transform spawnPos = findChild(this.transform, "shootPoint");
            Transform spawnPos2 = findChild(this.transform, "shootPoint (1)");
            var pooledBullet = bulletManager.gPool.Get();
            var pooledBullet2 = bulletManager.gPool.Get();
            pooledBullet.transform.position = spawnPos.position;
            pooledBullet2.transform.position = spawnPos2.position;
            pooledBullet.transform.rotation = spawnPos.transform.rotation * Quaternion.Euler(0, -90, 0);
            pooledBullet2.transform.rotation = spawnPos2.transform.rotation * Quaternion.Euler(0, -90, 0);
            Rigidbody rb = pooledBullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(-spawnPos.right * gRange, ForceMode.Impulse);
            Rigidbody rb2 = pooledBullet2.GetComponent<Rigidbody>();
            rb2.velocity = Vector3.zero;
            rb2.angularVelocity = Vector3.zero;
            rb2.AddForce(-spawnPos2.right * gRange, ForceMode.Impulse);
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

    public void setComponents()
    {
        anim = this.transform.GetChild(3).GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    public void storeWeapon()
    {
        if (gameManager.CharacterManager.ActiveCharacter.characterID == "1")
        {
            Transform weaponTrans = findChild(this.transform, "weapon");
            GameObject weapon = weaponTrans.gameObject;
            Transform storePos = findChild(this.transform, "weaponBack");
            weapon.transform.position = storePos.position;
            weapon.transform.rotation = storePos.rotation * Quaternion.Euler(45, 0, 0);
        }  
    }

    // this will handle attack sound plays to stop rapid playing
    IEnumerator playAttackSound(AudioSource sound)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            yield return new WaitForSeconds(1f);
            sound.Play();
            isPlaying = false;
        }
    }

    public void getTypeAndLevel()
    {
        // logic to get current character and current characters stats
    }

}
