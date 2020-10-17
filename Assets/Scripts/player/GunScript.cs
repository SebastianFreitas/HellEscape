
using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float timeBtwShots = .5f;
    public float timeStamp = 0f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private bool canShoot = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot){
          Shoot();
        }
    }

    void Shoot(){
      muzzleFlash.Play();
      RaycastHit hit;
      if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
        Target target = hit.transform.GetComponent<Target>();

        if (target != null){
          target.TakeDamage(damage);
        }
        if (hit.transform != null)
        {
          GameObject impactGO = Instantiate(impactEffect, hit.point , Quaternion.LookRotation(hit.normal));
          Destroy(impactGO.transform.GetChild (0).gameObject, .2f);
          Destroy(impactGO.transform.GetChild (1).gameObject, 5f);
          Destroy(impactGO, 5.2f);
        }
      }
      canShoot = false;
      StartCoroutine(waiter());

    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(timeBtwShots);
      canShoot = true;
    }
}
