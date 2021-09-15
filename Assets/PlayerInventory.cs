using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    internal int gunParts = 1000;

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
        gunParts += gun.level;
        inventoryUI.RemoveWeapon(gun); 
    }
}
