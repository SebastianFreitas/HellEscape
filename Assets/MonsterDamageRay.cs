using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamageRay : MonoBehaviour
{
    Monster mob;

    private void Start()
    {
        mob = GetComponentInParent<Monster>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            mob.player.GetComponent<PlayerHpManager>().TakeDamage(mob.damage);
                
        }
    }
}
