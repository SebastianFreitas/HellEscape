
using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float timeBtwShots = .5f;

    public int currentBullets = 8;
    public Camera fpsCam;
    public CharacterController controller;
    public GameObject impactEffect;
    public GameObject projectile;
    public GameObject pnt;
    public GameObject muzzleFlashFront;

    public AudioSource audioS;
    public AudioClip shoot;
    public float volume = .6f;
    private Vector3 targetPoint ;
    private RaycastHit hit;
    private bool canShoot = true;


    void Start()
    {
      muzzleFlashFront.SetActive(false);
    }


    void Update()
    {
      if (Input.GetButton("Fire1") && canShoot ) Shoot();
      else if (Input.GetButton("Fire2")) SecondaryFire();
    }

    void Shoot(){
      muzzleFlashFront.SetActive(true);
      StartCoroutine(waiterFlash());

      GetComponent<AudioSource>().PlayOneShot(shoot, volume);

      Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
      if (Physics.Raycast(ray, out hit))
          targetPoint = hit.point;
      else
          targetPoint = ray.GetPoint(1000);

      pnt.transform.LookAt(targetPoint);

      GameObject bullet = Instantiate(projectile, pnt.transform.position , pnt.transform.rotation) ; 
      bullet.GetComponent<PlayerProjectile>().playerSpeed = controller.velocity;

      currentBullets--;
      canShoot = false;

      StartCoroutine(waiter());
    }

    void SecondaryFire()
    {
      GetComponent <ParticleSystem>().Play();
      ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
      em.enabled = true;
    }

    IEnumerator waiter(){
      yield return new WaitForSeconds(timeBtwShots);
      canShoot = true;
    }

    IEnumerator waiterFlash(){
      yield return new WaitForSeconds(.02f);
      muzzleFlashFront.SetActive(false);
    }

    void OnEnable()
    {
      canShoot = true;
    }
}
