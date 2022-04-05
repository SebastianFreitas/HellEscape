using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMonster : Monster
{
    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject turretBullet;
    [SerializeField] ParticleSystem explosionEffect;

    [SerializeField] float speed;

    [SerializeField] float waitingTime;

    private bool isRunning = false;

    private float finalWaitTime;
    new void Start()
    {

        //StartCoroutine(Waiter());
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        //isRunning = true;

        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.4) finalWaitTime = 0.4f;

    }
    private void OnEnable()
    {
        if (!isRunning)
        {
            StartCoroutine(MoveRandom());
            StartCoroutine(Waiter());
        }

    }
    private void OnDisable()
    {
        isRunning = false;
    }

    private IEnumerator Waiter()
    {


        isRunning = true;
        yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1f));
        while (true)
        {




            shootPoint.transform.LookAt(player.transform);
            var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
            var bulletScript = bullet.GetComponent<TurretBullet>();
            bulletScript.speed = speed;
            explosionEffect.Play();


            yield return new WaitForSecondsRealtime(finalWaitTime);

        }

    }

    private IEnumerator MoveRandom()
    {
        var waitTime = finalWaitTime * 2;
        bool signal = true;
        yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1f));
        while (true)
        {

            if (signal)
            {
                transform.position += Vector3.up;
                signal = false;
            }
            else
            {
                transform.position += Vector3.down;
                signal = true;
            }




            yield return new WaitForSecondsRealtime(waitTime);
        }

    }
}
