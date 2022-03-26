using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpManager : MonoBehaviour
{
    public float health = 50;
    public float maxHealth = 50;
    public HealthBar hp;
    private bool canTakeDamage = true;
    public PlayerSounds playerSounds;

    public GameObject bloodScreen;

    void Start()
    {
        //hp = GameObject.FindGameObjectsWithTag("HealthBar")[0].transform.GetComponent<HealthBar>();
        hp.SetMaxHealth((int)maxHealth);
    }

    private void OnEnable()
    {
        StartCoroutine(waiterImmunity());
    }

    public void TakeDamage(float amount)
    {
        if (canTakeDamage)
        {
            StartCoroutine(BloodScreen());
            StartCoroutine(waiterImmunity());
            playerSounds.PlayTakeDamageSound();

            health -= amount;
            hp.SetHealth((int)health);
            if (health <= 0f)
            {
                Die();

            }
        }
    }

    internal void Heal(float amount)
    {
        var result = health + amount;
        if (result > maxHealth) hp.SetHealth((int)maxHealth);
        else hp.SetHealth((int)result);
    }

    internal void ChangeMaxHP(int amount)
    {
        hp.SetMaxHealth((int)maxHealth+amount);
    }

    void Die()
    {
        transform.parent.parent.GetComponent<GameMan>().ReturnToHub();
        hp.SetHealth((int)maxHealth);
        health = maxHealth;
    }

    IEnumerator BloodScreen()
    {
      bloodScreen.GetComponent<RawImage>().enabled = true;
      yield return new WaitForSeconds(.2f);
      bloodScreen.GetComponent<RawImage>().enabled = false;
    }

    IEnumerator waiterImmunity()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(.5f);
        canTakeDamage = true;


    }
}
