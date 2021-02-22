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

  public bool visual = false;

  public bool initialFade = false;

  public int bounces = 2;
  Rigidbody rb;

  void Start()
  {
    SetVisibility(false);
    rb = GetComponent<Rigidbody>();
    /*if (totalConvergion.bludgeoning != 0) convertBludgeoning(); 
    else if (visual) bounces = 0;*/
    //if (initialFade) StartCoroutine(fadeWaiter());//this line will fuck up (usual bug andar pa tras ou pa frente + double bounce com side walk)
    rb.AddForce(transform.forward * speed);
    StartCoroutine(waiter(.5f));
  }


  private void SetVisibility(bool onOff)
  {
    //this.GetComponent<Renderer>().enabled = onOff;
    this.GetComponentInChildren<TrailRenderer>().enabled = onOff;
  }

  IEnumerator waiter(float a){
    yield return new WaitForSeconds(a);
    Destroy(gameObject);
  }

  IEnumerator fadeWaiter()
  {
    SetVisibility(false);
    yield return new WaitForSeconds(.005f);
    SetVisibility(true);
  }

  void OnEnable()
  {
    StartCoroutine(waiter(.5f));
  }

  void OnTriggerEnter(Collider other)
  {
      if (other.gameObject.CompareTag("Monster"))
      {
         other.GetComponent<Monster>().TakeDamage(damage);
         Physics.IgnoreCollision(transform.GetComponent<Collider>(), other.transform.GetComponent<Collider>());
      }

  }

  void OnCollisionEnter(Collision collision)
  {
      ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.transform.GetComponent<Monster>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (bounces > 0)
        {
            SetVisibility(true);
            AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position);
            rb.AddForce(contact.normal * 100);
            bounces--;
        }
        else Destroy(this.gameObject);
  }







}
