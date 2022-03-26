using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRoom : MonoBehaviour
{
    [SerializeField] GameObject weaponDrop;

    [SerializeField] GameObject weaponUI;
    [SerializeField] GameObject boonUI;
    [SerializeField] GameObject healUI;

    private VoidBoon boon;

    private void Awake()
    {
        boon = new VoidBoon();
        boon.player = GameObject.FindGameObjectsWithTag("Dude")[0];
        boon.playerHP = boon.player.GetComponent<PlayerHpManager>();
        boon.playerInv = boon.player.GetComponent<PlayerInventory>();
        boon.playerMov = boon.player.GetComponent<PlayerBasicMovement>();
    }

    internal void DropWeapon()
    {
        Instantiate(weaponDrop, transform);
        DisableEvent();
    }

    internal void DropHeal()
    {
        Instantiate(healUI, transform);
        DisableEvent();
    }

    internal VoidBoon GenerateBoon()
    {
        return new VoidBoon();
    }

    private void DisableEvent()
    {
        weaponUI.SetActive(false);
        boonUI.SetActive(false);
    }
}
