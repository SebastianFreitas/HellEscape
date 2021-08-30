using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler
{
    public GunOfAType gun;
    public Text gunType;
    public TextUI uiText;

    public GameObject ButtonGameObject;
    public Inventory inventory;


    public void Update()
    {
        // Compare selected gameObject with referenced Button gameObject
        if (EventSystem.current.currentSelectedGameObject == ButtonGameObject)
        {
            if (Input.GetKeyDown("e")) GetComponent<Button>().onClick.Invoke();
            if (Input.GetKeyDown("f")) DismantleGun();

            if (!uiText.isActiveAndEnabled) uiText.gameObject.SetActive(true);
        }
    }

    private void DismantleGun()
    {
        inventory.UpdateFragments(gun.level);
        gun = null;
        UpdateInventoryText();
        ShowGun();
    }

    public void UpdateInventoryText()
    {
        if (gun != null) gunType.text = gun.type.ToString();
        else gunType.text = "-";

        uiText.gameObject.SetActive(true);
    }

    public void SwitchGun()
    {
        if (gun != null)
        {
            GameObject.FindWithTag("PlayerGun").transform.GetComponent<Gun>().SetGun(gun);
        }
        
    }

    public void ShowGun()
    {
        if (gun != null)
        {
            uiText.UpdateText(gun.text);
            StartCoroutine(FadeGunText());
        }
        else uiText.UpdateText("");

    }
    
    public void OnSelect(BaseEventData eventData)
    {
        ShowGun();
    }

    IEnumerator FadeGunText()
    {
        yield return new WaitForSeconds(10f);
        uiText.gameObject.SetActive(false);
    }
}
