using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{
    public GameObject skull;

    private GameObject clone;

    private bool wait = true;

    private float xSpawn;
    private float ySpawn;
    private float zSpawn;

    public int[] dificulty;
    private int difTier = 0;
    private int killed = 0;

    void Start()
    {
        StartCoroutine(waiter());
        //FillDificulties();
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait)
        {   
            if (clone == null)
            {   
                //spawnPoint.rotation = Quaternion.Euler(-90, 0, 0);
                /*Alive -= killed;
                dificulty = Alive ;
                killed = dificulty ;*/
                killed = 0;
                for (int i = 0; i<dificulty[difTier]; i++)
                {
                    randomize();
                    Vector3 spawnPoint = new Vector3 (transform.position.x + xSpawn,transform.position.y + ySpawn,transform.position.z+zSpawn);
                    clone = Instantiate(skull, spawnPoint , transform.rotation);
                    killed++;
                }
                wait = true;
            }
            StartCoroutine(waitToSpawn());
        }
    }

    void randomize()
    {
      xSpawn = Random.Range(-8.0f, 8.0f);
      ySpawn = Random.Range(1f, 5.0f);
      zSpawn = Random.Range(-8.0f, 8.0f);
       
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(5f);
        wait = false;
    }

    IEnumerator waitToSpawn()
    {
        yield return new WaitForSeconds(6f);
        if (difTier +1 < 4) difTier++;
        wait = false;
    }
}
