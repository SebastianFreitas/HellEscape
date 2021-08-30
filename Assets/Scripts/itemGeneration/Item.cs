using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GunGenerator 
{
    private GunOfAType gun;

    private bool hasBeenCollected = false;
    void Start()
    {
        gun = CreateWeapon(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !hasBeenCollected)
        {
            hasBeenCollected = true;
            var x = GameObject.FindGameObjectWithTag("Inventory").transform;
            x.GetComponent<Inventory>().AddWeapon(gun);
            Destroy(this.gameObject);
        }
    }
}
