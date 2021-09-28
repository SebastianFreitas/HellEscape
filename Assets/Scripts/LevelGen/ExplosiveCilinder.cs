using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCilinder : PropBehaviour
{
    [SerializeField] float health;

    public AudioSource audioSource;
    public AudioClip kaboom;

    void Explode()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ParticleSystem exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.main.duration);
    }

    void OnCollisionEnter(Collision other)
    {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Bullet")) Explode(transform.position, 6f);


    }

    void Explode(Vector3 center, float radius)
    {
        Explode();

        audioSource.PlayOneShot(kaboom,.05f);

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Dude")){
                //hitCollider.gameObject.GetComponent<PlayerMovement>().TakeDamage(50);
                hitCollider.SendMessage("TakeDamage", 50);
                var direction = hitCollider.transform.position - transform.position;
                hitCollider.SendMessage("AddHighImpact", direction);
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
