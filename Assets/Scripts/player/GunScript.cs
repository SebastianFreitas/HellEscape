
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
    public CharacterController controller;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject projectile;
    public GameObject pnt;
    public GameObject muzzleFlashFront;

    public AudioSource audio;
    public AudioClip shoot;
    public AudioClip magOut;
    public AudioClip magIn;
    public float volume = .6f;

    private float x = Screen.width / 2;
    private float y = Screen.height / 2;
    private Transform objectHit;

    private bool canShoot = true;
    private bool reloading = false;
    private bool flashOn = false;

    void Start()
    {
      muzzleFlashFront.SetActive(false);
    }


    void Update()
    {
      if (!reloading)
      {

          if (Input.GetButton("Fire1") && canShoot )
          {
            if (currentBullets > 0) {
            Shoot();
            }
              else Reload();
          }
          if (Input.GetButton("Reload")) Reload();

          //if(Input.GetButtonUp("Fire1")) muzzleFlashFront.SetActive (false);

          //if(!canShoot) muzzleFlashFront.SetActive (false);
          //if (!flashOn) muzzleFlashFront.SetActive (false);
      }
    }

    void LateUpdate(){}

    void Shoot(){
      muzzleFlashFront.SetActive(true);
      StartCoroutine(waiterFlash());
      //muzzleFlashFront.SetActive(true);
      Debug.Log("Bam");
      audio.PlayOneShot(shoot, volume);
      RaycastHit hit;
      Ray ray = fpsCam.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
      Vector3 targetPoint ;
      if (Physics.Raycast(ray, out hit))
          targetPoint = hit.point;
      else
          targetPoint = ray.GetPoint(1000);


      pnt.transform.LookAt(targetPoint);
      GameObject bullet = Instantiate(projectile, pnt.transform.position , pnt.transform.rotation) ; //Quaternion.Euler(new Vector3(x,y,0))

      bullet.GetComponent<PlayerProjectile>().playerSpeed = controller.velocity;
      //bullet.GetComponent<Rigidbody>().velocity = controller.velocity ;

      currentBullets--;
      canShoot = false;
      /* sollution for bullet offset
      https://gamedev.stackexchange.com/questions/58390/when-i-shoot-from-a-gun-while-walking-the-bullet-is-off-the-center-but-when-st/58431
      var bullet_speed = transform.TransformDirection(Vector3 (0, 0, speed));
      clone.velocity=bullet_speed + player.velocity;
      */
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

    IEnumerator waiterFlash(){
      yield return new WaitForSeconds(.02f);
      muzzleFlashFront.SetActive(false);
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
