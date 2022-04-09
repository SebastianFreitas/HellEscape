using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFloor : MonoBehaviour
{
    [SerializeField] internal bool isActive = true;
    private bool passed = false;
    [SerializeField] Transform nextSteps;
    [SerializeField] GameObject mainBase;

    [SerializeField] GameObject[] additionalObjects;
    [SerializeField] internal int maxSteps = 40;

    private int totalSteps;

    [SerializeField] PathCombat[] combats;

    private void Start()
    {
        totalSteps = maxSteps;
        //for(int i = Random.Range(-18, 2); i > 0; i--)
        //{
        //    additionalObjects[Random.Range(0, additionalObjects.Length)].SetActive(true);
        //}

    }

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !passed && isActive)
        {
            passed = true;

            if(IsDivisible(maxSteps, 10))
            {
                SpawnCombat();
            }
            else if (maxSteps > 0)
            {
                var a = Instantiate(this, nextSteps.position, nextSteps.rotation, transform) as PathFloor;
                a.maxSteps = maxSteps - 1;
                //foreach (var current in a.additionalObjects)
                //{
                //    current.SetActive(false);
                //}
            }


            if (maxSteps == 0)
            {
                Instantiate(mainBase, nextSteps.position, nextSteps.rotation, transform);
                maxSteps--;
            }
        }




    }

    private void SpawnCombat()
    {
        PathCombat a = Instantiate(combats[Random.Range(0, combats.Length)], nextSteps.position, nextSteps.rotation, transform) as PathCombat;
        a.maxsteps = maxSteps - 1;
    }

    public bool IsDivisible(int x, int n)
    {
        return (x % n) == 0;
    }

    internal void ResetPath()
    {
        passed = false;
        maxSteps = totalSteps;
        int i = 0;

        foreach (Transform child in this.transform)
        {
            if (i > 2)GameObject.Destroy(child.gameObject);
            i++;
        }
    }
}
