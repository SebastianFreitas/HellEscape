using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    internal int gunParts = 100000000;

    public Inventory inventoryUI;

    public Gun playerGun;
    void Start()
    {
        inventoryUI = FindObjectOfType<Inventory>(); 
    }


    internal void DisassembleGun(GunOfAType gun)
    {
        UpdateGunParts(gun.level * (gun.mods.Count + 1));
        inventoryUI.RemoveWeapon(gun); 
    }

    internal void DestroyGun(GunOfAType gun)
    {
        UpdateGunParts(gun.level * (gun.mods.Count + 1) * 2);
        inventoryUI.RemoveWeapon(gun);
    }

    internal void UpdateGunParts(int x)
    {
        gunParts += x;
        inventoryUI.SetGunParts(gunParts.ToString());
    }

    internal void UpdateEquipedGun()
    {
        playerGun.SetBulletStats();
    }
}
