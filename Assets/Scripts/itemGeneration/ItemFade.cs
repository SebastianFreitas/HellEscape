using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFade : MonoBehaviour
{


    public float amplitude;          //Set in Inspector 
    public float speed;                  //Set in Inspector 
    private float tempVal;
    private Vector3 pos;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        pos = transform.position;
        StartCoroutine(SetVisibleTimer());  
    }

    IEnumerator Rotate()
    {

        yield return new WaitForSeconds(Random.Range(0, .4f));
        if (Random.Range(1, 5) > 3) transform.position = pos;
        else
        {
            transform.Rotate(Random.Range(-0f, -90.0f), Random.Range(-0f, -90.0f), Random.Range(-0f, -90.0f), Space.Self);
            transform.Translate(new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f),Random.Range(-.2f, .2f)));
        }
        
        StartCoroutine(Rotate());

    }

    IEnumerator SetVisibleTimer()
    {
        
        yield return new WaitForSeconds(.5f);
        GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(Rotate());
    }


}
