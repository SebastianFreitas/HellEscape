using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : GunGenerator 
{
    private GunOfAType gun;
    void Start()
    {
        gun = CreateWeapon(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var x = GameObject.FindGameObjectWithTag("Inventory").transform;
            x.GetComponent<Inventory>().AddWeapon(gun);
            Debug.Log("yep");
            Destroy(this.gameObject);
        }
    }
}
