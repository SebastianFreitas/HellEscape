using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    internal int gunParts = 10000;

    public Inventory inventoryUI;
    void Start()
    {
        inventoryUI = FindObjectOfType<Inventory>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void DisassembleGun(GunOfAType gun)
    {
        UpdateGunParts(gun.level * (gun.mods.Count + 1));
        inventoryUI.RemoveWeapon(gun); 
    }

    internal void UpdateGunParts(int x)
    {
        gunParts += x;
        inventoryUI.UpdateFragments(gunParts.ToString());
    }
}
