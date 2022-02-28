using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testNav : MonoBehaviour
{
    public Transform playerpos;
    public NavMeshAgent agent;
    public Room room;
    // Start is called before the first frame update
    void Start()
    {
        playerpos = room.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(playerpos.position);
    }
}
