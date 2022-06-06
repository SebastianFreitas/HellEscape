
using UnityEngine;
using System.Collections;


public class Gun : MonoBehaviour
{
    public Animator animator;
    public GunGenerator gunGen;

    internal GunOfAType gun;

    public float range = 100f;
    public float timeBtwShots = .5f;

    public Camera fpsCam;
    public GameObject realBulletHolder;
    private PlayerProjectile projectile;


    public GameObject pnt;
    public GameObject muzzleFlashFront;
    public GameObject lightFlash;

    public GameObject head;
    public GameObject body;

    public AudioSource audioS;
    public AudioClip shoot;
    public float volume = .6f;

    public PlayerBasicMovement playerScript;
    private Vector3 targetPoint ;
    private RaycastHit hit;
    private bool canShoot = true;


    public GameObject inventory;

    public GunOfAType gunText;

    public int bounces;
    public int bulletSpeed;

    public int fireDamage;
    public int coldDamage;
    public int poisonDamage;
    public int physicalDamage;

    private Transform[] enemies;


    private float firerate;


    float m_start, m_time;
    int m_fired = 0;
    private bool isWaiting;

    [SerializeField] PlayerInventory playerInv;
 
    void OnEnable()
    {
        m_start = m_time = Time.time;
        m_time +=0.2f;

        if (isWaiting) StartCoroutine(waiter(1));
    }

    private void OnDisable()
    {
        animator.Rebind();
    }



    void Start()
    {
        projectile = normal;
        gunGen = new GunGenerator();
        //EquipBaseGun();
        if (!PlayerPrefs.HasKey("WeaponExists0") ||PlayerPrefs.GetInt("WeaponExists0") < 0) EquipBaseGun();
        else
        {
            var x = GameObject.FindGameObjectWithTag("Inventory").transform;
            gun = x.GetComponent<Inventory>().slots[0].GetComponent<Slot>().gun;
            playerScript.increasedSpeed = gun.increasedSpeed;
            firerate = 1 / gun.finalFireRate;

            SetBulletStats();


        }
        lightFlash = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        muzzleFlashFront.transform.parent = transform.parent;
        muzzleFlashFront.SetActive(false);

    }



    void LateUpdate()
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
            canShoot = false;
            Shoot(firerate);
        }
    }


    public void ShootBlank()
    {
        StartCoroutine(waiterFlash());

        GetComponent<AudioSource>().PlayOneShot(shoot, volume / 2);
    }
    public void SetBulletStats()
    {
        fireDamage = (int)gun.GetFireDamage();
        coldDamage = (int)gun.GetColdDamage();
        poisonDamage = (int)gun.GetPoisonDamage();
        physicalDamage = (int)gun.GetPhysicalDamage();
        bounces = (int)gun.GetBounces();
        bulletSpeed = (int)gun.GetShotSpeed();
    }
    public GrenadeCooldown cd;
    void Shoot(float attackRate)
    {
        cd.startCD(timeBtwShots);
        //if (this.gun.increasedRicochetGuide < 0) enemies = playerScript.transform.parent.GetComponent<Room>().GetEnemies();

        canShoot = false;
        StartCoroutine(waiterFlash());

        GetComponent<AudioSource>().PlayOneShot(shoot, volume / 2);

        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000);

        pnt.transform.LookAt(targetPoint);
        var realpos = new GameObject();
        //Transform realpos;// = fpsCam.transform;
        realpos.transform.rotation = fpsCam.transform.rotation;
        realpos.transform.position = fpsCam.transform.position;
        realpos.transform.LookAt(targetPoint);

        SpawnBullet(realpos.transform, false);

        for (var i = 0; i < gun.baseBulletsPerShot - 1; i++) //shoot extra bullets
        {
            var pelletRot = realpos;
            var spread = 5f;
            pelletRot.transform.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);

            SpawnBullet(pelletRot.transform, false);
        }

        //if (gun.delayedBullet > 0) StartCoroutine(DelayedBullet(realpos.transform));
        StartCoroutine(waiter(attackRate));
    }

    internal PlayerProjectile SpawnBullet(Transform realpos, bool isCold)
    {
        var pos = realBulletHolder.transform.position;
        if (isCold)
        {
            pos = realpos.transform.position;
        }

        var bullet = Instantiate(projectile, pos, realpos.rotation); //shoot normal bullet

        bullet.initialFade = true;
        bullet.Gun = gun;

        var speed = bulletSpeed * (1+(playerInv.increasedBulletSpeed / 100));


        bullet.SetStats((int)gun.increasedRicochetGuide,
            bounces         + playerInv.additionalBounces,
            speed, 
            (fireDamage      + playerInv.additionalFireDamage) * playerInv.fireMultiplier, 
            coldDamage      + playerInv.additionalColdDamage, 
            poisonDamage    + playerInv.additionalPoisonDamage, 
            (physicalDamage  + playerInv.additionalPhysicalDamage), 
            gun.increasedCriticalDamage + playerInv.additionalincreasedCriticalDamage
            );

        if (playerInv.movementToPhys > 0)
        {
            bullet.stats.physicalDamage *= 1 + (int)gun.increasedSpeed / 100;
        }

        if (playerInv.fireToPhys > 0)
        {
            bullet.stats.physicalDamage += bullet.stats.fireDamage * playerInv.fireToPhys;
            bullet.stats.fireDamage*= 1-playerInv.fireToPhys;
        }

        bullet.stats.fireDamage += bullet.stats.poisonDamage * playerInv.poisonToFixeAsExtra;

        if (isCold)
        {
            bullet.stats.fireDamage = 0;
            bullet.stats.physicalDamage = 0;
            bullet.stats.poisonDamage = 0;
            bullet.isFilter = true;

        }

        bullet.playerMov = playerScript;
        bullet.gameObject.SetActive(true);
        bullet.playerInv = playerInv;
        if ((playerInv.additionalColdProj > 0 || playerInv.coldProjectileChance > 0) && bullet.stats.coldDamage > 0 )
        {
            if (!isCold)
            {
                var coldProjs = playerInv.additionalColdProj;
                var coldChance = playerInv.coldProjectileChance;

                while (coldChance > 100)
                {
                    coldChance -= 100;
                    coldProjs++;
                }

                if (Random.Range(1f, 100f) > 100 - (1 + coldChance))
                {
                    coldProjs++;
                }

                for (int i = 0; i < coldProjs; i++)
                {
                    SpawnBullet(realBulletHolder2.transform, true);
                }


            }
        }
        bullet.AwakeRemote();
        return bullet;
    }
    IEnumerator DelayedBullet(Transform realpos)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        SpawnBullet(realpos, false);
        for (var i = 0; i < gun.baseBulletsPerShot - 1; i++) //shoot extra bullets
        {
            var pelletRot = realpos;
            var spread = 5f;
            pelletRot.Rotate(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);

            SpawnBullet(pelletRot, false);
        }
    }

    IEnumerator waiter(float attackRate){
      isWaiting = true;
      yield return new WaitForSecondsRealtime(attackRate);
      canShoot = true;
      isWaiting = false;
    }
    public PlayerAnimations animators;
    IEnumerator waiterFlash(){
        //animator.Rebind();
        //animator.ResetTrigger("Running");
        //animator.SetBool("Running", false);
        //animator.SetBool("Shoot", true);
        //animator.Play("Shoot");
        animators.justShot = true;
        muzzleFlashFront.SetActive(true);
        lightFlash.SetActive(true);
        
        yield return new WaitForSeconds(.02f);
        muzzleFlashFront.SetActive(false);
        lightFlash.SetActive(false);
    }

    internal void EquipBaseGun()
    {
        //gun = gunGen.CreateWeapon(10, true);
        //playerScript.increasedSpeed = gun.increasedSpeed;
        //firerate = 1 / gun.finalFireRate;

        //SetBulletStats();



        SetGun(gunGen.CreateWeapon(10, true));
        var x = GameObject.FindGameObjectWithTag("Inventory").transform;
        x.GetComponent<Inventory>().AddWeapon(gun, true);

        GameObject.FindGameObjectWithTag("Slot").GetComponent<Slot>().EquipGun();
    }

    [SerializeField] PlayerProjectile horizontal;
    [SerializeField] PlayerProjectile slow;
    [SerializeField] PlayerProjectile piercing;
    [SerializeField] PlayerProjectile normal;
    [SerializeField] GameObject realBulletHolder2;

    public void SetGun(GunOfAType gun)
    {

        if(this.gun != null)
        {
            playerInv.increasedFireArea -= gun.fireExplosionAreaModifier;
            playerInv.igniteChance -= gun.igniteChance;

            playerInv.poisonSpeedDouble -= gun.poisonRateModifier;
            playerInv.poisonDuration /= 1 + gun.poisonDurationModifier / 100;

            playerInv.freezeChance /= 1 + gun.freezeChance / 100;
            playerInv.coldProjectileChance -= gun.ColdProjectileChance;

            playerInv.chancePhysDoubleDamage -= gun.doubleDamageChance;
            playerInv.bleedChance -= gun.bleedChance;
        }

        this.gun = gun;
        SetBulletStats();

        playerInv.increasedFireArea += gun.fireExplosionAreaModifier;
        playerInv.igniteChance *= 1 + gun.igniteChance / 100;

        playerInv.poisonSpeedDouble += gun.poisonRateModifier;
        playerInv.poisonDuration *= 1+ gun.poisonDurationModifier/100;

        playerInv.freezeChance *= 1 + gun.freezeChance / 100;
        playerInv.coldProjectileChance += gun.ColdProjectileChance;

        playerInv.chancePhysDoubleDamage += gun.doubleDamageChance;
        playerInv.bleedChance += gun.bleedChance;


        playerScript.increasedSpeed = gun.increasedSpeed;
        firerate = 1 / gun.finalFireRate;

        if (gun.horizontalShot > 0) projectile = horizontal;
        else if (gun.slowbullet > 0) projectile = slow;
        else if (gun.piercing > 0)
        {
            projectile = piercing;

        }
        else projectile = normal;

    }
}
