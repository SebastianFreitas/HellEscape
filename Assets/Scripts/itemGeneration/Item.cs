using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GunGenerator 
{
    public GunOfAType gun;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip collected;
    private float volume;

    private bool hasBeenCollected = false;


    void Start()
    {
        volume = PlayerPrefs.GetFloat("Volume");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !hasBeenCollected)
        {
            AudioSource.PlayClipAtPoint(collected, transform.position, volume+.25f);
            hasBeenCollected = true;
            var x = GameObject.FindGameObjectWithTag("Inventory").transform;
            if (x.GetComponent<Inventory>().AddWeapon(gun)) Destroy(this.gameObject);
            else hasBeenCollected = false;
        }
    }
}
