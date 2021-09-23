using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedGuns : MonoBehaviour
{
    public CraftingDevice craftingTable;
    public GameObject[] slots;
    public SelectGun[] generatedGuns;
    void Start()
    {
        int i = 0;
        slots = craftingTable.playerInventory.inventoryUI.slots;
       foreach(SelectGun selectX in generatedGuns)
       {
            var currentGun = slots[i].GetComponent<Slot>().gun;
            if (currentGun != null)
            {
                selectX.gun = currentGun;
                selectX.gunTypeText.text = "Equip " + slots[i].GetComponent<Slot>().gun.type.ToString();
            }
            else selectX.gunTypeText.text = "EMpty Slot ";
            
            i++;
       } 


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
