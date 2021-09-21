using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDevice : GunGenerator
{
    internal GameObject player;
    public WeaponStatsCrafting weaponStats;
    public TMPro.TextMeshPro guntext;

    public GunOfAType gun;

    public GameObject online;
    public GameObject offline;

    public GameObject iventoryDevice;
    public GameObject craftingRecipes;

    public GameObject move;
    public GameObject craft;
    public GameObject generate;
    public GameObject deconstruct;
    public GameObject destroy;

    public TMPro.TextMeshPro stats;

    public Mod removedMod;

    public Material green;
    public Material yellow;
    public Material red;

    public int weaponParts;
    public TMPro.TextMeshPro weaponPartsText;

    public PlayerInventory playerInventory;

    public int zoneLevel = 1;

    public bool portalOnline = false;


    public AddModUI addMod;
    public RemoveMod removeMod;
    public DisassembleGun deconstructGun;
    // Start is called before the first frame update
    void Start()
    {
        if (portalOnline) offline.SetActive(true);
        player = transform.GetComponentInParent<Room>().player;
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    internal void DisassembleGun()
    {
        playerInventory.DisassembleGun(gun);
        offline.SetActive(true);
        online.SetActive(false);
    }

    internal int RemoveRandomMod(int timesUsed)
    {
        if (gun.mods.Count == 0) return 1;
        if ((gun.level * (1 + gun.mods.Count) * 2 )* timesUsed > playerInventory.gunParts) return 2;


        playerInventory.UpdateGunParts(-(gun.level * (1 + gun.mods.Count) * 2) * timesUsed);
        int i = Random.Range(0, gun.mods.Count);
        int a = 0;
        foreach(Mod x in gun.mods)
        {
            if (i == a)
            {
                removedMod = x;
                RemoveMod(gun, x);

                break;
            }
            a++;
        }
        weaponStats.ResetUI();
        FinishWeaponText(gun);
        guntext.text = gun.text;
        weaponStats.UpdateUI();
        UpdateStats();
        playerInventory.UpdateEquipedGun();
        return 0;
    }

    public void GoToInventory()
    {
        craftingRecipes.SetActive(false);
        iventoryDevice.SetActive(true);

    }

    public void GoToCrafting()
    {
        iventoryDevice.SetActive(false);
        craftingRecipes.SetActive(true);
    }

    public int AddNewMod(int timesUsed)
    {
        var worked = 0;
        var price = (gun.level * (1 + gun.mods.Count)) * timesUsed;
        if (gun.mods.Count == 6) return worked;
        else
        {
            if (price <= playerInventory.gunParts)
            {
                playerInventory.UpdateGunParts(-price);
                gun.mods.Add(AddMod(zoneLevel, gun));
                FinishWeaponText(gun);
                guntext.text = gun.text;
                worked = 1;
                weaponStats.gun = gun;
                UpdateStats();
                playerInventory.UpdateEquipedGun();
                weaponStats.UpdateUI();

            }
            else return -1;


        }
        return worked;
    }

    public void UpdateStats()
    {
        stats.text = CreateGunStats(gun);
    }

    public void UpdateCrafts()
    {
        addMod.UpdatePriceText();
        removeMod.UpdatePricetext();
        deconstructGun.UpdatePriceText();

    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Bullet") && portalOnline)
        {
            ReadWeapon(collision);

            offline.SetActive(false);
            online.SetActive(true);
        }


    }

    internal void ReadWeapon(Collision collision)
    {
        
        gun = collision.transform.GetComponent<PlayerProjectile>().Gun;
        
        zoneLevel = gun.level;
       
        weaponStats.gun = this.gun; 
        weaponStats.ResetUI();
        weaponStats.UpdateUI();
        guntext.text = gun.text;
        UpdateCrafts();
        UpdateStats();
    }

    public IEnumerator HighLight(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = yellow;
        yield return new WaitForSeconds(.3f);
        foreach (var x in materials) x.material = green;
    }

    public IEnumerator HighLightNot(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = red;
        yield return new WaitForSeconds(.3f);
        foreach (var x in materials) x.material = green;
    }

    public void TurnRed(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = red;
    }
    public void TurnGreen(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = green;
    }
    public void TurnYellow(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = yellow;
    }



}
