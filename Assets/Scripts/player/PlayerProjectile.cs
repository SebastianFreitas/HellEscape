using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{

  private float speed = 500;
  private Vector3 startPosition;
  public  Vector3 playerSpeed;
  Rigidbody rb;

    void Start()
    {
      StartCoroutine(waiter());
      rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
      rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(5f);
      Destroy(gameObject);
    }

    void OnEnable()
    {
      StartCoroutine(waiter());
    }


}
