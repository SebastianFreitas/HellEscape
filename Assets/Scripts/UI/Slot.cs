using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GunOfAType gun;
    public Text gunTypeText;
    public TextUI gunDescription;

    public GameObject ButtonGameObject;

    public Inventory inventory;

    public Gun playerGun;

    private String gunType;
    private bool equiped = false;


    public void Update()
    {
        // Compare selected gameObject with referenced Button gameObject
        if (EventSystem.current.currentSelectedGameObject == ButtonGameObject)
        {
            if (Input.GetKeyDown("e")) GetComponent<Button>().onClick.Invoke();
            if (Input.GetKeyDown("f")) DismantleGun();
            if (!gunDescription.isActiveAndEnabled) gunDescription.gameObject.SetActive(true);
        }
    }


    public void AddWeapon(GunOfAType gun)
    {
        this.gun = gun;
        gunType = gun.type.ToString();
        UpdateInventoryText();
    }

    private void DismantleGun()
    {
        if (gun != null)
        {
            inventory.UpdateFragments(gun.level);
            gun = null;
            gunTypeText.text = "-";
            gunType = null;
            ShowGun();
            if (equiped) playerGun.EquipBaseGun();
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

    private void UpdateSelectedGunUI()
    {
        if (gun != null) gunTypeText.text = "-> " + gunType;
        else gunTypeText.text = "-> ";
    }

    private void UpdateDeselectedGunUI()
    {
        if (gun != null) gunTypeText.text = gunType;
        else gunTypeText.text = "-";
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
        UpdateSelectedGunUI();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        UpdateDeselectedGunUI();
    }

    IEnumerator FadeGunText()
    {
        yield return new WaitForSeconds(10f);
        gunDescription.gameObject.SetActive(false);
    }
}
