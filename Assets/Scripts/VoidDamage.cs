using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidDamage : MonoBehaviour
{
    [SerializeField] GameMan gameMan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Dude"))
        {
            gameMan.VoidPlayer();
        }
    }



}
