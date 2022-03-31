using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedGuns : MonoBehaviour
{

    public CraftingDevice craftingTable;

    public GameObject[] slots;
    public GunOfAType[] layoutSlots;

    public SelectGun[] generatedGuns;
    public SelectGun[] gunLayouts;


    void Start()
    {
        GetGeneratedGuns();

        GetGunLayout();

    }

    private void OnEnable()
    {
        GetGeneratedGuns();

        GetGunLayout();

        //foreach (var currenGun in generatedGuns)
        //{
        //    if (currenGun.gun != null)
        //    {
        //        if (currenGun.gun.Equals(craftingTable.gun))
        //        {
        //            SelectSlot(currenGun.position);
        //        }
        //    }

        //}

        foreach (var currenGun in generatedGuns)
        {
            if (currenGun.gun != null)
            {

                SelectSlot(currenGun.position);
                craftingTable.ReadWeapon(null, currenGun.gun);

            }

        }

    }

    public void GetGunLayout()
    {
        int i = 0;
        layoutSlots = craftingTable.playerInventory.inventoryUI.layouts;
        foreach (SelectGun selectX in gunLayouts)
        {
            var currentGun = layoutSlots[i];
            if (currentGun != null)
            {
                selectX.gun = currentGun;
                selectX.gunTypeText.text = currentGun.type.ToString();
            }
            else
            {
                selectX.gunTypeText.text = "- ";
                selectX.gun = null;
            }

            i++;
        }
    }

    public void GetGeneratedGuns()
    {
        int i = 0;
        slots = craftingTable.playerInventory.inventoryUI.slots;
           
        foreach (SelectGun selectX in generatedGuns)
        {
            var currentGun = slots[i].GetComponent<Slot>().gun;
            if (currentGun != null)
            {
                selectX.gun = currentGun;
                selectX.gunTypeText.text = slots[i].GetComponent<Slot>().gun.type.ToString();
            }
            else
            {
                selectX.gunTypeText.text = "- ";
                selectX.gun = null;
            }

            i++;
        }
    }

    public void SelectSlot(int position)
    {
        foreach(SelectGun x in generatedGuns)
        {
            if (x.position == position)
            {
                x.isSelected = true;
                craftingTable.TurnGreen(x.meshes);
            }
            else if (x.isSelected)
            {
                x.isSelected = false;
                x.craftingTable.TurnBlue(x.meshes);
                
            }
        }

        foreach (SelectGun x in gunLayouts)
        {
            if (x.position == position)
            {
                x.isSelected = true;
                craftingTable.TurnGreen(x.meshes);
            }
            else if (x.isSelected)
            {
                x.isSelected = false;
                x.craftingTable.TurnBlue(x.meshes);
            }
        }


    }

    public bool SelectRandomGun()
    {
        int i = 1;
        foreach (SelectGun x in generatedGuns)
        {
            if (x.gun != null)
            {
                craftingTable.ReadWeapon(null, x.gun);
                return true;
            }
            i++;
        }
        return false;
    }

}
