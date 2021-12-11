using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public Transform playerStart;
    public int roomType;
    public Transform[] propSpawns;
    public GameObject[] objects;
    public Transform[] enemySpawns;



    public GameObject[] skulls;
    public GameObject portal;

    public GameObject skull;
    public bool locked = true;
    public int monstersAlive = 0;
    public int[] weight;

    public GameObject player;
    public PlayerMovement playerMovement;

    public GameObject[] Layouts;

    public int areaLevel = 1;

    private Transform[] enemies;

    private void Start()
    {
        portal.SetActive(false);
        StartCoroutine(WatchEnemies());
        StartCoroutine(TimerPortalUnlock());
    }

    private void OnEnable()
    {
        StartCoroutine(WatchEnemies());
        StartCoroutine(TimerPortalUnlock());
    }

    internal void VoidPlayer()
    {
        player.GetComponent<CharacterController>().enabled = false;
       // Vector3 mov = player.GetComponent<PlayerBasicMovement>().lastPos;
        //player.transform.position = mov;
        player.GetComponent<CharacterController>().enabled = true;

    }

    public void PickLayout()
    {
        var x = Random.Range(0, Layouts.Length);
        Layouts[x].SetActive(true);
        var y = Layouts[x].GetComponentsInChildren<Monster>();
        foreach ( Monster z in y)
        {
            z.player = this.player;
            z.transform.parent = transform;
            z.level = areaLevel;
            monstersAlive++;
        }

    }

    public void SpawnObjects(int more)
    {
        for (int i = 0; i < propSpawns.Length; i++)
        {   
            for (int j = 0; j<Random.Range(1,6); j++){
                var x = GetRandomWeightedIndex(weight);
                Instantiate(objects[x], new Vector3(propSpawns[i].position.x, propSpawns[i].position.y, propSpawns[i].position.z ), transform.rotation, transform);
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
        if (!locked) transform.parent.parent.GetComponent<GameMan>().Next(roomType);
    }

    private IEnumerator WatchEnemies()
    {
        
        yield return new WaitForSeconds(2f);
        if (monstersAlive <= 0)
        {
            locked = false;
            portal.SetActive(true);
            
        }
        StartCoroutine(WatchEnemies());
    }

    private IEnumerator TimerPortalUnlock()
    {
        yield return new WaitForSeconds(60f);
        locked = false;
        portal.SetActive(true);
    }

    internal Transform[] GetEnemies()
    {
        return enemies;
    }
}
