
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
  public float health = 50f;
  public float speed;
  public float stoppingDistance;
  public float retreatDistance;
  public float damage = 10f;
  public float range = 100f;
  public float timeBtwShots;
  public float startTimeBtwShots= 1f;

  public GameObject projectile;
  private Transform player;

  private bool running = false;
  private GameObject bullet;

  private Rigidbody skullBody;


  public GameObject spawn;

  void Start(){
    player = GameObject.FindGameObjectWithTag("Player").transform;
    timeBtwShots = startTimeBtwShots;
    skullBody = transform.GetComponent<Rigidbody>();
    //transform.Rotate(new Vector3(-80,0,0));
    StartCoroutine(waiter());
  }

  void Update()
  {
    if (running)
    {
      Vector3 playerPos = new Vector3(player.position.x, player.position.y-10f, player.position.z);
      Vector3 direction_to_player = (playerPos - this.transform.position).normalized;
      skullBody.AddForce(direction_to_player * 20000f);
      running = false;
      StartCoroutine(monsterCycle());
    
    }  
  }

  void OnTriggerEnter(Collider other)
  {
      if (other.gameObject.tag == "Player")
      {
          player.GetComponent<PlayerMovement>().TakeDamage(damage);
      }

  }


  public void TakeDamage(float amount)
  {
    health-= amount;
    if (health <= 0f){
      Die();
    }
  }

  void Die(){
    Destroy(gameObject);
  }

  IEnumerator waiter()
  {
    yield return new WaitForSeconds(.5f);
    player = GameObject.FindGameObjectWithTag("Player").transform;
    running = true;  
  }

  IEnumerator monsterCycle()
  {
    float a = Random.Range(.6f, 2f); 
    yield return new WaitForSeconds(a);
    if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    running = true;
  }
/*
  IEnumerator fireRateCycle()
  {
    yield return new WaitForSeconds(timeBtwShots);
    canShoot= true;
  }

      //transform.LookAt(player);
      //transform.Rotate(new Vector3(-80,0,0));
      
      if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
      {
          transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
      }     
      else 
      if(Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
      {
          transform.position = transform.position;
      } 
      else 
      if(Vector3.Distance(transform.position, player.position) < retreatDistance)
      {
          transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
      }

      if (canShoot)
      {
        bullet = Instantiate(projectile, spawn.transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyProjectile>().damage = damage;
        bullet.transform.LookAt(player, Vector3.up);
        canShoot = false;
        StartCoroutine(fireRateCycle());
      }*/ 

}
