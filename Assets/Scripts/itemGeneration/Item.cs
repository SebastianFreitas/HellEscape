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

 
}
