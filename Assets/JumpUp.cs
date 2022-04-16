using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dude"))
        {
            collision.gameObject.GetComponent<PlayerBasicMovement>().AddImpact(Vector3.up, 5000f);
            collision.gameObject.GetComponent<PlayerHpManager>().TakeDamage(5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            other.GetComponent<PlayerBasicMovement>().AddImpact(Vector3.up, 2500f);
            other.GetComponent<PlayerHpManager>().TakeDamage(5);
        }
    }
}
