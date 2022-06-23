
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] internal float health = 50f;
    [SerializeField] internal float damage = 10f;
    [SerializeField] bool isBoss = false;
    internal GameObject player;

    public Rigidbody rigidBody;
    [SerializeField] private Collider monsterCollider;

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
    private float oldActionSpeed;
    internal float actionSpeed = 1f;
    private bool isFrozzen;
    internal float forceSpeed = 1f;

    public DamagePopUp dmgPopUp;
    private bool died = false;
    internal bool isElite = false;
    internal RoomActivator roomActivator;
    internal ModDataRoom.GeneratedMission mission;
    internal BridgeHandler path;

    private PlayerBasicMovement playerMov;
    private PlayerInventory playerInv;
    private PlayerHpManager playerHP;

    float eliteChance;
    int gunparts = 1;
    float totalDropChance = 1f;
    private HealthBar healthUI;
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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("Volume");

        if (rigidBody == null) rigidBody = transform.GetComponent<Rigidbody>();
        if (monsterCollider == null) monsterCollider = rigidBody.GetComponent<Collider>();

        if (player == null)
            player = transform.root.GetComponent<GameMan>().player;
        if (player == null)
            player = FindObjectOfType<PlayerBasicMovement>().gameObject;

        playerCollider = player.transform.GetComponent<Rigidbody>().GetComponent<Collider>();

        playerMov =  player.GetComponent<PlayerBasicMovement>();
        playerHP = player.GetComponent<PlayerHpManager>();
        playerInv = player.GetComponent<PlayerInventory>();

        if(playerInv.moreFrozenGunparts) gunparts+=Random.Range(0,3);

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
        if (!isFiller)
        {
            if (isBoss)
            {
                gunparts += 25;
                healthUI = transform.root.GetComponent<GameMan>().hpBarBoss;
                healthUI.gameObject.SetActive(true);
                healthUI.SetMaxHealth((int)health);
                healthUI.SetHealth((int)health);
            }
            else if (Random.Range(1f, 100f) > 100 - eliteChance) TurnElite();
        }


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
                //var direction = player.transform.position- transform.position;

                //playerMov.AddImpact(direction, 100f + mission.kockBack);
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

    internal void TakeDamage(BulletStats stats, bool isCrit,float critMulti)
    {
        if (mission.fireImmunity) stats.fireDamage = 0;
        else
        {
            if (stats.fireDamage > 0)
            {
                if (Random.Range(1f, 100f) > 100 - playerInv.igniteChance)
                {
                    StartIgnite( stats);
                }
            }

        }

        if (mission.coldImmunity ) stats.coldDamage = 0;
        else
        {
            if (stats.coldDamage > 0)
            {
                StopCoroutine("Chilled");
                StartCoroutine("Chilled");
                if (Random.Range(1f, 100f) > 100 - (1+playerInv.freezeChance))
                {
                    StopCoroutine("Freezed");
                    StartCoroutine("Freezed");
                }

            }

        }

        if (mission.poisonImmunity) stats.poisonDamage = 0;
        else
        {
            if (stats.poisonDamage > 0)
            {
                if (isChilled) stats.poisonDamage *= playerInv.chillPoison;
                StartPoison( stats);
            }

        }

        if (mission.physicalImmunity) stats.physicalDamage = 0;
        else
        {
            if(playerInv.chancePhysDoubleDamage > 0)
            {
                if (Random.Range(1f, 100f) > 100 - playerInv.chancePhysDoubleDamage) stats.physicalDamage *= 2;
            }

            if (isFrozzen) stats.physicalDamage *= playerInv.doublePhysOnFreezes;

            if (isCrit)
            {
                
                if(playerInv.phyisToColdCrit)
                {
                    stats.coldDamage = stats.physicalDamage;
                    stats.physicalDamage = 0;
                }

                stats.physicalDamage *= playerInv.critGlobalMultiplier + critMulti / 100f;
            }

            if (Random.Range(1f, 100f) > 100 - (1 + playerInv.bleedChance))
            {
                StartBleeding(stats);
            }
        }

       

       
        
        float amount = stats.GetDamage() ;

        if (isFrozzen) amount *= playerInv.extraDamageWhileFrozen;

        if(amount > 0)
        {
            amount +=(int) Random.Range(-amount*.30f, amount * .30f);
    
            health -= amount;
            if (isBoss) healthUI.SetHealth((int)health);
            DmgPopUp(amount, isCrit);

            if (health <= 0f ) Die(stats);
            else Bleed();  
        }
    }

    internal void TakeDamage(float amount, BulletStats stats)
    {
        health -= amount;
        if (isBoss) healthUI.SetHealth((int)health);
        DmgPopUp(amount, false);
        if (health <= 0f)
        {

            Die(stats);
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

    void Die(BulletStats stats)
    {

        Bleed();
        var rep = player.transform;
        rep.LookAt(transform.position);
        AudioSource.PlayClipAtPoint(die, transform.position, volume+0.5f);
        var bloodSplat = Instantiate(AshesDeath, transform.position, rep.rotation);
        bloodSplat.Play();
        if (!died)
        {

            if (isBoss) healthUI.gameObject.SetActive(false);

            died = true;
            if (playerInv.fireDeath)
            {
                PlayExplosion explo = Instantiate(playExplosion, transform.position, transform.rotation, transform);
                explo.RemoteAwake(playerInv, playerMov, stats);
            }

            for (int i = 0; i < playerInv.coldShatter; i++)
            {
                var gun = playerInv.GetComponentInChildren<Gun>();
                for (int a = 0; a < 8; a++)
                {
                    var y = gun.SpawnBullet(rigidBody.transform, true);
                    y.transform.parent = null;
                    //var newRotation = y.transform.rotation * Quaternion.AngleAxis(60f, Random.insideUnitCircle);
                    //y.transform.rotation = newRotation;
                    y.isFilter = true;
                }

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

            if (Random.Range(1f, 100f) > 100 - (totalDropChance*2 + mission.monsterHealthDropChance + playerInv.vampiricBonus))
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
        if (isIgnited) StartCoroutine("Ignited");
        if (isBleeding) StartCoroutine("Bleeding");
    }


    private bool isChilled = false;
    IEnumerator Chilled()
    {
        var x = 0;
        var baseAct = actionSpeed;
        if (isPoisoned) x = 1;
        actionSpeed -= 0.5f + playerInv.poisonedChill*x;
        isChilled = true;
        yield return new WaitForSecondsRealtime(3f);
        isChilled = false;
        actionSpeed = baseAct;
    }

    private float poisonValue = 0;
    private bool isPoisoned = false;
    private int poisonTicks = 0;
    internal bool isFiller = false;
    [SerializeField] PlayExplosion playExplosion;

    IEnumerator Poisoned(BulletStats stats)
    {
        WaitForSecondsRealtime waiter = new WaitForSecondsRealtime(.5f / playerInv.poisonSpeedDouble);
        isPoisoned = true;
        if (playerInv.weakerPoison) damage *= 0.75f;
        while (true)
        {
            TakeDamage((int)poisonValue, stats);

            poisonTicks++;
            if (poisonTicks >  playerInv.poisonDuration) break;

            yield return waiter;

        }
        if (playerInv.weakerPoison) damage *= 1.25f;
        poisonTicks = 0;
        isPoisoned = false;
    }

    void StartPoison( BulletStats stats)
    {
        if (playerInv.instantPoison)
        {
            TakeDamage(stats.poisonDamage/5f * ((float)playerInv.poisonDuration), stats);
        }  
        else
        {
            if (isPoisoned)
            {
                poisonTicks = 0;

            }
            else
            {
                StartCoroutine(Poisoned(stats));
            }

            poisonValue += (stats.poisonDamage / 5);
        }

    }

    internal bool IsPoisoned()
    {
        return isPoisoned;
    }

    IEnumerator Freezed()
    {
        oldActionSpeed = actionSpeed;
        actionSpeed =0f;
        isFrozzen = true;
        yield return new WaitForSecondsRealtime(1f + playerInv.additionalFreezeDuration);
        isFrozzen = false;
        actionSpeed = oldActionSpeed;
    }

    private float igniteValue = 0;
    private bool isIgnited = false;
    private int igniteTicks = 0;

    IEnumerator Ignited(BulletStats stats)
    {
        WaitForSecondsRealtime waiter = new WaitForSecondsRealtime(.75f);
        isIgnited = true;

        while (true)
        {
            TakeDamage(igniteValue, stats);

            igniteTicks++;
            if (igniteTicks > playerInv.igniteDuration) break;

            yield return waiter;

        }

        igniteTicks = 0;
        isIgnited = false;
    }

    void StartIgnite( BulletStats stats)
    {

        
        if (isIgnited)
        {
            igniteTicks = 0;

        }
        else
        {
            StartCoroutine(Ignited(stats));
        }

        igniteValue += (stats.fireDamage / 2);
        

    }

    internal bool IsIgnited()
    {
        return isIgnited;
    }

    //Bleeding ------------------------------
    private float bleedValue = 0;
    private bool isBleeding = false;
    private int bleedingTicks = 0;

    IEnumerator Bleeding(BulletStats stats)
    {
        WaitForSecondsRealtime waiter = new WaitForSecondsRealtime(.1f);
        isBleeding = true;

        while (true)
        {
            TakeDamage(bleedValue, stats);

            bleedingTicks++;
            if (bleedingTicks > playerInv.bleedingDuration) break;

            yield return waiter;

        }

        bleedingTicks = 0;
        isBleeding = false;
    }

    void StartBleeding (BulletStats stats)
    {


        if (isBleeding)
        {
            bleedingTicks = 0;

        }
        else
        {
            StartCoroutine(Bleeding(stats));
        }

        bleedValue += (stats.physicalDamage / 10);


    }

    internal bool IsBleeding()
    {
        return isBleeding;
    }
}
