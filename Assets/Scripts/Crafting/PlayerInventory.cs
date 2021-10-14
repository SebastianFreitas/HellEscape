using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    internal int gunParts = 100000000;

    public Inventory inventoryUI;

    public Gun playerGun;


    internal void DisassembleGun(GunOfAType gun)
    {
        UpdateGunParts(gun.level * (gun.mods.Count + 1));
        inventoryUI.RemoveWeapon(gun, true); 
    }

    internal void DestroyGun(GunOfAType gun)
    {
        inventoryUI.RemoveWeapon(gun, false);
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

    internal int GetLayoutLength()
    {
        return inventoryUI.GetLayoutLength();
    }

    internal int GetGeneratedGunsLength()
    {
        return inventoryUI.GetSlotsLength();
    }

    internal void GenerateGun(GunOfAType gun)
    {
        inventoryUI.AddWeapon(gun, true);
    }
}
