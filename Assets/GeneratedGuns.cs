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

    private void GetGunLayout()
    {
        int i = 0;
        foreach (SelectGun inventorySlot in gunLayout)
        {
            if (inventorySlot != null)  inventorySlot.gunTypeText.text = "- ";


            i++;
        }
    }

    private void GetGeneratedGuns()
    {
        int i = 0;
        slots = craftingTable.playerInventory.inventoryUI.slots;
        foreach (SelectGun selectX in generatedGuns)
        {
            var currentGun = slots[i].GetComponent<Slot>().gun;
            if (currentGun != null)
            {
                selectX.gun = currentGun;
                selectX.gunTypeText.text = "Equip " + slots[i].GetComponent<Slot>().gun.type.ToString();
            }
            else selectX.gunTypeText.text = "- ";

            i++;
        }
    }

    public void SelectSlot(int position)
    {
        foreach(SelectGun x in generatedGuns)
        {
            if (x.isSelected && x.position != position)
            {
                x.isSelected = false;
                x.craftingTable.TurnBlue(x.meshes);
            }
        }
        foreach (SelectGun x in gunLayout)
        {
            if (x.isSelected && x.position != position)
            {
                x.isSelected = false;
                x.craftingTable.TurnBlue(x.meshes);
            }
        }
    }
}
