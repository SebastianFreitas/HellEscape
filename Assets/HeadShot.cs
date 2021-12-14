using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShot : MonoBehaviour
{
    public Monster mon;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var bullet = collision.transform.GetComponent<PlayerProjectile>();
            mon.TakeDamage(bullet.physicalDamage, true, bullet.critMulti);
        }
    }
}
