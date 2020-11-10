using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{

  private float speed = 500f;
  private float damage = 50;
  private Vector3 startPosition;
  public  Vector3 playerSpeed;
  public AudioClip ricochet;
  public AudioSource source;

  private Monster enemyScript;

  public int bounces = 50;
  Rigidbody rb;

    void Start()
    {
      rb = GetComponent<Rigidbody>();
      rb.AddForce(transform.forward * speed);
      //rb.velocity =   playerSpeed - rb.velocity;
      //rb.position = 
      StartCoroutine(waiter(.5f));
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
      //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
      //Vector3 position = contact.point;
      //Instantiate(explosionPrefab, position, rotation);
      //Destroy(gameObject);
      if (bounces > 0)
      {
        transform.forward = contact.normal;
        AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position);
        rb.AddForce(transform.forward * 100);
        bounces--;
      } 
      else 
      {
         //AudioSource.PlayClipAtPoint(audio, this.gameObject.transform.position);
         Destroy(gameObject);
      }
  }







}
