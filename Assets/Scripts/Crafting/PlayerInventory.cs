using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    internal int gunParts = 0;

    public Inventory inventoryUI;

    public Gun playerGun;

    internal List<VoidBoon> listBoons = new List<VoidBoon>();
    internal int weaponLevel = 10;

    //player stats
    internal int additionalFireDamage = 0;
    internal int additionalColdDamage;
    internal int additionalPoisonDamage;
    internal int additionalPhysicalDamage;
    internal float additionalincreasedCriticalDamage;
    internal int additionalBounces;
    internal int increasedBulletSpeed;


    internal int additionalGrenadeDamage;
    internal int addtitionalGrenadeSpeed;
    internal int increasedMovementSpeed;
    internal bool explosiveRicochet;
    internal float increasedFireArea;
    internal bool RicochetStack;
    internal int fireMultiplier = 1;
    internal bool delayedFire;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("GunParts"))
        {
            gunParts = PlayerPrefs.GetInt("GunParts");
            inventoryUI.SetGunParts(gunParts.ToString());
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("GunParts", gunParts);
    }

    internal void LooseBoons()
    {
        foreach(var current in listBoons.ToArray())
        {
            current.AcceptOrRemoveBoon(false);
        }
    }

    internal void DisassembleGun(GunOfAType gun)
    {
        inventoryUI.RemoveWeapon(gun, true); 
    }

    internal void DestroyGun(GunOfAType gun, int reward)
    {
        UpdateGunParts(reward);
        inventoryUI.RemoveWeapon(gun, false);
    }

    internal void UpdateGunParts(int x)
    {
        gunParts += x;
        inventoryUI.SetGunParts(gunParts.ToString());
    }

    internal void UpdateEquipedGun(GunOfAType gun)
    {
        playerGun.SetGun(gun);
       
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
