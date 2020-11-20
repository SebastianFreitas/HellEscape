using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct convergion
{
  public float bludgeoning;
  public float piercing;
  public float slashing;

}
public class PlayerProjectile : MonoBehaviour
{

  public convergion totalConvergion;

  private float speed = 500f;
  private float damage = 50;
  public  Vector3 playerSpeed;
  public AudioClip ricochet;
  public AudioSource source;

  public GameObject newBullet;

  private Monster enemyScript;

  public int bounces = 2;
  Rigidbody rb;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    if (totalConvergion.bludgeoning != 0) convertBludgeoning(); 

    rb.AddForce(transform.forward * speed);
    StartCoroutine(waiter(.5f));
  }

  private void convertBludgeoning()
  {
    float totalBullets = totalConvergion.bludgeoning/5;
    float bludgeoningDamage = damage/totalBullets + 10;
    for(int i = 0; i < totalBullets; i++)
    {   
        Vector3 a = Random.insideUnitCircle * .7f;;
        Vector3 positionI = rb.transform.position + a;
        GameObject bullet = Instantiate(newBullet, positionI , rb.transform.rotation);
        bullet.GetComponent<PlayerProjectile>().damage = bludgeoningDamage; 
    }
  }

  IEnumerator waiter(float a){
    yield return new WaitForSeconds(a);
     Destroy(gameObject);
  }

  void OnEnable()
  {
    StartCoroutine(waiter(.5f));
  }

  void OnTriggerEnter(Collider other)
  {
      if (other.gameObject.tag == "Monster")
      {
          enemyScript = other.GetComponent<Monster>();
          enemyScript.TakeDamage(damage);
      }

  }
  void OnCollisionEnter(Collision collision)
  {
      ContactPoint contact = collision.contacts[0];
      if (bounces > 0)
      {
        transform.forward = contact.normal;
        AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position);
        rb.AddForce(transform.forward * 100);
        bounces--;
      } 
      else 
      {
        //GetComponent <ParticleSystem>().Play();
        //ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        //em.enabled = true;
        Destroy(gameObject);
      }
  }







}
