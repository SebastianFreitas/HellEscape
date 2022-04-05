using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct convergion
{
  public float bludgeoning;
  public float piercing;
  public float slashing;

}
public class PlayerProjectile : MonoBehaviour
{
    public GameObject damagePopup;
    public float bounceSpeed = 1000f;
    private float speed;

    public Vector3 playerSpeed;
    public AudioClip ricochet;
    public AudioSource source;

    public ParticleSystem spark;

    public GameObject newBullet;

    public bool visual = false;

    public bool initialFade = false;
    private int bulletGuided;
    public int bounces = 2;
    Rigidbody rb;

    private GunOfAType gun;

    internal BulletStats stats;
    public int fireDamage;
    public int coldDamage;
    public int poisonDamage;
    public int physicalDamage;
    internal float critMulti;

    public GameObject physicalTrail;
    public GameObject fireTrail;
    public GameObject coldTrail;
    public GameObject poisonTrail;


    public PlayerBasicMovement playerMov;

    public KillObject exp;

    public GameObject trails;


    public GunOfAType Gun { get => gun; set => gun = value; }

    internal Transform[] enemies;
    private float newSizeMulti;

    void Start()
    {
        newSizeMulti  = (1 + gun.increasedBulletSize / 100);
        //transform.localScale = new Vector3(newSizeMulti, newSizeMulti, newSizeMulti);
        transform.GetComponent<SphereCollider>().radius *= newSizeMulti;

        SetVisibility(false);
        rb = GetComponent<Rigidbody>();
        ConfigureTrails();

        if (initialFade) StartCoroutine(fadeWaiter());//this line will fuck up (usual bug andar pa tras ou pa frente + double bounce com side walk)
        rb.AddForce(transform.forward * speed);
        bounceSpeed = speed / 2;
        StartCoroutine(waiter(10f));
    }

    private void ConfigureTrails()
    {
        physicalTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;

        if (fireDamage != 0)
        {
            fireTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;
            fireTrail.SetActive(true);
        }
        if (coldDamage != 0) 
        {
            coldTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;
            coldTrail.SetActive(true);
        }
        if (poisonDamage != 0)
        {
            poisonTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;
            poisonTrail.SetActive(true);
        }
    }

    private void SetVisibility(bool onOff)
    {
        //this.GetComponentInChildren<TrailRenderer>().enabled = onOff;
    }

    IEnumerator waiter(float a){
    yield return new WaitForSeconds(a);
    Destroy(gameObject);
    }

    IEnumerator fadeWaiter()
    {
    SetVisibility(false);
    yield return new WaitForSeconds(.005f);
    SetVisibility(true);
    }

    void OnEnable()
    {
        StartCoroutine(waiter(10f));
    }

    public void SetStats(int bulletguided,  int bounces, int bulletSpeed, int fireDamage, int coldDamage, int poisonDamage, int physicalDamage, float critMulti)
    {
        this.bulletGuided = bulletguided;
        this.bounces = bounces;
        this.speed = bulletSpeed;
        this.fireDamage = fireDamage;
        this.coldDamage = coldDamage;
        this.poisonDamage = poisonDamage;
        this.physicalDamage = physicalDamage;
        this.critMulti = critMulti;

        stats = new BulletStats(fireDamage, coldDamage, poisonDamage, physicalDamage, critMulti);
    }

    void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.transform.parent.GetComponentInParent<Monster>().TakeDamage( stats, false, critMulti);

            StartCoroutine(KillBullet());
        }
        else if (collision.gameObject.CompareTag("MonsterHead"))
        {
            collision.transform.GetComponent<Monster>().TakeDamage( stats, true, critMulti);

            StartCoroutine(KillBullet());
        }

        if (fireDamage > 0)
        {
            StartCoroutine(KillBullet());
            FireExplode();
            
        }
        else 
        {
            if (bounces > 0)
            {
                SetVisibility(true);

                RicochetSparkAndSound();

                if (bulletGuided > 0)
                {
                    bool foundEnemy = GuidedBullet();
                    if (!foundEnemy) rb.AddForce(contact.normal * bounceSpeed);
                }
                else rb.AddForce(contact.normal * bounceSpeed);

                bounces--;
            } else StartCoroutine(KillBullet());
        

        }
    }

    private void FireExplode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Dude"))
            {
                playerMov.AddImpact(hitCollider.transform.position - transform.position, fireDamage);
                //playerMov.GainSpeed(2);
            }
            else if (hitCollider.CompareTag("Monster") || hitCollider.CompareTag("MonsterHead"))
            {
                hitCollider.GetComponent<Monster>().TakeDamage(new BulletStats(stats.fireDamage, 0, 0, 0, 0), false, 0);
            }
            else if (hitCollider.CompareTag("Prop"))
            {
                hitCollider.GetComponent<Rigidbody>().AddForce((hitCollider.transform.position - transform.position) * 5f, ForceMode.Impulse);
            }

            ExplodeParticule();
        }
       // Destroy(this.gameObject);
    }

    void ExplodeParticule()
    {
        exp.gameObject.SetActive(true);
        var explode = exp.GetComponent<ParticleSystem>();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        explode.Play();
        Destroy(gameObject, explode.main.duration);

    }


    private bool GuidedBullet()
    {
        var foundEnemy = false;
        Collider[] hitColliders = Physics.OverlapSphere(rb.position, 10f);
        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.CompareTag("Monster"))
            {
                foundEnemy = true;
                var direction = hitCollider.transform.position - transform.position;
                rb.AddForce(direction * bounceSpeed);
                break;
            }
        }

        return foundEnemy;
    }

    private void RicochetSparkAndSound()
    {
        var sparkBounce = Instantiate(spark, transform.position, Quaternion.Inverse(transform.rotation));
        sparkBounce.Play();
        AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position, 0.2f);
    }

    private IEnumerator KillBullet()
    {

        StartCoroutine(exp.GetComponent<KillObject>().WaitDie(3f));
        if (fireDamage != 0 ) exp.transform.parent = null;
        
        trails.transform.parent = null;


        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
