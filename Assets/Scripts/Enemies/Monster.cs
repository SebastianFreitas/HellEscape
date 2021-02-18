
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

  //private bool running = false;


  public Rigidbody skullBody;


  public GameObject spawn;
    

  void Start(){
    player = GameObject.FindGameObjectWithTag("Player").transform;
    timeBtwShots = startTimeBtwShots;
    skullBody = transform.GetComponent<Rigidbody>();
    //transform.Rotate(new Vector3(-80,0,0));
    StartCoroutine(waiter());
    StartCoroutine(randomJump());
  }



  void OnTriggerEnter(Collider other)
  {
        //ContactPoint contact = other.contacts[0];
        if (other.gameObject.CompareTag("Dude"))
        {
            var playerScript = other.gameObject.GetComponent<PlayerMovement>();
            playerScript.AddImpact(-transform.forward, 100f);
            playerScript.TakeDamage((int)damage);
            
        }

  }


  public void TakeDamage(float amount)
  {
    health-= amount;
    if (health <= 0f ){
      transform.parent.GetComponent<Room>().killMonster();
      Die();
    }
        
  }

  void Die()
    {
        Destroy(gameObject);
    }

  IEnumerator waiter()
  {
    yield return new WaitForSeconds(.5f);
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }



  IEnumerator randomJump()
  {
    float a = Random.Range(1.5f, 5f);
    Vector3 playerPos = new Vector3(player.position.x, player.position.y-10f, player.position.z);
    Vector3 direction_to_player = (playerPos - this.transform.position).normalized;
    Vector3 randomHeight = new Vector3(0,Random.Range(0,2),0);
     skullBody.AddForce((randomHeight+direction_to_player) * Random.Range(300f, 1000f));

    yield return new WaitForSeconds(a);
    StartCoroutine(randomJump());
  }
  IEnumerator monsterCycle()
  {
    float a = Random.Range(.6f, 5f); 
    yield return new WaitForSeconds(a);
    if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
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
