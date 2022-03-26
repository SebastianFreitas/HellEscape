using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoom : MonoBehaviour
{
    [SerializeField] GameObject weaponDrop;
    [SerializeField] GameObject boonDrop;
    [SerializeField] GameObject healDrop;

    [SerializeField] GameObject weaponUI;
    [SerializeField] GameObject boonUI;
    [SerializeField] GameObject healUI;

    internal void DropWeapon()
    {
        Instantiate(weaponDrop, transform.localPosition, transform.rotation, transform);
        DisableEvent();
    }

    internal void DropHeal()
    {
        Instantiate(healDrop, transform.localPosition, transform.rotation, transform);
        DisableEvent();
    }

    internal void GenerateBoon()
    {
        boonDrop.SetActive(true);
        DisableEvent();
    }

    internal void DisableEvent()
    {
        weaponUI.SetActive(false);
        boonUI.SetActive(false);
        healUI.SetActive(false);
    }
}
