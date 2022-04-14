using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToSpawn : MonoBehaviour
{
    [SerializeField] int chance;
    [SerializeField] bool getsHigher = false;
    void Start()
    {
        var x = 1;
        if (getsHigher) x = PlayerPrefs.GetInt("PathLevel");
        else x = -PlayerPrefs.GetInt("PathLevel");
        var rando = Random.Range(0, 100);
        if (rando <= chance +x) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }


}
