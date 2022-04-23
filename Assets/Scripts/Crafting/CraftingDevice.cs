using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDevice : GunGenerator
{
    public GameObject player;
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
    public Material blue;

    public int weaponParts;

    public TMPro.TextMeshPro weaponPartsText;

    public PlayerInventory playerInventory;

    public int zoneLevel = 1;

    public bool portalOnline = false;

    internal int pos;

    public AddModUI addMod;
    public RemoveMod removeMod;
    public Destroy destroyGun;
    public GeneratedGuns slotGuns;

    public bool isHub;

    // Start is called before the first frame update
    void Start()
    {
        if (portalOnline || isHub) offline.SetActive(true);

        player = GameObject.FindGameObjectsWithTag("Dude")[0];
        playerInventory = player.GetComponent<PlayerInventory>();

        slotGuns.SelectSlot(0);
    }

    internal int GetRemoveModPrice()
    {
        if (gun != null) return (gun.level - 9) * (gun.mods.Count + 1);
        else return 1000;
        
    }

    internal int GetAddModPrice()
    {
        if (gun != null) return (gun.level - 9) * (gun.mods.Count + 1);
        else return 1000;
    }

    internal int GetDestroyReward()
    {
        if (gun != null) return (gun.level - 9) * (gun.mods.Count + 1) *2;
        else return 1000;
    }

    internal void DestroyGun()
    {
        if (playerInventory.GetGeneratedGunsLength()+playerInventory.GetLayoutLength() >1)
        {
            playerInventory.DestroyGun(this.gun, GetDestroyReward());

            slotGuns.GetGunLayout();
            slotGuns.GetGeneratedGuns();
            slotGuns.SelectRandomGun();
        }

    }

    internal bool GenerateGun()
    {
        if (playerInventory.GetGeneratedGunsLength() == 4) return false;

        playerInventory.inventoryUI.RemoveLayout(gun);


        playerInventory.GenerateGun(gun);

        UpdateInventory();

        slotGuns.SelectRandomGun();

        return true;
    }

    private void UpdateInventory()
    {
        slotGuns.GetGunLayout();
        slotGuns.GetGeneratedGuns();
    }

    public int DisassembleGun()
    {
        if (gun != null)
        {
            if (gun.isBase) return 2;
            if (playerInventory.GetGeneratedGunsLength() == 1) return 2;
            if (playerInventory.GetLayoutLength() < 8)
            {
                playerInventory.DisassembleGun(gun);

                slotGuns.GetGunLayout();
                slotGuns.GetGeneratedGuns();

                gun = null;



            } else return 1;
            return 0;
        }
        return 2;
    }

    internal int RemoveRandomMod(int timesUsed)
    {
        if (gun.mods.Count == 0) return 1;
        if ((GetRemoveModPrice())* timesUsed > playerInventory.gunParts) return 2;


        playerInventory.UpdateGunParts(-(GetRemoveModPrice() * timesUsed));
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
        playerInventory.UpdateEquipedGun(gun);
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
        var price = GetAddModPrice() * timesUsed;
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
                playerInventory.UpdateEquipedGun(gun);
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
        destroyGun.UpdatePriceText();

    }

    public void TurnOn()
    {
        offline.SetActive(false);
        online.SetActive(true);
    }

    internal void ReadWeapon(Collision collision, GunOfAType gunA)
    {
        if (gunA != null)
        {
            ReadPlayerLayouts(gunA);
        }
        else
        {
            ReadPlayerLayouts(collision.transform.GetComponent<PlayerProjectile>().Gun);

        }

    }

    private void ReadPlayerSlots()
    {
        foreach (var currenGun in slotGuns.generatedGuns)
        {
            if (currenGun.gun == gun)
            {
                slotGuns.SelectSlot(currenGun.position);
                pos = currenGun.position;
                craftingRecipes.SetActive(true);
                deconstruct.SetActive(true);
            }
        }
        SetGun(gun);
    }

    [SerializeField] GameObject destroyGun2;
    [SerializeField] GameObject addmod2;
    [SerializeField] GameObject removeMod2;


    private void ReadPlayerLayouts(GunOfAType gunA)
    {

        foreach (var currenGun in slotGuns.generatedGuns)
        {
            if (currenGun.gun == gunA)
            {
                slotGuns.SelectSlot(currenGun.position);
                pos = currenGun.position;
                //craftingRecipes.SetActive(true);
                destroyGun2.SetActive(true);
                addmod2.SetActive(true);
                removeMod2.SetActive(true);

                deconstruct.SetActive(true);
                generate.SetActive(false);
                break;
            }
        }

        foreach (var currenGun in slotGuns.gunLayouts)
        {
            if (currenGun.gun == gunA)
            {
                slotGuns.SelectSlot(currenGun.position);
                pos = currenGun.position;

                destroyGun2.SetActive(true);
                addmod2.SetActive(false);
                removeMod2.SetActive(false);

                deconstruct.SetActive(false);
                generate.SetActive(true);
                break;
            }
        }

        SetGun(gunA);
    }

    private void SetGun(GunOfAType gunA)
    {
        gun = gunA;
        zoneLevel = gun.level;

        weaponStats.gun = gunA;
        weaponStats.ResetUI();
        weaponStats.UpdateUI();

        guntext.text = gun.text;
        UpdateCrafts();
        UpdateStats();
        slotGuns.GetGunLayout();
        slotGuns.GetGeneratedGuns();

        playerInventory.inventoryUI.EquipWeapon(gun);
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
    internal void TurnBlue(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = blue;
    }



}
