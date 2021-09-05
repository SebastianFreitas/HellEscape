
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] float health = 50f;
    [SerializeField] public float damage = 10f;

    public GameObject player;

    public PlayerMovement playerMovement;
    public Rigidbody rigidBody;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip die;
    public AudioClip[] hurts;
    public float volume = 0.5f;

    public GameObject drop;

    private Collider monsterCollider;
    private Collider playerCollider;
    public ParticleSystem AshesDamage;
    public ParticleSystem AshesDeath;



    protected void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();
        monsterCollider = rigidBody.GetComponent<Collider>();
        playerCollider = player.transform.GetComponent<Rigidbody>().GetComponent<Collider>();
    }


    void Update(){
        if (monsterCollider.bounds.Intersects(playerCollider.bounds))
        {
            var direction = player.transform.position- transform.position;
            var playerScript = player.GetComponent<PlayerBasicMovement>();
            playerScript.AddImpact(direction, 100f);
            player.GetComponent<PlayerHpManager>().TakeDamage((int)damage);
        }
    }

  public void TakeDamage(float amount)
  {  
    health -= amount;

    if (health <= 0f )
    {
        transform.parent.GetComponent<Room>().killMonster();
        Die();
    } 
    else
    {
        var rep = player.transform;
        rep.LookAt(transform.position);
        audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume);
        var bloodSplat = Instantiate(AshesDamage, transform.position, rep.rotation);
        bloodSplat.Play();
    }
  }

  void Die()
   {
        var rep = player.transform;
        rep.LookAt(transform.position);
        AudioSource.PlayClipAtPoint(die, transform.position, volume+0.5f);
        var bloodSplat = Instantiate(AshesDeath, transform.position, rep.rotation);
        bloodSplat.Play();
        Drop();
        Destroy(gameObject);
   }

    private void Drop()
    {
        if (Random.Range(1,20) > 10)
        {
           GameObject x =Instantiate(drop, transform.position, transform.rotation) as GameObject;
        }
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
