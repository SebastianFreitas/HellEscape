using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFade : MonoBehaviour
{


    public float amplitude;          //Set in Inspector 
    public float speed;                  //Set in Inspector 
    private float tempVal;
    private Vector3 tempPos;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(SetVisibleTimer());
    }

   IEnumerator SetVisibleTimer()
    {
        
        yield return new WaitForSeconds(.5f);
        GetComponent<MeshRenderer>().enabled = true;
    }
}
