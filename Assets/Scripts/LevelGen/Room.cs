using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;
    public MeshCollider MeshCollider;
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

    public void SpawnObjects()
    {
        for (int i = 0; i < randomSpawns.Length; i++)
        {

                var objecty = Instantiate(objects[Random.Range(0, objects.Length)], new Vector3(randomSpawns[i].position.x, randomSpawns[i].position.y+2, randomSpawns[i].position.z + (Random.Range(-30, 30))), transform.rotation);
                objecty.transform.parent = transform;
              
        }
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
