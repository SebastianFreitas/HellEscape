using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GunGenerator 
{
    public GunOfAType gun;

    private bool hasBeenCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !hasBeenCollected)
        {
            hasBeenCollected = true;
            var x = GameObject.FindGameObjectWithTag("Inventory").transform;
            if (x.GetComponent<Inventory>().AddWeapon(gun)) Destroy(this.gameObject);
            else hasBeenCollected = false;
        }
    }
}
