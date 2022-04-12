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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            other.transform.GetComponent<PlayerHpManager>().TakeDamage(10);
            var direction = other.transform.position - transform.position;
            other.GetComponent<PlayerBasicMovement>().AddImpact(direction, 50f);

            source.PlayOneShot(explodeSound, 0.1f);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(this.gameObject);
    }

    
}
