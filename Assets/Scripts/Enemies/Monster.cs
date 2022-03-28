
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

    public float level;

    internal float actionSpeed = 1f;

    public DamagePopUp dmgPopUp;
    private bool died = false;
    private bool isElite = false;
    internal RoomActivator roomActivator;
    internal ModDataRoom.GeneratedMission mission;
    protected void Awake()
    {
        volume = PlayerPrefs.GetFloat("Volume");

        if (rigidBody == null) rigidBody = transform.GetComponent<Rigidbody>();
        if (monsterCollider == null) monsterCollider = rigidBody.GetComponent<Collider>();

        if (player == null) player = GameObject.FindGameObjectsWithTag("Dude")[0];
        playerCollider = player.transform.GetComponent<Rigidbody>().GetComponent<Collider>();
        roomActivator = transform.GetComponentInParent<RoomActivator>();
        mission = roomActivator.mission;
        ApplyMission();
        
        if (Random.Range(1f, 100f) > 100 - eliteChance) TurnElite();
    }
    float eliteChance;
    int gunparts = 1;
    float totalDropChance = 1f;
    private void ApplyMission()
    {
        health += mission.aditionalLife;
        damage += mission.aditionalDamage;
        eliteChance = 5 + mission.increasedChanceElite;

        gunparts = 1;
        totalDropChance *= 1 + (((float)mission.increasedMonsterDrops * 3) / 100);

        if (isElite)
        {
            gunparts = 10;
            totalDropChance += 10;
        }

        if (mission.doubleLife) health*= 2;
        if (mission.doubleDrops)
        {
            totalDropChance = (totalDropChance-1) * 2 + 1;
        }


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

    [SerializeField] internal bool isHub;
    private GameObject explosion;

    public void TakeDamage(int damage, bool isCrit,float critMulti)
    {
        float amount = damage;
        amount +=(int) Random.Range(-amount*.30f, amount * .30f);
        if (isCrit) amount *= 2+ critMulti/100;
        health -= amount;
        DmgPopUp(amount, isCrit);
        if (health <= 0f )
        {
            if (!dead && !isHub) transform.GetComponentInParent<Room>().killMonster();
            Die();
        } 
        else
        {
            Bleed();

        }
    }

    internal void UpdateStatsToLevel()
    {
        health *= 1f + (level / 100f);
        actionSpeed *= 1f + (level / 100f/2);

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
        if (!died)
        {
            if (mission.deathExplosion)
            {
                var explo = Instantiate(explosion, transform.position, transform.rotation, transform);
                explo.GetComponent<ExplosiveCilinder>().Explode(transform.position, 6f);
            }
                
            roomActivator.IsEncounterDone();
            Drop();
        }
        died = true;
        Destroy(gameObject);
    }

    internal enum EliteType
    {
        Mini,
        Big,
        Pack,
    }

    internal void TurnElite()
    {
        isElite = true;
        EliteType type = (EliteType)Random.Range(0, System.Enum.GetValues(typeof(EliteType)).Length);
        switch (type)
        {
            case EliteType.Mini:
                TurnMini();
                break;

            case EliteType.Big:
                TurnBig();
                break;           
        }
    }

    private void TurnBig()
    {
        transform.localScale *= 2f;
        health *= 4;
        damage *= 2;
    }

    private void TurnMini()
    {
        transform.localScale *= 0.5f;
        actionSpeed += .25f;
    }
    
    private void Drop()
    {
         
        player.transform.GetComponent<PlayerInventory>().UpdateGunParts(gunparts);



        if (Random.Range(1f,100f) > 100-totalDropChance)
        {
           GameObject x =Instantiate(drop, transform.position, transform.rotation) as GameObject;
            x.transform.parent = transform.parent;
            var y = x.GetComponent<Item>();
            y.gun = y.CreateWeapon((int)level, false);

        }
    }





}
