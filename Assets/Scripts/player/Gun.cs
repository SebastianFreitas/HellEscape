
using UnityEngine;
using System.Collections;


public class Gun : MonoBehaviour
{
    public Animator animator;
    public GunGenerator gunGen;

    public GunOfAType gun;

    public convergion totalConvergion;
    public float damage = 50f;
    public float range = 100f;
    public float timeBtwShots = .5f;

    public Camera fpsCam;
    public GameObject realBulletHolder;
    public GameObject projectile;
    public GameObject pnt;
    public GameObject muzzleFlashFront;
    public GameObject lightFlash;

    public GameObject head;
    public GameObject body;

    public AudioSource audioS;
    public AudioClip shoot;
    public float volume = .6f;

    public PlayerMovement playerScript;
    private Vector3 targetPoint ;
    private RaycastHit hit;
    private bool canShoot = true;


    public GameObject inventory;

    public GunOfAType gunText;

    float m_start, m_time;
    int m_fired = 0;
    void OnEnable()
    {
        m_start = m_time = Time.time;
        m_time +=0.2f;

    }



    void Start()
    {
        gunGen = new GunGenerator();
        gun = gunGen.CreateWeaponEmpty();
        lightFlash = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        muzzleFlashFront.transform.parent = transform.parent;
        muzzleFlashFront.SetActive(false);

        playerScript.speed *= ((gun.increasedSpeed/100)+1);
    }


    void Update()
    {
        if (Time.time >= m_time)
        {
            m_time += 1 / gun.finalFireRate;
            ++m_fired;
            //            Debug.Log("Rate of fire: " + (Time.time - m_start) / m_fired);

            /*if (m_fired % 200 == 0)
            {
                Debug.Log("Next 200 in: " + (Time.time - m_start));
            }*/

        }
        if (Input.GetButton("Fire1") && canShoot)
        {
            
            Shoot(1 / gun.finalFireRate, 1);
        }
    }

    internal void EquipBaseGun()
    {
        gun = gunGen.CreateWeaponEmpty();
    }

    void Shoot(float attackRate, int gun)
    {
        canShoot = false;
        StartCoroutine(waiterFlash());

        GetComponent<AudioSource>().PlayOneShot(shoot, volume/2);

        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000);

        pnt.transform.LookAt(targetPoint);
        Transform realpos = fpsCam.transform;
        realpos.LookAt(targetPoint);

        GunOfAType gunx;
        gunx = this.gun;
        gunText = this.gun;

        GameObject bullet = Instantiate(projectile, realBulletHolder.transform.position, realpos.rotation); //shoot normal bullet
        var bulletscript = bullet.GetComponent<PlayerProjectile>();
        bulletscript.initialFade = true;
        bulletscript.Gun = gunx;
        bulletscript.SetStats(gunx.GetDamage(), gunx.GetBounces(), gunx.shotSpeed, gunx.bounceSpeed);

        for (var i = 0; i < gunx.baseBulletsPerShot-1; i++) //shoot extra bullets
        {
            var pelletRot = realpos;
            var spread = 5f;
            pelletRot.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);

            bullet = Instantiate(projectile, realBulletHolder.transform.position, pelletRot.rotation);

            bulletscript = bullet.GetComponent<PlayerProjectile>();
            bulletscript.Gun = gunx;
            bulletscript.initialFade = true;
            bulletscript.SetStats(gunx.finalDamage, gunx.finalBounces, gunx.shotSpeed, gunx.bounceSpeed);
        }


      
      StartCoroutine(waiter(attackRate));
    }

    IEnumerator waiter(float attackRate){
      yield return new WaitForSeconds(attackRate);
      canShoot = true;
    }

    IEnumerator waiterFlash(){
        animator.SetTrigger("Shoot");
 
        muzzleFlashFront.SetActive(true);
        lightFlash.SetActive(true);
        
        yield return new WaitForSeconds(.02f);
        muzzleFlashFront.SetActive(false);
        lightFlash.SetActive(false);
    }

    public void SetGun(GunOfAType gun)
    {
        this.gun = gun;
    }
}
