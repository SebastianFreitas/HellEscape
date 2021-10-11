
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

    public int level;

    public DamagePopUp dmgPopUp;



    protected void Start()
    {
        level = transform.parent.GetComponent<Room>().areaLevel;
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

    public void TakeDamage(int fireDamage, int coldDamage, int poisonDamage, int physicalDamage)
    {
        var amount = fireDamage + coldDamage + poisonDamage + physicalDamage;
        health -= amount;
        DmgPopUp(amount);
        if (health <= 0f )
        {
            transform.parent.GetComponent<Room>().killMonster();
            Die();
        } 
        else
        {
            Bleed();

        }
    }

    private void DmgPopUp(int amount)
    {
        var rep = player.transform;
        rep.LookAt(transform.position);
        var x = Instantiate(dmgPopUp, transform.position, rep.rotation, null);
        //x.gameObject.transform.parent = null;
        x.damageLabel.text = amount.ToString("F0");
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
        Bleed();
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
        if (Random.Range(1,20) > 1)
        {
           GameObject x =Instantiate(drop, transform.position, transform.rotation) as GameObject;
            var y = x.GetComponent<Item>();
            y.gun = y.CreateWeapon(level);

        }
    }





}
