using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{

    [SerializeField] bool isActive = true;
    private bool passed = false;
    [SerializeField] GameObject[] nextSteps;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !passed  && isActive)
        {
            passed = true;
            foreach(var current in nextSteps)
            {
                current.SetActive(true);
            }
        }
    }
}
