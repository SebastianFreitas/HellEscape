using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] doorMesh;

    private int numberOfEnemies = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                numberOfEnemies++;
            }
            transform.GetComponent<BoxCollider>().size = Vector3.zero;

            foreach (GameObject door in doorMesh) door.SetActive(true);

        }
    }

    internal void IsEncounterDone()
    {
        numberOfEnemies--;
        if(numberOfEnemies == 0)
        {
            foreach (GameObject door in doorMesh) door.SetActive(false);
        }
    }
}
