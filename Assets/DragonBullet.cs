using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBullet : PlayerProjectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            other.transform.GetComponentInParent<Monster>().TakeDamage(stats, false, critMulti);

           
        }
        else if (other.gameObject.CompareTag("MonsterHead"))
        {
            other.transform.GetComponent<Monster>().TakeDamage(stats, true, critMulti);

            
        }

        if (fireDamage > 0)
        {
            StartCoroutine(KillBullet());
            FireExplode();

        }

    }
}
