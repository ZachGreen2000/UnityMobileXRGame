using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;

public class meleeWeapon : MonoBehaviour
{
    // this script can be attached to any melee weapon and calls damage function on enemy if it collides
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<enemy>().DmgNDead(defenceManager.Instance.playerDamage);
        }
    }
}
