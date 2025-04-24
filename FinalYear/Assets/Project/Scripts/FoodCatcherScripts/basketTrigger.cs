using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basketTrigger : MonoBehaviour
{
    private string tag;
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("protein"))
        {
            foodManager.Instance.Pool.Release(obj.gameObject);
            tag = obj.gameObject.tag;
            catchManager.Instance.foodCaught(tag);
        }
        else if (obj.gameObject.CompareTag("fruit"))
        {
            foodManager.Instance.Pool.Release(obj.gameObject);
            tag = obj.gameObject.tag;
            catchManager.Instance.foodCaught(tag);
        }
        else if (obj.gameObject.CompareTag("veg"))
        {
            foodManager.Instance.Pool.Release(obj.gameObject);
            tag = obj.gameObject.tag;
            catchManager.Instance.foodCaught(tag);
        }
    }
}
