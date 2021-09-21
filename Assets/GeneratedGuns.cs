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
       foreach(SelectGun x in generatedGuns)
       {
            x.gun = slots[i].GetComponent<Slot>().gun;
            x.gunTypeText.text = "Equip "+ slots[i].GetComponent<Slot>().gun.type.ToString();
            i++;
       } 


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
