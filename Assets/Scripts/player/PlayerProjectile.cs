using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerProjectile : MonoBehaviour
{

  private float speed = 150;

    void Start()
    {
      StartCoroutine(waiter());
    }

    void Update()
    {
      transform.position += transform.forward*Time.deltaTime*speed;
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(5f);
      Destroy(gameObject);
    }
}
