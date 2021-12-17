using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeMonster : Monster
{
    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject turretBullet;
    [SerializeField] ParticleSystem explosionEffect;

    [SerializeField] float speed;

    private bool isRunning = false;
    new void Start()
    {
        base.Start();
        //StartCoroutine(Waiter());
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        //isRunning = true;

    }
    private void OnEnable()
    {
        if (!isRunning) StartCoroutine(Waiter()); 

    }
    private void OnDisable()
    {
        isRunning = false;
    }

    private IEnumerator Waiter()
    {
        isRunning = true;

        while (true)
        {
            shootPoint.transform.LookAt(player.transform);


            var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
            var bulletScript = bullet.GetComponent<TurretBullet>();
            bulletScript.speed = speed;
            explosionEffect.Play();


            yield return new WaitForSecondsRealtime(1f);

        }

    }
}
