using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUp : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dude"))
        {
            collision.gameObject.GetComponent<PlayerBasicMovement>().AddImpact(Vector3.up, 500f);
            collision.gameObject.GetComponent<PlayerHpManager>().TakeDamage(5);
        }
    }
}
