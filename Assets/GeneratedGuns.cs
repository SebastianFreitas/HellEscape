using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedGuns : MonoBehaviour
{

    public CraftingDevice craftingTable;
    public GameObject[] slots;
    public SelectGun[] generatedGuns;
    public SelectGun[] gunLayout;


    void Start()
    {
        GetGeneratedGuns();

        GetGunLayout();

    }

    public void GetGunLayout()
    {
        int i = 0;
        foreach (SelectGun inventorySlot in gunLayout)
        {
            if (inventorySlot != null)  inventorySlot.gunTypeText.text = "- ";


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
                selectX.gunTypeText.text =  slots[i].GetComponent<Slot>().gun.type.ToString();
            }
            else selectX.gunTypeText.text = "- ";

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
            }else
            if (x.isSelected)
            {
                x.isSelected = false;
                x.craftingTable.TurnBlue(x.meshes);
                
            }
        }
        foreach (SelectGun x in gunLayout)
        {
            if (x.position == position)
            {
                x.isSelected = true;
                craftingTable.TurnGreen(x.meshes);
            }else
            if (x.isSelected)
            {
                x.isSelected = false;
                x.craftingTable.TurnBlue(x.meshes);
            }
        }


    }
}
