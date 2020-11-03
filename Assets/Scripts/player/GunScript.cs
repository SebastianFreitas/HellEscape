
using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float timeBtwShots = .5f;
    public float timeReload = .75f;
    public float timeStamp = 0f;

    public int magSize = 8;
    public int currentBullets = 8;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject projectile;

    public AudioSource audio;
    public AudioClip shoot;
    public AudioClip magOut;
    public AudioClip magIn;
    public float volume = .6f;

    private float x = Screen.width / 2;
    private float y = Screen.height / 2;

    private bool canShoot = true;
    private bool reloading = false;




    void Update()
    {
      if (!reloading){

          if (Input.GetButton("Fire1") && canShoot ){
            if (currentBullets > 0)Shoot();
              else {Reload();}
          }

          if (Input.GetButton("Reload")){
            Reload();
          }
      }


    }

    void Shoot(){
      audio.PlayOneShot(shoot, volume);
      GameObject bullet = Instantiate(projectile, transform.GetChild(4).position , Quaternion.Euler(new Vector3(x,y,0)));
      //bullet.transform.SetParent(transform, true); //set bullet as child of gun, its less messy in playtime
      bullet.transform.forward = fpsCam.transform.forward;
      currentBullets--;
      canShoot = false;
      //StartCoroutine is stoped by SetActive(false) when they come back
      StartCoroutine(waiter());
    }

    void Reload()
    {
      reloading = true;
      audio.PlayOneShot(magOut, volume);
      StartCoroutine(waiterReload());
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(timeBtwShots);
      canShoot = true;
    }

    IEnumerator waiterReload(){
      yield return new WaitForSeconds(.5f);
      audio.PlayOneShot(magIn, volume);
      yield return new WaitForSeconds(timeReload);
      currentBullets = magSize;
      reloading = false;
    }
    //this is bad very bad player can pause to reset reload
    void OnEnable()
    {
      canShoot = true;
      reloading = false;
    }
}
