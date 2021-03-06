using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCilinder : PropBehaviour
{
    [SerializeField] float health;

    public AudioSource audioSource;
    public AudioClip kaboom;


    void OnCollisionEnter(Collision other)
  {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Bullet")) Explode(transform.position, 6f);


  }

    void Explode(Vector3 center, float radius)
    {
        transform.GetChild(0).GetComponent<ParticleSystem>().Play(true);

        audioSource.PlayOneShot(kaboom,.2f);

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Dude")){
                hitCollider.SendMessage("TakeDamage", 50);
                var direction = hitCollider.transform.position - transform.position;
                hitCollider.SendMessage("AddHighImpact", direction) ;
            }
            else
            if (hitCollider.CompareTag("Monster"))
            {
                hitCollider.SendMessage("TakeDamage", 50);
                var direction = hitCollider.transform.position - transform.position;
                hitCollider.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, radius);
            }
            else
            if (hitCollider.CompareTag("Prop"))
            {
                var direction = hitCollider.transform.position - transform.position;
                hitCollider.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position, radius);
            }
        }
        StartCoroutine(Die());
    }

    IEnumerator Die()
   {
        transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
   }
}
