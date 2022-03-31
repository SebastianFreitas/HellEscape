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

    private RoomActivator activator;

    private void Start()
    {
        activator = GetComponentInParent<RoomActivator>();
    }

    internal void DropWeapon()
    {
        activator.SpawnWeapon();
        DisableEvent();
    }

    internal void DropHeal()
    {
        activator.SpawnHeal();
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
