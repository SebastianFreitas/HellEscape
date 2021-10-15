using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCilinder : PropBehaviour
{
    [SerializeField] float health;

    public AudioSource audioSource;
    public AudioClip kaboom;

    void ExplodeParticule()
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
        ExplodeParticule();

        audioSource.PlayOneShot(kaboom,.05f);

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Dude")){
                hitCollider.transform.GetComponent<PlayerHpManager>().TakeDamage(10);
                var direction = hitCollider.transform.position - transform.position;
                hitCollider.GetComponent<PlayerBasicMovement>().AddImpact(direction, 10f );

                //hitCollider.GetComponent<>
            }
            else if (hitCollider.CompareTag("Monster") || hitCollider.CompareTag("MonsterHead"))
            {
                hitCollider.GetComponent<Monster>().TakeDamage(10, false, 0);

                var direction = hitCollider.transform.position - transform.position;
                hitCollider.GetComponent<Rigidbody>().AddForce((hitCollider.transform.position - transform.position) * 5f, ForceMode.Impulse);
            }
            else if (hitCollider.CompareTag("Prop"))
            {
                hitCollider.GetComponent<Rigidbody>().AddForce((hitCollider.transform.position - transform.position) * 5f, ForceMode.Impulse);
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
