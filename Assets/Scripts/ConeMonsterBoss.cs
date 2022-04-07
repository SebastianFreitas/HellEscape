using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMonsterBoss : Monster
{
    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject turretBullet;
    [SerializeField] ParticleSystem explosionEffect;


    [SerializeField] float speed;

    [SerializeField] float waitingTime;

    private bool isRunning = false;
    private bool isRed = false;

    private float finalWaitTime;
    new void Start()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;


        posOffset = transform.position;

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
        if (!isRunning)
        {
            //StartCoroutine(MoveRandom());
            StartCoroutine(Waiter());
        }

    }
    private void OnDisable()
    {
        isRunning = false;
    }

    private IEnumerator Waiter()
    {

        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.1) finalWaitTime = 0.1f;
        isRunning = true;
        yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1f));
        while (true)
        {
            //ShotAgaisntTarget(player.transform);

            //foreach (var current in directionsToShot) ShotAgaisntTarget(current);
            shootPoint.transform.LookAt(player.transform);
            var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
            var bulletScript = bullet.GetComponent<TurretBullet>();
            bulletScript.speed = speed;




            yield return new WaitForSecondsRealtime(finalWaitTime);

        }

    }



    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
