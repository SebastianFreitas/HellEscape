using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            other.GetComponent<PlayerHpManager>().TakeDamage(5);
            var mov = other.GetComponent<PlayerBasicMovement>();
            mov.velocity = Vector3.zero;
                mov.AddImpact(Vector3.up,500);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            other.GetComponent<PlayerHpManager>().TakeDamage(5);
            var mov = other.GetComponent<PlayerBasicMovement>();
            mov.velocity = Vector3.zero;
            mov.AddImpact(Vector3.up, 100);
        }
        else if (other.CompareTag("MonsterHead"))
        {
            other.GetComponentInParent<TowerBoss>().StopBoss();
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(3 * Time.deltaTime * Vector3.up, Space.World);
        transform.Rotate(.05f, 0, 0, Space.World);
    }

    
}
