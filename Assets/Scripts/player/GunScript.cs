
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
      RaycastHit hit;
      Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
      Vector3 targetPoint ;
      if (Physics.Raycast(ray, out hit))
          targetPoint = hit.point;
      else
          targetPoint = ray.GetPoint( 1000 );



      GameObject bullet = Instantiate(projectile, transform.GetChild(4).position , transform.GetChild(4).rotation) ; //Quaternion.Euler(new Vector3(x,y,0))
      bullet.transform.forward = targetPoint - transform.GetChild(4).position;
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
