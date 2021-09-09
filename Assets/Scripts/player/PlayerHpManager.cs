using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpManager : MonoBehaviour
{
    public float health = 50;
    public HealthBar hp;
    private bool canTakeDamage = true;
    public PlayerSounds playerSounds;

    void Start()
    {
        //hp = GameObject.FindGameObjectsWithTag("HealthBar")[0].transform.GetComponent<HealthBar>();
        hp.SetMaxHealth((int)health);
    }
    public void TakeDamage(float amount)
    {
        if (canTakeDamage)
        {
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

    void Die()
    {
        transform.parent.GetComponent<GameMan>().RestartGame();
        Destroy(gameObject);

    }

    IEnumerator waiterImmunity()
    {
        canTakeDamage = false;
        
        yield return new WaitForSeconds(10f);
        canTakeDamage = true;
        
       
    }
}
