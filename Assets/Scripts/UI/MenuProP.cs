using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuProP : MonoBehaviour
{
    public Rigidbody body;
    private float x = 0, y = 0, z = 0;
    private Vector3 m_EulerAngleVelocity;
    // Start is called before the first frame update
    void Start()
    {
        StartProp();
    }

    private void StartProp()
    {


        //transform.Rotate(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
        x = Random.Range(-1f, 1f);
        y = Random.Range(-1f, 1f);
        z = Random.Range(-1f, 1f);

        body.AddTorque(new Vector3(x, y, z), ForceMode.Force);
        body.AddForce(new Vector3(x, y, z) * Random.Range(.5f, 4f));

        m_EulerAngleVelocity = new Vector3(x, y, z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        transform.GetComponent<MenuProP>().enabled = false;
    }
}
