using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawber : MonoBehaviour
{
    [SerializeField] GameObject[] spawnMonsterList;
    [SerializeField] GameObject[] errorList;
    [SerializeField] Transform spawnPos;
    [SerializeField] TMPro.TextMeshPro errorTXT;

    [SerializeField] GameObject player;

    private void Start()
    {
        errorTXT.text = errorMessages[errorIterator];
    }

    internal void SpawnMonster()
    {

        var pos = spawnPos.position;
        var rot = spawnPos.rotation;
        Spawn(pos, rot, false);
    }

    string[] errorMessages = { "????????????????" , "disable.component", "ACCESS DENIED", "plz stop", "Listen here you litle shit", "Thats it"};
    int errorIterator = 0;
    internal void Error()
    {
 
            errorTXT.text = errorMessages[errorIterator];

            if (errorIterator == 5)
            {
                var pos = errorList[0].transform.position;
                var rot = errorList[0].transform.rotation;
                for (int i = 0; i < 20; i++) Spawn(pos, rot, true);
            }
            else errorIterator++;

    }

    private void Spawn(Vector3 pos, Quaternion rot, bool ishive)
    {
        pos.x += Random.Range(-5, +5);
        pos.y += Random.Range(-1, +10);
        var spawn = spawnMonsterList[Random.Range(0, spawnMonsterList.Length)];
        var spawnx = Instantiate(spawn, pos, rot, transform);
        var script = spawnx.GetComponent<Monster>();
        script.player = this.player;
        script.isHub = true;
        script.level = 1;
        script.UpdateStatsToLevel();
        if (ishive) script.transform.GetComponent<Skull>().isHive = true;
    }
}
