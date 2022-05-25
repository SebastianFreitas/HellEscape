using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpManager : MonoBehaviour
{
    private float health = 50;
    private float maxHealth = 50;

    private float baseHealth = 50;
    public HealthBar hp;
    internal bool canTakeDamage = true;
    public PlayerSounds playerSounds;

    public GameObject bloodScreen;

    private GameMan manager;

    private PlayerBasicMovement playerMov;



    void Awake()
    {
        //hp = GameObject.FindGameObjectsWithTag("HealthBar")[0].transform.GetComponent<HealthBar>();
        manager = transform.root.GetComponent<GameMan>();
        hp.SetMaxHealth((int)maxHealth);


        playerMov = GetComponent<PlayerBasicMovement>();//.isSideDashing;
    }

    private void OnEnable()
    {
        StartCoroutine(waiterImmunity());
    }

    public void TakeDamage(float amount)
   {
        if (!canTakeDamage || playerMov.isSideDashing) return;
     

        canTakeDamage = false;
        StartCoroutine(BloodScreen());
        StartCoroutine(waiterImmunity());
        playerSounds.PlayTakeDamageSound();



        health -= amount;

        hp.SetHealth((int)health);
        if (health <= 0f) Die();

        Debug.LogError(amount + "damage taken");
        Debug.LogError(health + "health");
    }


    internal void Heal(float amount)
    {
        if (manager.mission != null)
        {
            if (manager.mission.halfHealing) amount /= 2;
        }


        var result = health + amount;
        if (result > maxHealth)
        {
            hp.SetHealth((int)maxHealth);
            health = maxHealth;
        }
        else
        {
            hp.SetHealth((int)result);
            health = result;
        }
       
    }

    internal void HealForMax()
    {
        Heal((int)maxHealth);
    }

    internal void SetHPToBase()
    {
        hp.SetMaxHealth((int)baseHealth);
        hp.SetHealth((int) baseHealth);

        maxHealth = baseHealth;
        health = baseHealth;
    }

    internal void ChangeMaxHP(int amount)
    {
        hp.SetMaxHealth((int)maxHealth+amount);
        maxHealth += amount;

        health += amount;
        hp.SetHealth((int)health);

    }

    void Die()
    {
        transform.parent.parent.GetComponent<GameMan>().ReturnToHub();
        hp.SetHealth((int)maxHealth);
        health = maxHealth;
        transform.GetComponent<PlayerInventory>().LooseBoons();
    }

    IEnumerator BloodScreen()
    {
      bloodScreen.GetComponent<RawImage>().enabled = true;
      yield return new WaitForSecondsRealtime(.2f);
      bloodScreen.GetComponent<RawImage>().enabled = false;
    }

    IEnumerator waiterImmunity()
    {
        canTakeDamage = false;

        yield return new WaitForSecondsRealtime(1f);
        canTakeDamage = true;


    }
}
