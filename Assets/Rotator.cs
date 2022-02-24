using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0.0f, Random.Range(-180f,180f), 0.0f, Space.World);
    }


}
