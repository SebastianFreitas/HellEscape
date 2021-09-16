using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDevice : GunGenerator
{
    private GameObject player;
    public WeaponStatsCrafting weaponStats;
    public TMPro.TextMeshPro guntext;

    public GunOfAType gun;

    public GameObject online;
    public GameObject offline;

    public TMPro.TextMeshPro stats;

    public Mod removedMod;

    public Material green;
    public Material yellow;
    public Material red;

    public int weaponParts;
    public TMPro.TextMeshPro weaponPartsText;

    public PlayerInventory playerInventory;

    public int zoneLevel = 1;


    public AddModUI addMod;
    public RemoveMod removeMod;
    // Start is called before the first frame update
    void Start()
    {
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

        playerInventory.gunParts -= (gun.level * (1 + gun.mods.Count) * 2) *timesUsed;
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
        return 0;
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
                playerInventory.gunParts -= price;
                gun.mods.Add(AddMod(zoneLevel, gun));
                FinishWeaponText(gun);
                guntext.text = gun.text;
                worked = 1;
                weaponStats.gun = gun;
                weaponStats.UpdateUI();
                UpdateStats();
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
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Bullet"))
        {
            weaponStats.ResetUI();

            

            gun = collision.transform.GetComponent<PlayerProjectile>().Gun;
            guntext.text = gun.text;
            zoneLevel = gun.level;
            offline.SetActive(false);
            weaponStats.gun = this.gun;
            weaponStats.UpdateUI();
            online.SetActive(true);

            UpdateCrafts();
            UpdateStats();

        }


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
