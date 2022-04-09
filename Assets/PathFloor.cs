using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFloor : MonoBehaviour
{
    [SerializeField] bool isActive = true;
    private bool passed = false;
    [SerializeField] Transform nextSteps;
    [SerializeField] GameObject mainBase;

    [SerializeField] GameObject[] additionalObjects;
    [SerializeField] int maxSteps = 40;

    private void Start()
    {
        for(int i = Random.Range(-7, 4); i > 0; i--)
        {
            additionalObjects[Random.Range(0, additionalObjects.Length)].SetActive(true);
        }

    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !passed && isActive && maxSteps > 0)
        {
            passed = true;

            var a = Instantiate(this, nextSteps.position, nextSteps.rotation) as PathFloor;
            a.maxSteps = maxSteps-1;
            foreach(var current in a.additionalObjects)
            {
                current.SetActive(false);
            }

        }

        if (maxSteps == 0)
        {
            Instantiate(mainBase, nextSteps.position, nextSteps.rotation);
            maxSteps--;
        }
    }
}
