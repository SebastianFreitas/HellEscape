using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    private void OnEnable()
    {
        transform.Rotate(0.0f, Random.Range(-180f, 180f), 0.0f, Space.World);
    }

}
