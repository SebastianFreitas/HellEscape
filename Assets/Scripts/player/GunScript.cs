
using UnityEngine;
using System.Collections;


public class GunScript : MonoBehaviour
{
    public convergion totalConvergion;
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

    public GameObject inventory;


    void Start()
    {
      muzzleFlashFront.SetActive(false);
    }


    void Update()
    {
      if (Input.GetButton("Fire1") && canShoot ) Shoot(.49f,1);
      //else if (Input.GetButton("Fire2") && canShoot) SecondaryFire(.2f);
      else if (Input.GetKey("1") && canShoot) Shoot(.09f,1);
      else if (Input.GetKey("2") && canShoot) Shoot(1f,100);
      else if (Input.GetKeyDown("i")) OpenInventory();
    }

    void Shoot(float attackRate, int numberOfBullets){
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
      //bullet.GetComponent<PlayerProjectile>().totalConvergion.bludgeoning = 60;
      
      canShoot = false;
      StartCoroutine(waiter(attackRate));
    }

    IEnumerator waiter(float attackRate){
      yield return new WaitForSeconds(attackRate);
      canShoot = true;
    }

    private void OpenInventory()
    {
      Vector3 inventoryPosition = new Vector3(fpsCam.transform.position.x, fpsCam.transform.position.y, fpsCam.transform.position.z+5f);
      Instantiate(inventory,inventoryPosition , fpsCam.transform.rotation);
    }
    IEnumerator waiterFlash(){
      yield return new WaitForSeconds(.07f);
      muzzleFlashFront.SetActive(false);
    }

    void OnEnable()
    {
      canShoot = true;
    }
}
