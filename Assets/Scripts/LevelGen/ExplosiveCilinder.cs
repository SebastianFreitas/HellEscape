using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCilinder : PropBehaviour
{
    [SerializeField] float health;

  
  void OnCollisionEnter(Collision other)
  {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Bullet"))
        {
            Explode(transform.position, 6f);

        }


  }

    void Explode(Vector3 center, float radius)
    {
        int layerMask = 7 << 9;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Dude")){
                hitCollider.SendMessage("TakeDamage", 50);
                var direction = hitCollider.transform.position - transform.position;
                hitCollider.SendMessage("AddImpact", direction) ;
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
    }

    void Die()
   {
        //AudioSource.PlayClipAtPoint(die, transform.position, volume+0.5f);
        Destroy(gameObject);
   }
}
