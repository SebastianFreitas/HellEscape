using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHolder : MonoBehaviour
{
    public Camera fpsCam;

    private bool canShoot1 = true;
    private bool canShoot2 = true;

    public GameObject projectile1;
    public GameObject projectile2;


    // Start is called before the first frame update
    void OnEnable()
    {
        canShoot1 = true;
        canShoot2 = true;
    }
    

    void Update()
    {

        if (Input.GetKey("q") && (baseCD <=0) && canShoot1 )
        {
            canShoot1 = false;
            Shoot(true);
        }
        
    }


    private RaycastHit hit;
    private Vector3 targetPoint;
    public GameObject pnt;
    public GameObject realBulletHolder;
    public PlayerBasicMovement playerBasicMov;
    public GrenadeCooldown cd;
    void Shoot(bool primary)
    {

        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000);

        pnt.transform.LookAt(targetPoint);
        Transform realpos = fpsCam.transform;
        realpos.LookAt(targetPoint);
        GameObject projectile;

        if (primary)
        {
            projectile = Instantiate(projectile1, realBulletHolder.transform.position, realpos.rotation);
            var specificGrenade = projectile.GetComponent<GrenadeData>();
            specificGrenade.playerMov = playerBasicMov;
            //StartCoroutine(CoolDown1(basecolldown));
            var basecolldown = specificGrenade.cooldown;
            cd.startCDUP(basecolldown);
 
            baseCD = basecolldown;
            StartCoroutine(TimerDown());

        }
        else
        {
            projectile = Instantiate(projectile2, realBulletHolder.transform.position, realpos.rotation);
            var specificGrenade = projectile.GetComponent<GrenadeData>();
            specificGrenade.playerMov = playerBasicMov;
            StartCoroutine(CoolDown2(specificGrenade.cooldown));

            cd.SetMaxCD(specificGrenade.cooldown);
            cd.SetCD(specificGrenade.cooldown);
        }    
    }

    int baseCD =0;

    IEnumerator TimerDown()
    {
        while (baseCD > 0)
        {
            baseCD -= 1;
            //cd.SetCD(baseCD);
            yield return new WaitForSecondsRealtime(1);

        }
        canShoot1 = true;
    }



    IEnumerator CoolDown1(int cooldown)
    {
        canShoot1 = false;
        yield return new WaitForSeconds(cooldown);
        canShoot1 = true;
    }

    IEnumerator CoolDown2(int cooldown)
    {
        canShoot2 = false;
        yield return new WaitForSeconds(cooldown);
        canShoot2 = true;
    }
}
