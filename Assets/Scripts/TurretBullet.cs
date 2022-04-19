using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{

    Rigidbody rb;
    internal float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
    }
    void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Dude"))
        {
            collision.transform.GetComponent<PlayerHpManager>().TakeDamage(10);

           
        }
        Destroy(gameObject);


    
    }


}
