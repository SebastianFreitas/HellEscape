using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaActivator : MonoBehaviour
{

    [SerializeField] private GameObject lava;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            lava.SetActive(true);
        }
    }
}
