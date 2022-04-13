using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToSpawn : MonoBehaviour
{
    [SerializeField] int chance;
    void Start()
    {

        var rando = Random.Range(0, 100);
        if (rando <= chance) gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }


}
