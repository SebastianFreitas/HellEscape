using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ModGenerator 
{
    public GunMods gun;
    void Start()
    {
        gun = createWeapon(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
