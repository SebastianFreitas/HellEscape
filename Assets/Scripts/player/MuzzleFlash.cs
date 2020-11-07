using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public Transform flash;

    void Update()
    {
      //flash.localScale = Vector3.one * Random.Range(2f,3.5f);
      flash.Rotate(0f, Random.Range(-0f,-90.0f), 0f, Space.Self);
      //flash.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
      //Random.Range(0,90.0f);
    }



}
