
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] internal float health = 50f;
    [SerializeField] internal float damage = 10f;

    internal GameObject player;

    internal PlayerMovement playerMovement;

    public Rigidbody rigidBody;
    public Collider monsterCollider;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip die;
    public AudioClip[] hurts;
    internal float volume;

    public GameObject drop;

    
    private Collider playerCollider;
    public ParticleSystem AshesDamage;
    public ParticleSystem AshesDeath;

    public int level;

    public DamagePopUp dmgPopUp;
    private bool died = false;

    protected void Start()
    {
        volume = PlayerPrefs.GetFloat("Volume");

        if (rigidBody == null) rigidBody = transform.GetComponent<Rigidbody>();
        if (monsterCollider == null) monsterCollider = rigidBody.GetComponent<Collider>();

        if (player == null) player = transform.parent.parent.GetComponentInParent<Room>().player;
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
    private bool dead = false;
    public void TakeDamage(int damage, bool isCrit,float critMulti)
    {
        float amount = damage;
        amount +=(int) Random.Range(-amount*.30f, amount * .30f);
        if (isCrit) amount *= 2+ critMulti/100;
        health -= amount;
        DmgPopUp(amount, isCrit);
        if (health <= 0f )
        {
            if (!dead) transform.parent.GetComponent<Room>().killMonster();
            Die();
        } 
        else
        {
            Bleed();

        }
    }

    private void DmgPopUp(float amount, bool isCrit)
    {
        
        var rep = player.transform;
        rep.LookAt(transform.position);
        var x = Instantiate(dmgPopUp, transform.position, rep.rotation, null);
        //x.gameObject.transform.parent = null;
        x.damageLabel.text = amount.ToString("F0");
        x.player = rep;
        if (isCrit) x.damageLabel.color = Color.yellow;
    }

    private void Bleed()
    {
        var rep = player.transform;
        rep.LookAt(transform.position);
        audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume);
        var bloodSplat = Instantiate(AshesDamage, transform.position, rep.rotation);
        bloodSplat.Play();
    }

    void Die()
    {
        dead = true;
        Bleed();
        var rep = player.transform;
        rep.LookAt(transform.position);
        AudioSource.PlayClipAtPoint(die, transform.position, volume+0.5f);
        var bloodSplat = Instantiate(AshesDeath, transform.position, rep.rotation);
        bloodSplat.Play();
        if (!died) Drop();
        died = true;
        Destroy(gameObject);
    }

    private void Drop()
    {
        if (Random.Range(1,20) > 1)
        {
           GameObject x =Instantiate(drop, transform.position, transform.rotation) as GameObject;
            x.transform.parent = transform.parent;
            var y = x.GetComponent<Item>();
            y.gun = y.CreateWeapon(level);

        }
    }





}
