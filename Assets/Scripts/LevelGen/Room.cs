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
    public GameObject portal;

    public GameObject skull;
    public GameObject prop;
    public bool locked = true;
    public int monstersAlive = 0;
    public int[] weight;


    private void Start()
    {
        portal.SetActive(false);
        StartCoroutine(WatchEnemies());
    }

    public void SpawnEnemies(int dif)
    {
        var total = 4;
        for (int i = 0; i < total; i++)
        {
            StartCoroutine(SpawnEnemy(Random.Range(0,5)));
        }
    }

    public void SpawnObjects(int more)
    {
        for (int i = 0; i < randomSpawns.Length; i++)
        {   
            for (int j = 0; j<more; j++){
                var x = GetRandomWeightedIndex(weight);
                prop = Instantiate(objects[x], new Vector3(randomSpawns[i].position.x, randomSpawns[i].position.y + Random.Range(2, 6), randomSpawns[i].position.z ), transform.rotation, transform);
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

    private IEnumerator SpawnEnemy(int i)
    {
        var x = Random.Range(.01f,.1f);
        yield return new WaitForSeconds(x);
        skull = skulls[Random.Range(0, skulls.Length)];
        for(int a=0; a<3; a++)
        {
            var skully = Instantiate(skull, new Vector3(enemySpawns[i].position.x+ Random.Range(-.5f,.5f) , enemySpawns[i].position.y , enemySpawns[i].position.z + Random.Range(-.5f, .5f)), transform.rotation);
            monstersAlive++;
            skully.transform.parent = transform;
        }
    }

    private IEnumerator WatchEnemies()
    {
        
        yield return new WaitForSeconds(2f);
        if (monstersAlive == 0)
        {
            locked = false;
            portal.SetActive(true);
        }
        StartCoroutine(WatchEnemies());
    }

}
