using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropBehaviour : MonoBehaviour
{

    public bool trown = true;
    public bool steady = false;

    public bool constantRotating = false;



    void Start()
    {


        if (constantRotating) GetComponent<Rigidbody>().angularVelocity = transform.InverseTransformDirection(Vector3.up) * 0.1f;

        if (!steady)
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
        }

    }



}
