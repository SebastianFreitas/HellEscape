
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

    NavMeshAgent agent;

    private bool running = false;
    private GameObject bullet;

    private Rigidbody skullBody;


    //private bool seeTarget = false; //use to check if enemy as seen the player

    /** send raycast from point A to B
    Vector3 fromPosition = source.transform.position;
    Vector3 toPosition = destination.transform.position;
    Vector3 direction = toPosition - fromPosition;*/

    /*  pos = playerView.transform.position;
      dir = (this.transform.position - playerView.transform.position).normalized;
      Debug.DrawLine (pos, pos + dir * 10, Color.red, Mathf.Infinity); */


    void Start(){
      player = GameObject.FindGameObjectWithTag("Player").transform;
      timeBtwShots = startTimeBtwShots;
      skullBody = transform.GetComponent<Rigidbody>();
      StartCoroutine(waiter());
    }

    void Update()
    {

      if (running)
      {
        if (bullet != null) bullet.transform.LookAt(player, Vector3.up);

        if (Vector3.Distance(transform.position, player.position) > stoppingDistance){
              transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }     
        else 
        if(Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance){

            transform.position = transform.position;
        } 
        else 
        if(Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

          if (timeBtwShots <= 0){
              bullet = Instantiate(projectile, transform.position, Quaternion.identity);
              bullet.GetComponent<EnemyProjectile>().damage = damage;
              timeBtwShots = startTimeBtwShots;
          }else {
              timeBtwShots -=Time.deltaTime;
          }

          
          running = false;
          StartCoroutine(monsterCycle());
      }  
  }


  public void TakeDamage(float amount){
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
      //agent.SetDestination(player.transform.position);
      
    }

    IEnumerator monsterCycle()
    {
      yield return new WaitForSeconds(.09f);
      if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
      running = true;
    }


}
