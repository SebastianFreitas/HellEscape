using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBoss : Monster
{
    [SerializeField] Transform[] steps;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        var target = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        var boss = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 relativePos = target - boss;
        transform.Translate(relativePos * Time.deltaTime * 3, Space.World);



    }

    
}
