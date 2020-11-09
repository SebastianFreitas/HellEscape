using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{

  private float speed = 250f;
  private Vector3 startPosition;
  public  Vector3 playerSpeed;
  public AudioClip ricochet;
  public AudioSource source;

  public int bounces = 2;
  Rigidbody rb;

    void Start()
    {
      StartCoroutine(waiter(3f));
      rb = GetComponent<Rigidbody>();
      rb.AddForce(transform.forward * speed);
    }

    void Update()
    {
      //rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
      
    }

    IEnumerator waiter(float a){
      yield return new WaitForSeconds(a);
      Destroy(gameObject);
    }

    void OnEnable()
    {
      StartCoroutine(waiter(3f));
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
          source.PlayOneShot(ricochet, .1f);
          rb.AddForce(transform.forward * 100);
          bounces--;
        } else StartCoroutine(waiter(.5f));

    }





}
