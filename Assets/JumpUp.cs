using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUp : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            other.GetComponent<PlayerBasicMovement>().AddImpact(Vector3.up, 1500f);
            other.GetComponent<PlayerHpManager>().TakeDamage(10);
        }
    }
}
