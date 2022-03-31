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
        int level = GameObject.FindGameObjectsWithTag("Dude")[0].GetComponent<PlayerInventory>().weaponLevel;
        gun = CreateWeapon(level, false);//transform.GetComponentInParent<Room>().areaLevel
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !hasBeenCollected)
        {
            CollectItem();
        }
    }

    public void CollectItem()
    {
        var x = GameObject.FindGameObjectWithTag("Inventory").transform;
        if (x.GetComponent<Inventory>().AddWeapon(gun, true))
        {
            AudioSource.PlayClipAtPoint(collected, transform.position, volume + .25f);
            hasBeenCollected = true;
            Destroy(this.gameObject);
        }
        else hasBeenCollected = false;
    }
}
