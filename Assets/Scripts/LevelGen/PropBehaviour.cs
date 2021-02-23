using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    private Transform oldTransform;

    void Start()
    {
        transform.Rotate(Random.Range(0,180), Random.Range(0, 180), Random.Range(0, 180));
        rb = GetComponent<Rigidbody>();
        StartCoroutine(waiterGravity());

    }

    IEnumerator waiterGravity(){
        
        float wait = Random.Range(15f,60f);
        if (rb.mass > 6) wait-= 30f;
         yield return new WaitForSeconds(wait);
         GetComponent<Rigidbody>().useGravity = true;
    }

/*void OnCollisionEnter(Collision other)
  {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Dude"))
        {
            Debug.Log("should be working");
            oldTransform = other.transform.parent;
            other.transform.parent = transform;

        }


  }

  
void OnCollisionExit(Collision other)
  {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Dude"))
        {
            Debug.Log("Out");
            //other.transform.parent = oldTransform;
        }


  }*/



}
