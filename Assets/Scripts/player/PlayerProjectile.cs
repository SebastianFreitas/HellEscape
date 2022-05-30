using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public GameObject physicalTrail;
    public GameObject fireTrail;
    public GameObject coldTrail;
    public GameObject poisonTrail;


    public PlayerBasicMovement playerMov;

    public PlayExplosion exp;

    public GameObject trails;

    internal PlayerInventory playerInv;

    public GunOfAType Gun { get => gun; set => gun = value; }

    
    private float newSizeMulti;



    internal void AwakeRemote()
    {
        newSizeMulti  = (1 + gun.increasedBulletSize / 100);

        transform.localScale *= newSizeMulti;

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

        if (stats.fireDamage != 0)
        {
            fireTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;
            fireTrail.SetActive(true);
        }
        if (stats.coldDamage != 0) 
        {
            coldTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;
            coldTrail.SetActive(true);
        }
        if (stats.poisonDamage != 0)
        {
            poisonTrail.GetComponent<TrailRenderer>().widthMultiplier *= newSizeMulti;
            poisonTrail.SetActive(true);
        }
    }

    internal void SetVisibility(bool onOff)
    {
       // this.GetComponentInChildren<TrailRenderer>().enabled = onOff;
    }

    IEnumerator waiter(float a){
    yield return new WaitForSeconds(a);

            Destroy(gameObject);

    }

    IEnumerator fadeWaiter()
    {
        SetVisibility(false);
        yield return new WaitForSeconds(.055f);
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

        stats = new BulletStats(fireDamage, coldDamage, poisonDamage, physicalDamage, critMulti);
    }

    void OnCollisionEnter(Collision collision)
    {

        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.transform.GetComponentInParent<Monster>().TakeDamage( stats, false, stats.critMulti);

            StartCoroutine(KillBullet());
        }
        else if (collision.gameObject.CompareTag("MonsterHead"))
        {
            collision.transform.GetComponent<Monster>().TakeDamage( stats, true, stats.critMulti);

            StartCoroutine(KillBullet());
        }



        if (bounces > 0)
        {
            RicochetSparkAndSound();
            if (playerInv.RicochetStack) bounceSpeed *= 2;
            rb.AddForce(contact.normal * bounceSpeed);
            transform.LookAt(contact.normal);

            bounces--;

            if (stats.fireDamage > 0)
            {
                //if (!playerInv.explosiveRicochet) StartCoroutine(KillBullet());
                FireExplode();
            }

        } else StartCoroutine(KillBullet());
        

        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
     //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
     Gizmos.DrawWireSphere(transform.position, 2f*(1f + (playerInv.increasedFireArea / 100f)));
    }

    internal void FireExplode()
    {
        float areaModifier = DetectExplosion();
        ExplodeParticule(areaModifier);
    }

    private float DetectExplosion()
    {
        float areaModifier = (1f + (playerInv.increasedFireArea / 100f));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f * areaModifier);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 direction = hitCollider.transform.position - transform.position;

            if (playerInv.pullfire) direction = transform.position - hitCollider.transform.position;

            if (hitCollider.CompareTag("Dude"))
            {
                playerMov.AddImpact(direction, stats.fireDamage * 5 * playerInv.pushForceModifier);
            }
            else if (hitCollider.CompareTag("Monster"))
            {
                var monster = hitCollider.GetComponentInParent<Monster>();
                monster.TakeDamage(new BulletStats(stats.fireDamage, 0, 0, 0, 0), false, 0);
                //monster.GetComponent<Rigidbody>().AddForce((hitCollider.transform.position - transform.position) * 5f, ForceMode.Impulse);
            }
            else if (hitCollider.CompareTag("MonsterHead"))
            {
                var monster = hitCollider.GetComponentInParent<Monster>();
                monster.TakeDamage(new BulletStats(stats.fireDamage, 0, 0, 0, 0), false, 0);
                monster.GetComponent<Rigidbody>().AddForce(direction * 5f * playerInv.pushForceModifier, ForceMode.Impulse);
            }
            else if (hitCollider.CompareTag("Prop"))
            {
                hitCollider.GetComponent<Rigidbody>().AddForce(direction * 5f * playerInv.pushForceModifier, ForceMode.Impulse);
            }


        }

        return areaModifier;
    }

    internal void ExplodeParticule(float area)
    {
        if (playerInv.delayedFire)
        {
            PlayExplosion play = Instantiate(exp, transform.position, transform.rotation) as PlayExplosion;
            play.RemoteAwake(playerInv, playerMov, stats);
        }

        PLayExplosionAnimation(area);
        if (!playerInv.explosiveRicochet)
            Destroy(this);
    }

    private void PLayExplosionAnimation(float area)
    {
        exp.gameObject.SetActive(true);
        var exp2 = Instantiate(exp, transform.position, transform.rotation);
        exp2.transform.localScale *= area;
        var explode = exp2.GetComponent<ParticleSystem>();


        explode.Play();

        Destroy(exp2, explode.main.duration);
    }

    IEnumerator DelayedFire()
    {
        yield return new WaitForSecondsRealtime(.5f);
        PLayExplosionAnimation(DetectExplosion());
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

    internal void RicochetSparkAndSound()
    {
        var sparkBounce = Instantiate(spark, transform.position, Quaternion.Inverse(transform.rotation));
        sparkBounce.Play();
        AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position, 0.2f);
    }

    internal IEnumerator KillBullet()
    {

       // StartCoroutine(exp.GetComponent<KillObject>().WaitDie(3f));
        if (stats.fireDamage != 0 ) exp.transform.parent = null;
        
        trails.transform.parent = null;


        yield return new WaitForSeconds(.1f);
        Destroy(this.gameObject);
        
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
