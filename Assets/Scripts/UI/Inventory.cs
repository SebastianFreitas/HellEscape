using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public GunOfAType[] layouts = new GunOfAType[8];


    public TextUI fragmentText;
    public int fragments = 0;

    private int previousEquipedGun = -1;

    public GameMan manager;
    private PlayerInventory playerInventory;

    

    void Start()
    {
        playerInventory = manager.player.GetComponent<PlayerInventory>();
        StartCoroutine(GiveGunToSlots());
        SetGunParts(playerInventory.gunParts.ToString());
    }
    private void OnEnable()
    {
        playerInventory = manager.player.GetComponent<PlayerInventory>();
        StartCoroutine(GiveGunToSlots());
        SetGunParts(playerInventory.gunParts.ToString());
    }

    internal void RemoveWeapon(GunOfAType gun, bool isAdded)
    {
        foreach(GameObject slot in slots)
        {
            var x = slot.GetComponent<Slot>();
            if (x.gun == gun) x.DismantleGun();
           
        }

        int i = 0;
        while (layouts[i] != gun && layouts[i] != null) i++;

        if (i < 8) layouts[i] = null;


        if (isAdded) AddGunLayout(gun);
    }

    internal int GetLayoutLength()
    {
        int x = 0;
        foreach (var y in layouts) if (y != null) x++;

        return x;
    }

    internal int GetSlotsLength()
    {
        int x = 0;
        foreach (var y in slots) if (y.GetComponent<Slot>().gun != null) x++;

        return x;
    }

    IEnumerator GiveGunToSlots()
    {
        yield return new WaitForSeconds(1f);
       
        for (int i = 0; i < 4; i++)
        {
            var currentSlot = slots[i].GetComponent<Slot>();
            if (currentSlot.gun != null) slots[i].SetActive(true);

            
        }
    }

    private void Update()
    {
        if      (Input.GetKeyDown("1")) EquipWeaponShortcut(1);
        else if (Input.GetKeyDown("2")) EquipWeaponShortcut(2);
        else if (Input.GetKeyDown("3")) EquipWeaponShortcut(3);
        else if (Input.GetKeyDown("4")) EquipWeaponShortcut(4);
    }

    public bool AddWeapon(GunOfAType gun, bool canBeLayout)
    {
        var i = 0;
        for (; i < 4; i++)
        {
            if (slots[i].GetComponent<Slot>().gun == null)
            {
                slots[i].GetComponent<Slot>().AddWeapon(gun);
                return true;
            }
        }

        if (canBeLayout) return AddGunLayout(gun);
        else return true;
    }

    public bool AddGunLayout(GunOfAType gun)
    {
        for (int a = 0; a < 8; a++)
        {
            if (layouts[a] == null)
            {
                layouts[a] = gun;
                return true;
            }
        }
        return false;
    }

    public void SetGunParts(string z)
    {
        fragmentText.UpdateText(z);
    }

    public void EquipWeaponShortcut(int number)
    {
        if (number-1 != previousEquipedGun && number <5)
        {
            slots[number-1].GetComponent<Slot>().EquipGun();
            if (previousEquipedGun != -1)
            {
                var prevGun = slots[previousEquipedGun].GetComponent<Slot>();
                prevGun.UnEquipGun();
            
            }
            previousEquipedGun = number - 1;
        }
    }
}
