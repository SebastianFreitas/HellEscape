
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] internal float health = 50f;
    [SerializeField] internal float damage = 10f;

    internal GameObject player;

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
    internal float forceSpeed = 1f;

    public DamagePopUp dmgPopUp;
    private bool died = false;
    internal bool isElite = false;
    internal RoomActivator roomActivator;
    internal ModDataRoom.GeneratedMission mission;
    internal BridgeHandler path;

    private PlayerBasicMovement playerMov;
    private PlayerHpManager playerHP;
    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("Volume");

        if (rigidBody == null) rigidBody = transform.GetComponent<Rigidbody>();
        if (monsterCollider == null) monsterCollider = rigidBody.GetComponent<Collider>();

        if (player == null) player = GameObject.FindGameObjectsWithTag("Dude")[0];
        playerCollider = player.transform.GetComponent<Rigidbody>().GetComponent<Collider>();

        playerMov =  player.GetComponent<PlayerBasicMovement>();
        playerHP = player.GetComponent<PlayerHpManager>();

        if (!isHub)
        {

            roomActivator = transform.GetComponentInParent<RoomActivator>();
            mission = roomActivator.mission;
            ApplyMission();


        }
        else
        {
            mission = new ModDataRoom.GeneratedMission();
            level = PlayerPrefs.GetInt("PathLevel");
            UpdateStatsToLevel();
        }

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

        gunparts = 1 + mission.additionalGunParts;
        totalDropChance *= 1 + (((float)mission.increasedMonsterDrops * 3) / 100);

        actionSpeed += mission.increasedActionSpeed/100;
        
        if (mission.isBig) transform.localScale *= 1.5f;

        if (mission.doubleLife) health*= 2;
        if (mission.doubleDrops) totalDropChance = (totalDropChance-1) * 2 + 1;

        if (mission.tick) StartCoroutine("Tick");

        if (isElite) TurnElite();

        if (mission.invisible) StartCoroutine("Invisible");
    }


    private bool isInvisible = false;
    IEnumerator Invisible()
    {
        isInvisible = true;

        var mesh = GetComponentsInChildren<MeshRenderer>();
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2,6));
            foreach (var current in mesh) current.enabled = false;

            yield return new WaitForSeconds(1);
            foreach (var current in mesh) current.enabled = true;
        }

    }
    private bool hasCollide = false;

    void FixedUpdate(){
        if (monsterCollider.bounds.Intersects(playerCollider.bounds))
        {
            if (hasCollide == false)
            {
                var direction = player.transform.position- transform.position;

                playerMov.AddImpact(direction, 100f + mission.kockBack);
                playerHP.TakeDamage((int)damage);

                if (mission.chill) playerMov.StartCoroutine("Chilled");
                StartCoroutine("WaitDamage");
            }
        }
    }

    IEnumerator WaitDamage()
    {
        yield return new WaitForSecondsRealtime(.5f);
        hasCollide = false;
    }

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
        health *= 1f + ((float)level / 100f);

        var speed = 1f + ((float)level / 100f);
        if (speed > 1.3) speed = 1.3f;
        actionSpeed *= speed;

        forceSpeed *= 1f + ((float)level / 100f );

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

            if (!isFiller && !isHub)roomActivator.IsEncounterDone();
            Drop();
        }
        died = true;

        var x = GetComponentInChildren<TrailRenderer>();
        if (x != null) x.transform.parent = null;

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

    internal void TurnBig()
   {
        transform.localScale *= 3f;
        rigidBody.mass *= 4;
        health *= 4;
        damage *= 2;
        forceSpeed *= 8;
    }

    internal void TurnMini()
    {
        transform.localScale *= 0.75f;
        actionSpeed += .25f;
        forceSpeed *= 2;
    }
    
    private void Drop()
    {
        if (!isHub)
        {
            player.transform.GetComponent<PlayerInventory>().UpdateGunParts(gunparts);



            if (Random.Range(1f,100f) > 100-totalDropChance + mission.monsterWeaponDropChance)
            {
               GameObject x =Instantiate(drop, transform.position, transform.rotation) as GameObject;
                x.transform.parent = transform.parent;
                var y = x.GetComponent<Item>();
                y.gun = y.CreateWeapon((int)level, false);

            }

            if (Random.Range(1f, 100f) > 100 - (totalDropChance*2 + mission.monsterHealthDropChance))
            {
                GameObject x = Instantiate(roomActivator.roomgen.healthPack, transform.position + Vector3.up, transform.rotation) as GameObject;
                x.transform.parent = transform.parent;

            }
        }

    }
    private void OnEnable()
    {
        if (isInvisible) StartCoroutine("Invisible");
        if (isPoisoned) StartCoroutine("Poisoned");
        if (isChilled) StartCoroutine("Chilled");
    }


    private bool isChilled = false;
    IEnumerator Chilled()
    {
        actionSpeed -= 0.5f;
        isChilled = true;
        yield return new WaitForSecondsRealtime(3f);
        isChilled = false;
        actionSpeed += 0.5f;
    }

    private float poisonValue = 0;
    private bool isPoisoned = false;
    private int poisonTicks = 0;
    internal bool isFiller = false;

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
            poisonValue = poisonDamage / 5;
            StartCoroutine(Poisoned());
        }
    }

}
