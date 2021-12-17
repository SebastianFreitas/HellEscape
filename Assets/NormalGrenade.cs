using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGrenade : GrenadeData
{
    public int damage;
    public int area;
    public int timeToExplode;
    public int bounces;

    public Rigidbody rb;

    public KillObject exp;

    private bool exploded = false;

    private void Start()
    {
        
        rb.AddForce(4*speed * transform.forward + playerMov.move );
        StartCoroutine(GrenadeTimer());
    }
    public IEnumerator GrenadeTimer()
    {
        yield return new WaitForSeconds(timeToExplode);
        if (!exploded) FireExplode();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!exploded)FireExplode();
    }

    public ParticleSystem spark;
    public AudioClip ricochet;
    public AudioClip explosionSound;


    public TrailRenderer trail;
    private void FireExplode()
    {
        exploded = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, area);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Dude"))
            {
                playerMov.AddImpact(hitCollider.transform.position - transform.position, damage);
            }
            else if (hitCollider.CompareTag("Monster") || hitCollider.CompareTag("MonsterHead"))
            {
                hitCollider.GetComponent<Monster>().TakeDamage(damage, false, 0);
            }
            else if (hitCollider.CompareTag("Prop"))
            {
                Debug.Log("hitcouch");
                hitCollider.GetComponent<Rigidbody>().AddForce((hitCollider.transform.position - transform.position) * 5f, ForceMode.Impulse);
            }

            
        }
        ExplodeParticule();
        transform.GetComponent<MeshRenderer>().enabled = false;
        trail.enabled = false;
        
    }

    void ExplodeParticule()
    {
        exp.gameObject.SetActive(true);
        ParticleSystem explode = exp.GetComponent<ParticleSystem>();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        explode.Play();
        AudioSource.PlayClipAtPoint(explosionSound, this.gameObject.transform.position, 0.2f);
        Destroy(gameObject, explode.main.duration);

    }
}
