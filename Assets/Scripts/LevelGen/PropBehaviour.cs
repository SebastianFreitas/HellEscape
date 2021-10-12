using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    private Transform oldTransform;

    public bool trown = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (trown)
        {
            transform.Rotate(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));

            rb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1) * Random.Range(1, 100)));
        }


        //StartCoroutine(waiterGravity());

    }

    IEnumerator waiterGravity(){
        
        float wait = Random.Range(15f,60f);
        if (rb.mass > 6) wait+= 30f;
         yield return new WaitForSeconds(wait);
         GetComponent<Rigidbody>().useGravity = true;
    }






}
