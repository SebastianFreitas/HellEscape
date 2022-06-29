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
           var monster =  other.GetComponentInParent<TowerBoss>();
            if(monster) monster.StopBoss();
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < 215)
        {
            transform.Translate(2 * Time.deltaTime * Vector3.up, Space.World);
            
        }
        transform.Rotate(.05f, 0, 0, Space.World);
    }

    
}
