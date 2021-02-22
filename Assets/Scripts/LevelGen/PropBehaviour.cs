using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        transform.Rotate(Random.Range(0,180), Random.Range(0, 180), Random.Range(0, 180));
        rb = GetComponent<Rigidbody>();
        StartCoroutine(waiterGravity());

    }

    IEnumerator waiterGravity(){
        float wait = Random.Range(15f,60f);
         yield return new WaitForSeconds(wait);
         GetComponent<Rigidbody>().useGravity = true;
    }


}
