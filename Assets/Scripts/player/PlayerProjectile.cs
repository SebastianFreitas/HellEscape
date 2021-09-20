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

    public int bounces = 2;
    Rigidbody rb;

    private GunOfAType gun;

    public FadeTrailBullet trail;

    public int fireDamage;
    public int coldDamage;
    public int poisonDamage;
    public int physicalDamage;

    public GameObject physicalTrail;
    public GameObject fireTrail;
    public GameObject coldTrail;
    public GameObject poisonTrail;


    public GunOfAType Gun { get => gun; set => gun = value; }

    void Start()
    {
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
        if (gun.type.Equals(GunType.normal))physicalTrail.GetComponent<TrailRenderer>().time = 0.2f;
        if (gun.type.Equals(GunType.sniper)) physicalTrail.GetComponent<TrailRenderer>().time = 0.4f;
        else physicalTrail.GetComponent<TrailRenderer>().time = 0.05f;

        if (fireDamage != 0) fireTrail.SetActive(true);
        if (coldDamage != 0) coldTrail.SetActive(true);
        if (poisonDamage != 0) poisonTrail.SetActive(true);
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

    public void SetStats( int bounces, int bulletSpeed, int fireDamage, int coldDamage, int poisonDamage, int physicalDamage)
    {
        this.bounces = bounces;
        this.speed = bulletSpeed;
        this.fireDamage = fireDamage;
        this.coldDamage = coldDamage;
        this.poisonDamage = poisonDamage;
        this.physicalDamage = physicalDamage;
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.transform.GetComponent<Monster>().TakeDamage(fireDamage, coldDamage, poisonDamage, physicalDamage);

            Destroy(this.gameObject);
        }

        if (bounces > 0)
        {
            SetVisibility(true);

            var sparkBounce = Instantiate(spark, transform.position, Quaternion.Inverse(transform.rotation));
            sparkBounce.Play();

            AudioSource.PlayClipAtPoint(ricochet, this.gameObject.transform.position, 0.2f);
            rb.AddForce(contact.normal * speed);
            bounces--;
        }
        else
        {
            //StartCoroutine(KillBullet());
            
        }
    }

    private IEnumerator KillBullet()
    {

        //trail.StartCoroutine(trail.KillTrail());
        physicalTrail.transform.parent = null;
        fireTrail.transform.parent = null;
        coldTrail.transform.parent = null;
        poisonTrail.transform.parent = null;
        trail.transform.parent = null;
        this.transform.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
