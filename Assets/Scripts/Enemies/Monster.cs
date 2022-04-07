
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
    private Collider monsterCollider;

    [Header("Sound")]
    private AudioSource audioSource;
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
    internal bool isElite = false;
    internal RoomActivator roomActivator;
    internal ModDataRoom.GeneratedMission mission;
    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("Volume");

        if (rigidBody == null) rigidBody = transform.GetComponent<Rigidbody>();
        if (monsterCollider == null) monsterCollider = rigidBody.GetComponent<Collider>();

        if (player == null) player = GameObject.FindGameObjectsWithTag("Dude")[0];
        playerCollider = player.transform.GetComponent<Rigidbody>().GetComponent<Collider>();

        if (!isHub)
        {

            roomActivator = transform.GetComponentInParent<RoomActivator>();
            mission = roomActivator.mission;
            ApplyMission();

            if (Random.Range(1f, 100f) > 100 - eliteChance) TurnElite();
        }
        else mission = new ModDataRoom.GeneratedMission();

    }
    float eliteChance;
    int gunparts = 1;
    float totalDropChance = 1f;
    private void ApplyMission()
    {
        health += mission.aditionalLife;
        damage += mission.aditionalDamage;
        eliteChance = 5 + mission.increasedChanceElite;

        gunparts = 1 + mission.additionalGunParts;
        totalDropChance *= 1 + (((float)mission.increasedMonsterDrops * 3) / 100);

        actionSpeed += mission.increasedActionSpeed/100;
        
        if (mission.isBig) transform.localScale *= 1.5f;

        if (mission.doubleLife) health*= 2;
        if (mission.doubleDrops) totalDropChance = (totalDropChance-1) * 2 + 1;

        if (mission.tick) StartCoroutine("Tick");

        if (isElite) TurnElite();


    }

    IEnumerator Tick()
    {
        isTick = true;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(.3f, .9f));
            rigidBody.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 500f);
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

    [SerializeField] internal bool isHub = false;
    [SerializeField] GameObject explosion;
    private bool isTick;

    internal void TakeDamage(BulletStats stats, bool isCrit,float critMulti)
    {
        if (mission.fireImmunity) stats.fireDamage = 0;

        if (mission.coldImmunity ) stats.coldDamage = 0;
        else
        {
            if (stats.coldDamage > 0)
            {
                StopCoroutine("Chilled");
                StartCoroutine("Chilled");
            }

        }

        if (mission.poisonImmunity) stats.poisonDamage = 0;
        else
        {
            if (stats.poisonDamage > 0)
            {
                //StartCoroutine(Poisoned(stats.poisonDamage));
                StartPoison(stats.poisonDamage);
            }

        }

        if (mission.physicalImmunity) stats.physicalDamage = 0;
 
        
        if (isCrit) stats.physicalDamage *= 2f + critMulti/100f;
        
        float amount = stats.GetDamage();

        if(amount > 0)
        {
            amount +=(int) Random.Range(-amount*.30f, amount * .30f);
    
            health -= amount;
            DmgPopUp(amount, isCrit);

            if (health <= 0f ) Die();
            else Bleed();  
        }
    }

    internal void TakeDamage(int amount)
    {
        health -= amount;
        DmgPopUp(amount, false);
        if (health <= 0f)
        {

            Die();
        }

    }

    internal void UpdateStatsToLevel()
    {
        health *= 1f + (level / 100f);
        actionSpeed *= 1f + (level / 100f/2);

    }

    private void DmgPopUp(float amount, bool isCrit)
    {

        var direction = transform.position - player.transform.position ;
        var rot = Quaternion.LookRotation(direction);
        
        var x = Instantiate(dmgPopUp, transform.position, rot, null);

        x.damageLabel.text = amount.ToString("F0");
        //x.player = rot;
        if (isCrit) x.damageLabel.color = Color.red;
    }

    private void Bleed()
    {
        var rep = player.transform;
        rep.LookAt(transform.position);
       // audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], 0.1f);
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
            died = true;
            if (mission.deathExplosion)
            {
                var explo = Instantiate(explosion, transform.position, transform.rotation, transform);
                explo.transform.parent = null;
                explo.GetComponent<ExplosiveCilinder>().Explode(transform.position, 5f);
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

        gunparts *= 5;
        totalDropChance += 10;
    }

    private void TurnBig()
    {
        transform.localScale *= 3f;
        rigidBody.mass *= 10;
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

        if (Random.Range(1f, 100f) > 100 - (totalDropChance*2))
        {
            GameObject x = Instantiate(roomActivator.roomgen.healthPack, transform.position + Vector3.up, transform.rotation) as GameObject;
            x.transform.parent = transform.parent;

        }
    }
    private void OnEnable()
    {
        if (isTick) StartCoroutine("Tick");
    }

    IEnumerator Chilled()
    {
        actionSpeed -= 0.5f;
        yield return new WaitForSecondsRealtime(3f);
        actionSpeed += 0.5f;
    }

    private float poisonValue = 0;
    private bool isPoisoned = false;
    private int poisonTicks = 0;
    IEnumerator Poisoned()
    {
        WaitForSecondsRealtime waiter = new WaitForSecondsRealtime(.5f);
        isPoisoned = true;

        while (true)
        {
            TakeDamage((int)poisonValue);

            poisonTicks++;
            if (poisonTicks > 9) break;

            yield return waiter;

        }

        poisonTicks = 0;
        isPoisoned = false;
    }

    void StartPoison(float poisonDamage)
    {
        if (isPoisoned)
        {
            poisonTicks = 0;
            poisonValue += poisonDamage / 5;  
        }
        else
        {
            StartCoroutine(Poisoned());
        }
    }

}
