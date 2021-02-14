using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{    
    public Transform flash;

    void OnEnable()
    {
      flash.Rotate(0f, Random.Range(-0f,-90.0f), 0f, Space.Self);
    }



}
