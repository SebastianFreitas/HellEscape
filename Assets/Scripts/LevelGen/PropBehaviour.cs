using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropBehaviour : MonoBehaviour
{

    public bool trown = true;

    void Start()
    {
        
        if (trown)
        {
            transform.Rotate(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
            GetComponent<Rigidbody>().velocity = Random.onUnitSphere * Random.Range(1,30);
            //transform.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1) * Random.Range(1, 11100)));
        }
        else
        {
            transform.Rotate(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
            GetComponent<Rigidbody>().velocity = Random.onUnitSphere * Random.Range(.6f, 2f);
        }


        //StartCoroutine(waiterGravity());

    }

    //IEnumerator waiterGravity(){
        
    //    float wait = Random.Range(15f,60f);
    //    if (rb.mass > 6) wait+= 30f;
    //     yield return new WaitForSeconds(wait);
    //     GetComponent<Rigidbody>().useGravity = true;
    //}






}
