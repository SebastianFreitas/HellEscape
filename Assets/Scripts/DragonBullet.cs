using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBullet : PlayerProjectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            other.transform.GetComponentInParent<Monster>().TakeDamage(stats, false, stats.critMulti);

           
        }
        else if (other.gameObject.CompareTag("MonsterHead"))
        {
            other.transform.GetComponent<Monster>().TakeDamage(stats, true, stats.critMulti);    
        }


        if (bounces > 0)
        {
            RicochetSparkAndSound();

            bounces--;

            if (stats.fireDamage > 0)
            {
                //if (!playerInv.explosiveRicochet) StartCoroutine(KillBullet());
                FireExplode();
            }

        }
        else StartCoroutine(KillBullet());

    }
}
