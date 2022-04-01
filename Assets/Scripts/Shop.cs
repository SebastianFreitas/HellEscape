
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : GunGenerator
{
    [SerializeField] WeaponStatsCrafting weaponUI;
    [SerializeField] TMPro.TextMeshPro buyText;
    [SerializeField] TMPro.TextMeshPro priceText;
    [SerializeField] TMPro.TextMeshPro itemType;


    [SerializeField] Material green;
    [SerializeField] Material red;

    [SerializeField] MeshRenderer[] meshes;

    public GunOfAType gun;
    private int price;
    internal enum ItemType
    {
        weapon,
        heal
    }

    List<(GunOfAType, int)> weapons = new List<(GunOfAType, int)>();

    private int weaponNumber = 0;
    private int weaponLevel = 10;
    private void Awake()
    {
        weaponLevel = GetComponentInParent<RoomGenerator>().weaponLevel;
        weaponUI.ResetUI();
        InitiateItemList();
        UpdateShop();
    }

    private void InitiateItemList()
    {
        for(int i = 0; i < Random.Range(3,7); i++)
        {
            var x = CreateWeapon(weaponLevel, false);
            var currentPrice = x.mods.Count * (x.level - 9);
            weapons.Add((x, Random.Range(currentPrice + 1, currentPrice * 2)));
        }
    }


    internal void PressNext()
    {
        if (weaponNumber == weapons.Count - 1) weaponNumber = 0;
        else weaponNumber++;

        UpdateShop();
    }

    private void UpdateShop()
    {
        gun = weapons[weaponNumber].Item1;
        price = weapons[weaponNumber].Item2;
        itemType.text = gun.type.ToString();

        weaponUI.gun = gun;
        weaponUI.ResetUI();
        weaponUI.UpdateUI();

        priceText.text = "" + price;
    }

    internal void Buy()
    {
        var inventory = FindObjectOfType<Inventory>();
        if (inventory.playerInventory.gunParts >= price)
        {
            inventory.playerInventory.UpdateGunParts(-price);
            inventory.AddWeapon(gun, true);
            weapons.Remove((gun, price));
            if (weapons.Count == 0) Destroy(gameObject);
            else
            {
                weaponNumber = 0;
                PressNext();
            }
        }
        else StartCoroutine(ExceptionMessage("Not enough gunparts"));

    }

    public void TurnRed()
    {
        foreach (var x in meshes) x.material = red;
    }
    public void TurnGreen()
    {
        foreach (var x in meshes) x.material = green;
    }

    private IEnumerator ExceptionMessage(string message)
    {
        TurnRed();
        buyText.text = message;
        yield return new WaitForSeconds(.3f);
        TurnGreen();
        buyText.text = "Buy";
    }
}
