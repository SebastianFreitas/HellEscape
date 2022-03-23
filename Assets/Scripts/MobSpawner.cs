using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] doorMesh;
    [SerializeField] Monster[] monsters;
    [SerializeField] GameObject lights;

    [SerializeField] bool isBoss = false;
    [SerializeField] Transform bossSpawnPos;

    internal int numberOfEnemies = 0;
    private BoxCollider[] boxColliders;
    private Light[] lightsComponent;


    private void Start()
    {
        lightsComponent = lights.GetComponentsInChildren<Light>();

        foreach (GameObject door in doorMesh) door.SetActive(false);

        boxColliders = transform.GetComponents<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            var x = 4;
            if (isBoss)
            {
                Instantiate(monsters[0], bossSpawnPos.position, bossSpawnPos.rotation, transform);
                numberOfEnemies++;
            } else SpawnTrashMobs(x);

            foreach (BoxCollider box in boxColliders) box.size = Vector3.zero;

            foreach (GameObject door in doorMesh) door.SetActive(true);
        }
    }

    private void SpawnTrashMobs(int x)
    {
        for (int i = 0; i < x; i++)
        {
            var chosenCollider = boxColliders[Random.Range(0, boxColliders.Length)];

            Vector3 randomPoint = RandomPointInBounds(chosenCollider.bounds);
            Instantiate(monsters[0], randomPoint, Quaternion.identity, transform);
            numberOfEnemies++;
        }
    }

    private Vector3 RandomPointInBounds(Bounds rawBounds)
    {
        Bounds bounds = rawBounds;
        bounds.size /= 2;
        return new Vector3(
        Random.Range(bounds.min.x , bounds.max.x ),
        Random.Range(bounds.min.y , bounds.max.y ),
        Random.Range(bounds.min.z , bounds.max.z )
        );
    }

    internal void IsEncounterDone()
    {
        numberOfEnemies--;

        if(numberOfEnemies == 0)
        {
            foreach (GameObject door in doorMesh) door.SetActive(false);
            foreach (Light light in lightsComponent) light.color = Color.red;
        }

    }
}
