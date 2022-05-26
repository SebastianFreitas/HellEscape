using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHolder : MonoBehaviour
{
    public Camera fpsCam;

    private bool canShoot1 = true;
    private bool canShoot2 = true;

    public GrenadeData projectile1;
    public GameObject projectile2;

    public Gun guna;
    internal bool possible = true;

    [SerializeField] PlayerInventory playerInv;

    // Start is called before the first frame update
    void OnEnable()
    {

        canShoot1 = true;
        canShoot2 = true;
        beenLong = true;
    }

    private void Start()
    {
        //lightFlash = transform.GetChild(0).gameObject;

    }
    void Update()
    {

        if (Input.GetKey("q") && possible)
        {
            if (cd.CanShoot())//(baseCD <= 0) && 
            {
                canShoot1 = false;
                Shoot(true);
            }
            else if (beenLong) {
                StartCoroutine(WaiterWrong()); beenLong = false;
            }

        }
        
    }

    private IEnumerator WaiterWrong()
    {
        beenLong = false;
        AudioSource.PlayClipAtPoint(wrong, transform.position, .3f);
        yield return new WaitForSecondsRealtime(1f);
        beenLong = true;
    }


    private RaycastHit hit;
    private Vector3 targetPoint;
    public GameObject pnt;
    public GameObject realBulletHolder;
    public PlayerBasicMovement playerBasicMov;
    public GrenadeCooldown cd;
    void Shoot(bool primary)
    {
        guna.ShootBlank();

        Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000);

        pnt.transform.LookAt(targetPoint);
        Transform realpos = fpsCam.transform;
        realpos.LookAt(targetPoint);
        GrenadeData projectile;

        if (primary)
        {
            projectile = Instantiate(projectile1, realBulletHolder.transform.position, realpos.rotation) as GrenadeData;


            projectile.playerMov = playerBasicMov;
            projectile.damage += playerInv.additionalGrenadeDamage;


            var basecolldown = projectile.cooldown;
            cd.startCDUP(basecolldown);
 
            baseCD = basecolldown;
            StartCoroutine(TimerDown());

        }

    }

    int baseCD =0;

    public AudioClip wrong;
    private bool beenLong = true;


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
