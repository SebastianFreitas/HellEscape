using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    void Start()
    {
        for(int i = 0; i<6; i++)
        {
            if (slots[i].GetComponent<Slot>().gun != null) slots[i].SetActive(true);
            else break;
        }
    }

    // Update is called once per frame
    public void AddWeapon(GunOfAType gun)
    {
        for (int i = 0; i < 6; i++)
        {
            if (slots[i].GetComponent<Slot>().gun == null)
            {
                var x = slots[i].GetComponent<Slot>();
                x.gun = gun;
                x.UpdateGunText();
                break;
            }
        }
    }
}
