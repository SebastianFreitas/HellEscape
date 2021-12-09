using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Monster
{

    public NavMeshAgent agent;


    private void OnEnable()
    {

        StartCoroutine(waiterStart());
    }


    private void updateOnce()
    {
        agent.ResetPath();
        agent.SetDestination(player.transform.position);
        StartCoroutine(waiterStart());


    }

    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(1f);
        updateOnce();
    }
}
