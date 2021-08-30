using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public TextUI fragmentText;
    public int fragments = 0;

    void Start()
    {
        for(int i = 0; i<6; i++)
        {
            if (slots[i].GetComponent<Slot>().gun != null) slots[i].SetActive(true);
            else break;
        }

        UpdateFragments(fragments);
    }

    public void AddWeapon(GunOfAType gun)
    {
        for (int i = 0; i < 6; i++)
        {
            if (slots[i].GetComponent<Slot>().gun == null)
            {
                var x = slots[i].GetComponent<Slot>();
                x.gun = gun;
                x.UpdateInventoryText();
                break;
            }

            if (i == 6) UpdateFragments(gun.level);
        }
    }

    public void UpdateFragments(int level)
    {
        fragments+= level;
        fragmentText.UpdateText(fragments.ToString());
    }
}
