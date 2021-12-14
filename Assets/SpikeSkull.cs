using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSkull : Monster
{

    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject turretBullet;

    private RaycastHit hit;
    private LayerMask mask;


    [SerializeField] float speed;
    new void Start()
    {
        mask = LayerMask.GetMask("Enemy");
        base.Start();
        StartCoroutine(Waiter());

    }
    private void OnEnable()
    {
        
        
    }

    private IEnumerator Waiter()
    {
        

        while (true)
        {
            
            shootPoint.transform.LookAt(player.transform);
            transform.LookAt(player.transform);
            
            

            if (Physics.Raycast(transform.position, shootPoint.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity,mask))
            {

                    var bullet = Instantiate(turretBullet, shootPoint.position, shootPoint.rotation, null);
                    var bulletScript = bullet.GetComponent<TurretBullet>();
                    bulletScript.speed = 3;


            }
            yield return new WaitForSeconds(1f);

        }

    }
}
