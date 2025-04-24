using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseTrigger : MonoBehaviour
{
    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("protein") || obj.gameObject.CompareTag("fruit") || obj.gameObject.CompareTag("veg"))
        {
            foodManager.Instance.Pool.Release(obj.gameObject);
        }
    }
}
