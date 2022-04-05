using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCone : MonoBehaviour
{
    public Transform torus;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.right * (50 * Time.deltaTime), Space.Self);
    }
}
