
using UnityEngine;
using System.Collections;


public class Gun : GunStats
{
    public Animator animator;

    public float timeBtwShots = .5f;

    public Camera fpsCam;

    public GameObject realBulletHolder;
    public GameObject projectile;
    public GameObject pnt;
    public GameObject muzzleFlashFront;
    public GameObject lightFlash;

    public AudioClip shoot;
    public float volume = .6f;
    private Vector3 targetPoint ;
    private RaycastHit hit;
    private bool canShoot = true;

    public GameObject inventory;


    void Start()
    {
        lightFlash = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        muzzleFlashFront.transform.parent = transform.parent;
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

    void Shoot(float attackRate, int numberOfBullets)
    {
      
        
      StartCoroutine(waiterFlash());

      GetComponent<AudioSource>().PlayOneShot(shoot, volume);

      Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
      if (Physics.Raycast(ray, out hit))
          targetPoint = hit.point;
      else
          targetPoint = ray.GetPoint(1000);

      pnt.transform.LookAt(targetPoint);
      Transform realpos = fpsCam.transform;
      realpos.LookAt(targetPoint);

      GameObject bullet = Instantiate(projectile, realBulletHolder.transform.position, realpos.rotation);
      bullet.GetComponent<PlayerProjectile>().initialFade = true;
      //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), head.GetComponent<Collider>());
      //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), body.GetComponent<Collider>());
      
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
        animator.SetTrigger("Shoot");
 
        muzzleFlashFront.SetActive(true);
        lightFlash.SetActive(true);
        
        yield return new WaitForSeconds(.03f);
        muzzleFlashFront.SetActive(false);
        lightFlash.SetActive(false);
    }

    void OnEnable()
    {
      canShoot = true;
    }
}
