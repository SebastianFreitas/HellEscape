
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [SerializeField] float damage = 10f;

    protected Transform player;
    protected Rigidbody rigidBody;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip die;
    public AudioClip[] hurts;
    public float volume = 0.5f;


    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = transform.GetComponent<Rigidbody>();
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
    health -= amount;
    if (health <= 0f )
    {
      transform.parent.GetComponent<Room>().killMonster();
      Die();
    } else audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume);

  }

  void Die()
   {
        AudioSource.PlayClipAtPoint(die, transform.position, volume+0.5f);
        Destroy(gameObject);
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
