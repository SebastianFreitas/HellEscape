using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSkull : Monster
{

    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject turretBullet;
    [SerializeField] Transform head;
    [SerializeField] ParticleSystem explosionEffect;

    private RaycastHit hit;
    private LayerMask mask;


    [SerializeField] float speed;
    new void Start()
    {
        mask = LayerMask.GetMask("Enemy");
        base.Start();
        StartCoroutine(Waiter());
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;


    }
    private void OnEnable()
    {
        
        
    }

    private IEnumerator Waiter()
    {
        

        while (true)
        {
            
            shootPoint.transform.LookAt(player.transform);
            head.LookAt(player.transform);


            var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
            var bulletScript = bullet.GetComponent<TurretBullet>();
            bulletScript.speed = 3;
            explosionEffect.Play();


            yield return new WaitForSecondsRealtime(1f);

        }

    }
}
