using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCone : MonoBehaviour
{
    public Transform torus;



    private void Start()
    {
        
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        while (true)
        {
            transform.Rotate(Vector3.right * 500 * Time.deltaTime, Space.Self);
            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
