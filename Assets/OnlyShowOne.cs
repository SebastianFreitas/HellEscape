using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyShowOne : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var random = Random.Range(0, transform.childCount);


        for (int i = 0; i < transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(random == i);
        }
    }


}
