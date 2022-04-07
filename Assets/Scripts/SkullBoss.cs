using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBoss : Monster
{
 
    [SerializeField] float minWaitingTime;
    [SerializeField] float maxWaitingTime;
    [SerializeField] float maxJumpForce;
    [SerializeField] float minJumpForce;
    [SerializeField] float randomJumpTime;

    public ParticleSystem attack;
    private bool isRed;

    [SerializeField] Skull miniSkull;

    new void Start()
    {
        var x = Random.Range(-2, 3);
        rigidBody.angularVelocity = new Vector3(x, x, x);

        switch (roomActivator.influcence)
        {
            case VoidBoon.BoonType.Blue:
                isRed = false;
                break;

            case VoidBoon.BoonType.Red:
                isRed = true;
                break;
        }
    }


    private void OnEnable()
    {
        StartCoroutine(RandomJump());
       if (isRed) StartCoroutine(SummonSkulls());
    }


    IEnumerator RandomJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomJumpTime);
            Vector3 playerPos = new Vector3(base.player.transform.position.x, base.player.transform.position.y, base.player.transform.position.z);

            Vector3 direction_to_player = (playerPos - this.transform.position).normalized;

            Vector3 finalDir; 
            if (isRed) finalDir = (RandomDir() + direction_to_player);
            else finalDir = direction_to_player;

            rigidBody.AddForce((finalDir) * Random.Range(minJumpForce, maxJumpForce));
        }
    }

    private Vector3 RandomDir()
    {
        return new Vector3(
        Random.Range(-10, 10),
        Random.Range(-10, 10),
        Random.Range(-10, 10)
        );
    }

    IEnumerator SummonSkulls()
    {
        while (true)
        {
            var waitTime = Random.Range(minWaitingTime, maxWaitingTime);
            yield return new WaitForSeconds(waitTime);
            var skull = Instantiate(miniSkull, transform.position, transform.rotation, transform.parent);
            skull.transform.parent.GetComponent<RoomActivator>().numberOfEnemies++;
        }

    }

}
