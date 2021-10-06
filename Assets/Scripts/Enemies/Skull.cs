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

    public ParticleSystem action;

    new void Start()
    {
        base.Start();
        action.Stop();
        StartCoroutine(waiterStart());
    }



    IEnumerator randomJump()
    {
       // audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume);

        action.Play();
        StartCoroutine(AshesWaiter());

        Vector3 playerPos = new Vector3(base.player.transform.position.x, base.player.transform.position.y, base.player.transform.position.z);

        Vector3 direction_to_player = (playerPos - this.transform.position).normalized; //randomHeight +

         Vector3 randomHeight = new Vector3(0, Random.Range(minAltitudeJumpDistance, maxAltitudeJumpDistance), 0);

        base.rigidBody.AddForce(( direction_to_player) * Random.Range(minJumpForce, maxJumpForce));

        var a = Random.Range(minWaitingTime, maxWaitingTime);
        yield return new WaitForSeconds(a);
        
        StartCoroutine(randomJump());


    }
    IEnumerator AshesWaiter()
    {
        yield return new WaitForSeconds(.5f);
        action.Stop();
    }


    IEnumerator waiterStart()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(randomJump());
    }
}