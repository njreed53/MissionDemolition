using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySleep : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.Sleep();
    }

    void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.relativeVelocity.magnitude);
        if(coll.gameObject.tag != "Projectile") return;
        if (coll.relativeVelocity.magnitude > 10)
        {
            Destroy(gameObject);
        }
    }
}
