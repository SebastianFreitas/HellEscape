
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

    private float x = Screen.width / 2;
    private float y = Screen.height / 2;

    private bool canShoot = true;

    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot){
          Shoot();
        }
    }

    void Shoot(){
      GetComponent<AudioSource>().Play();
      GameObject bullet = Instantiate(projectile,fpsCam.transform.position , Quaternion.identity);//transform.GetChild(0).position
      bullet.transform.forward = fpsCam.transform.forward;
      canShoot = false;
      StartCoroutine(waiter());
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(timeBtwShots);
      canShoot = true;
    }
}
