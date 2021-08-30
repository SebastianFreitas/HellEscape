using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFade : MonoBehaviour
{

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(SetVisibleTimer());  
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(Random.Range(0, .4f));
        transform.Rotate(Random.Range(-0f, -90.0f), Random.Range(-0f, -90.0f), Random.Range(-0f, -90.0f), Space.Self);
        StartCoroutine(Rotate());

    }

    IEnumerator SetVisibleTimer()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(Rotate());
    }


}
