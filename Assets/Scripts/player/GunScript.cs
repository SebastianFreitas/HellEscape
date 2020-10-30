
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
    public GameObject projectile;

    private bool canShoot = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot){
          Shoot();
        }
    }

    void Shoot(){
      GetComponent<AudioSource>().Play();
      Instantiate(projectile, transform.GetChild(0).position, Quaternion.identity);
      canShoot = false;
      StartCoroutine(waiter());
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(timeBtwShots);
      canShoot = true;
    }
}
