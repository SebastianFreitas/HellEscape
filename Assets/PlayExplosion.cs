using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayExplosion : MonoBehaviour
{
    private PlayerInventory playerInv;
    private PlayerBasicMovement playerMov;

    private BulletStats stats;
    internal void RemoteAwake(PlayerInventory inv, PlayerBasicMovement mov, BulletStats stats)
    {
        this.gameObject.SetActive(true);
        playerInv = inv;
        playerMov = mov;
        this.stats = stats;

        StartCoroutine(StartUp());

    }

    IEnumerator StartUp()
    {
        yield return new WaitForSecondsRealtime(.9f);
        PLayExplosionAnimation( DetectExplosion());
    }
    private float DetectExplosion()
    {
        float areaModifier = (1f + (playerInv.increasedFireArea / 100f));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f * areaModifier);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 direction = hitCollider.transform.position - transform.position;
            if (playerInv.pullfire) direction = transform.position - hitCollider.transform.position;
            if (hitCollider.CompareTag("Dude"))
            {
                playerMov.AddImpact(direction, stats.fireDamage * 5 * playerInv.pushForceModifier);
               if (playerInv.fireMovement) playerMov.StartCoroutine("FireMovement");
            }
            else if (hitCollider.CompareTag("Monster"))
            {
                var monster = hitCollider.GetComponentInParent<Monster>();
                monster.TakeDamage(new BulletStats(stats.fireDamage, 0, 0, 0, 0), false, 0);
                //monster.GetComponent<Rigidbody>().AddForce((hitCollider.transform.position - transform.position) * 5f, ForceMode.Impulse);
            }
            else if (hitCollider.CompareTag("MonsterHead"))
            {
                var monster = hitCollider.GetComponentInParent<Monster>();
                monster.TakeDamage(new BulletStats(stats.fireDamage, 0, 0, 0, 0), false, 0);
                monster.GetComponent<Rigidbody>().AddForce((direction) * 5f * playerInv.pushForceModifier, ForceMode.Impulse);
            }
            else if (hitCollider.CompareTag("Prop"))
            {
                hitCollider.GetComponent<Rigidbody>().AddForce((direction) * 5f * playerInv.pushForceModifier, ForceMode.Impulse);
            }


        }

        return areaModifier;
    }

    private void PLayExplosionAnimation(float area)
    {


        transform.localScale *= area;
        var explode = GetComponent<ParticleSystem>();


        explode.Play();

        Destroy(this, explode.main.duration);
    }
}
