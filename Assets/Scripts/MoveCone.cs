using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCone : MonoBehaviour
{
    public Transform torus;



    private void FixedUpdate()
    {
       // Vector3 chosen;
        transform.Rotate(Vector3.right * (125 * Time.deltaTime), Space.Self);
    }
}
