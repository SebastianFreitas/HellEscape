using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GunOfAType gun;
    public TMPro.TextMeshProUGUI gunTypeText;
    public TextUI gunDescription;

    public GameObject ButtonGameObject;

    public Inventory inventory;
    public GameObject player;
    public Gun playerGun;

    private String gunType;
    private bool equiped = false;

    public int slotNumber;


    public void Update()
    {
        if (equiped && Input.GetKeyDown("tab")) ShowGun();
    }


    public void AddWeapon(GunOfAType gun)
    {
        this.gun = gun;
        gunType = gun.type.ToString();
        UpdateInventoryText();
    }

    public void DismantleGun()
    {
        if (gun != null)
        {
            gun = null;
            gunTypeText.text = "-";
            gunType = null;
            ShowGun();
           // if (equiped) playerGun.EquipBaseGun();
        }
    }

    public void EquipGun()
    {
        if (gun != null)
        {
            playerGun.SetGun(gun);
            gunType = "< " + gun.type + " >";
            gunTypeText.text = gunType;
            equiped = true;
        }
    }

    public void UnEquipGun()
    {
        if (gun != null)
        {
            gunType = gun.type.ToString();
            gunTypeText.text = gunType;
            equiped = false;
        }
    }

    public void UpdateInventoryText()
    {
        if (gun != null) gunTypeText.text = gunType;
        else gunTypeText.text = "-";

        gunDescription.gameObject.SetActive(true);
        gunType = gun.type.ToString();
    }



    public void ShowGun()
    {

        if (gun != null)
        {
            gunDescription.UpdateText(gun.text);
            StartCoroutine(FadeGunText());
        }
        else gunDescription.UpdateText("");

    }
    
    public void OnSelect(BaseEventData eventData)
    {
        ShowGun();
        //EquipGun();
        //UpdateSelectedGunUI();
        inventory.EquipWeaponShortcut(slotNumber);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //UpdateDeselectedGunUI();
        //UnEquipGun();
    }

    IEnumerator FadeGunText()
    {
        gunDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        gunDescription.gameObject.SetActive(false);
    }
}
