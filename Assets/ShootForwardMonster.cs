using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootForwardMonster : Monster
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Transform shootDir;

    [SerializeField] GameObject turretBullet;
    [SerializeField] float speed;

    [SerializeField] float waitingTime;
    private float finalWaitTime;

    private void Start()
    {
        isFiller = true;
    }

    private void OnEnable()
    {

         StartCoroutine(Waiter());
        

    }

    private IEnumerator Waiter()
    {

        finalWaitTime = waitingTime - ((actionSpeed - 1) * waitingTime);
        if (finalWaitTime < 0.1) finalWaitTime = 0.1f;

        yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1f));
        while (true)
        {
            shootPoint.transform.LookAt(shootDir);
            var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
            var bulletScript = bullet.GetComponent<TurretBullet>();
            bulletScript.speed = speed;




            yield return new WaitForSecondsRealtime(finalWaitTime);

        }

    }

}
