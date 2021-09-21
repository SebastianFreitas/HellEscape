using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public TextUI fragmentText;
    public int fragments = 0;

    private int previousEquipedGun = -1;

    public GameMan manager;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = manager.playerPrefab.GetComponent<PlayerInventory>();
        StartCoroutine(GiveGunToSlots());
        UpdateFragments(playerInventory.gunParts.ToString());
    }

    internal void RemoveWeapon(GunOfAType gun)
    {
        foreach(GameObject slot in slots)
        {
            var x = slot.GetComponent<Slot>();
            if (x.gun == gun) x.DismantleGun();
           
        }
    }

    IEnumerator GiveGunToSlots()
    {
        yield return new WaitForSeconds(1f);
        var gun = GameObject.FindWithTag("PlayerGun").transform.GetComponent<Gun>();

        for (int i = 0; i < 4; i++)
        {
            var currentSlot = slots[i].GetComponent<Slot>();
            if (currentSlot.gun != null) slots[i].SetActive(true);
            currentSlot.playerGun = gun;
            
        }
    }

    private void Update()
    {
        if      (Input.GetKeyDown("1")) EquipWeaponShortcut(1);
        else if (Input.GetKeyDown("2")) EquipWeaponShortcut(2);
        else if (Input.GetKeyDown("3")) EquipWeaponShortcut(3);
        else if (Input.GetKeyDown("4")) EquipWeaponShortcut(4);
    }

    public void AddWeapon(GunOfAType gun)
    {
        var i = 0;
        for (; i < 4; i++)
        {
            if (slots[i].GetComponent<Slot>().gun == null)
            {
                slots[i].GetComponent<Slot>().AddWeapon(gun);
                break;
            }
        }
    }

    public void UpdateFragments(string z)
    {
        fragmentText.UpdateText(z);
    }

    public void EquipWeaponShortcut(int number)
    {
        if (number-1 != previousEquipedGun)
        {
            slots[number-1].GetComponent<Slot>().EquipGun();
            if (previousEquipedGun != -1)
            {
                var prevGun = slots[previousEquipedGun].GetComponent<Slot>();
                prevGun.UnEquipGun();
                //prevGun.UpdateInventoryText();
            
            }
            previousEquipedGun = number - 1;
        }
    }
}
