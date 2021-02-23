using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public Transform playerStart;
    public int roomType;
    public Transform[] randomSpawns;
    public GameObject[] objects;
    public Transform[] enemySpawns;
    public GameObject[] skulls;

    public GameObject skull;
    public GameObject prop;
    public bool locked = true;
    public int monstersAlive = 0;
    public int[] weight;


    private void Start()
    {
        
        StartCoroutine(WatchEnemies());

    }
    public void SpawnEnemies(int dif)
    {
        for (int i = 0; i < enemySpawns.Length; i++)
        {
            for (int j = 0; j < dif; j++)
            {
                skull = skulls[Random.Range(0, skulls.Length)];
                var skully = Instantiate(skull, new Vector3(enemySpawns[i].position.x, enemySpawns[i].position.y, enemySpawns[i].position.z), transform.rotation);
                skully.transform.parent = transform;
                monstersAlive++;

            }
        }
    }

    public void SpawnObjects(int more)
    {
        for (int i = 0; i < randomSpawns.Length; i++)
        {   
            for (int j = 0; j<more; j++){
                var x = GetRandomWeightedIndex(weight);
                //prop = Instantiate(objects[x], new Vector3(randomSpawns[i].position.x, randomSpawns[i].position.y + Random.Range(2, 6), randomSpawns[i].position.z + (Random.Range(-60, 60))), transform.rotation);
                prop = Instantiate(objects[x], new Vector3(randomSpawns[i].position.x, randomSpawns[i].position.y + Random.Range(2, 6), randomSpawns[i].position.z ), transform.rotation, transform);
                //prop.transform.parent = transform;
                //prop.transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
                //prop.GetComponent<Rigidbody>().AddForce();
            }  
        }
    }

    public int GetRandomWeightedIndex(int[] weights)
    {
        // Get the total sum of all the weights.
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;//elementCount
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }

        // No other item was selected, so return very last index.
        return index;
    }


    public void killMonster()
    {
        monstersAlive--;
    }

    public void UseDoor()
    {
        if (!locked) transform.parent.GetComponent<GameMan>().Next(roomType);
    }

    private IEnumerator WatchEnemies()
    {
        if (monstersAlive == 0) locked = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(WatchEnemies());
    }

}
