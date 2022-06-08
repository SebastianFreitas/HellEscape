using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
             other.GetComponent<PlayerHpManager>().TakeDamage(10);
            other.GetComponent<PlayerBasicMovement>().AddImpact(Vector3.up,100);
        }

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime *3, Space.World);
    }

    
}
