using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{    
    public Transform flash;
    private bool isRunning = true;
    void Update()
    {
      if (isRunning) 
      {
        flash.Rotate(0f, Random.Range(-0f,-90.0f), 0f, Space.Self);
        isRunning = false;
        StartCoroutine(waiter());
      }
    }

    IEnumerator waiter()
    {
      yield return new WaitForSeconds(.1f);
      isRunning = true;
    }



}
