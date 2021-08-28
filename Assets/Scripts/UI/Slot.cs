using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler
{
    public GunOfAType gun;
    public Text gunType;
    public GunText uiText;


    public void UpdateGunText()
    {
        gunType.text = gun.type.ToString();
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
        }
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Weapon is selected");
        ShowGun();
    }
}
