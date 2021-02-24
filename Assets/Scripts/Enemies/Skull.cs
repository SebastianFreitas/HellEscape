using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Monster
{

    [SerializeField] float maxAltitudeJumpDistance;
    [SerializeField] float minAltitudeJumpDistance;
    [SerializeField] float maxWaitingTime;
    [SerializeField] float minWaitingTime;
    [SerializeField] float maxJumpForce;
    [SerializeField] float minJumpForce;


    void Start()
    {
        base.Start();
        StartCoroutine(waiterStart());
    }



    IEnumerator randomJump()
    {

        Vector3 playerPos = new Vector3(base.player.position.x, base.player.position.y - 10f, base.player.position.z);

        Vector3 direction_to_player = (playerPos - this.transform.position).normalized;

        Vector3 randomHeight = new Vector3(0, Random.Range(minAltitudeJumpDistance, maxAltitudeJumpDistance), 0);

        base.rigidBody.AddForce((randomHeight + direction_to_player) * Random.Range(minJumpForce, maxJumpForce));

        var a = Random.Range(minWaitingTime, maxWaitingTime);
        yield return new WaitForSeconds(a);
        StartCoroutine(randomJump());


    }



    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(randomJump());
    }
}