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

    [SerializeField] GameObject player;

    internal void SpawnMonster()
    {

        var pos = spawnPos.position;
        var rot = spawnPos.rotation;
        Spawn(pos, rot);
    }



    internal void Error()
    {
        var pos = errorList[0].transform.position;
        var rot = errorList[0].transform.rotation;
        for (int i = 0; i < 300; i++) Spawn(pos, rot);

    }

    private void Spawn(Vector3 pos, Quaternion rot)
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
    }
}
