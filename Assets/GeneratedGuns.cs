using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedGuns : MonoBehaviour
{
    public CraftingDevice craftingTable;
    public GameObject[] guns;
    public SelectGun[] generatedGuns;
    void Start()
    {
        int i = 0;
       guns = craftingTable.playerInventory.inventoryUI.slots;
       foreach(SelectGun x in generatedGuns)
       {
            x.gun = guns[i].GetComponent<Slot>().gun;
            x.gunTypeText.text = "select "+ guns[i].GetComponent<Slot>().gunTypeText.text;
            i++;
       } 


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
