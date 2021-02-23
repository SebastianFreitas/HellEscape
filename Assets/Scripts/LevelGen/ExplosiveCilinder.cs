using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCilinder : PropBehaviour
{
    [SerializeField] float health;

  
  void OnTriggerEnter(Collider other)
  {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Bullet"))
        {


        }


  }


    void Die()
   {
        //AudioSource.PlayClipAtPoint(die, transform.position, volume+0.5f);
        Destroy(gameObject);
   }
}
