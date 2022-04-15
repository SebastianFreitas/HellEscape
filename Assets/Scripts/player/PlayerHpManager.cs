using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpManager : MonoBehaviour
{
    private float health = 50;
    private float maxHealth = 50;
    public HealthBar hp;
    internal bool canTakeDamage = true;
    public PlayerSounds playerSounds;

    public GameObject bloodScreen;

    private GameMan manager;
    private ModDataRoom.GeneratedMission mission;
    private bool isDashing;
    private int invincibilityDurationSeconds;

    void Awake()
    {
        //hp = GameObject.FindGameObjectsWithTag("HealthBar")[0].transform.GetComponent<HealthBar>();
        manager = transform.root.GetComponent<GameMan>();
        hp.SetMaxHealth((int)maxHealth);


        isDashing = GetComponent<PlayerBasicMovement>().isSideDashing;
    }

    private void OnEnable()
    {
        StartCoroutine(waiterImmunity());
    }

    public void TakeDamage(float amount)
   {
        if (!canTakeDamage || isDashing) return;
     

        canTakeDamage = false;
        StartCoroutine(BloodScreen());
        StartCoroutine(waiterImmunity());
        playerSounds.PlayTakeDamageSound();

        health -= amount;
        hp.SetHealth((int)health);
        if (health <= 0f)
        {
            Die();

            }

           // if (health <= maxHealth * .3f) hp.ChangeToRed();
        
    }

    void BecomeTemporarilyInvincible()
    {
        for (float i = 0; i < invincibilityDurationSeconds; i += Time.deltaTime)
        {
            // do stuff here
        }
    }

    internal void Heal(float amount)
    {
        if (manager.mission != null)
        {
            if (manager.mission.halfHealing) amount /= 2;
        }


        var result = health + amount;
        if (result > maxHealth) hp.SetHealth((int)maxHealth);
        else hp.SetHealth((int)result);

       
    }

    internal void HealForMax()
    {
        Heal((int)maxHealth);
    }

    internal void ChangeMaxHP(int amount)
    {
        hp.SetMaxHealth((int)maxHealth+amount);
        Heal(amount);
        maxHealth += amount;
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
