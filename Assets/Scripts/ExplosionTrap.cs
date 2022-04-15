using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip explodeSound;

    private void OnEnable()
    {
        StartCoroutine("Die");
    }
    private bool hasCollide = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            if (hasCollide == false)
            {
                hasCollide = true;
                other.transform.GetComponent<PlayerHpManager>().TakeDamage(10);
                var direction = other.transform.position - transform.position;
                other.GetComponent<PlayerBasicMovement>().AddImpact(direction, 50f);
                StartCoroutine("WaitDamage");
                source.PlayOneShot(explodeSound, 0.1f);
            }

        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(this.gameObject);
    }
    IEnumerator WaitDamage()
    {
        yield return new WaitForSecondsRealtime(.5f);
        hasCollide = false;
    }

    
}
