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

    private PlayerInventory playerInventory;

    public int zoneLevel = 1;
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

    internal bool RemoveRandomMod()
    {
        if (gun.mods.Count == 0) return false;
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
        return true;
    }

    public int AddNewMod()
    {
        var worked = 0;
        if (gun.mods.Count == 6) return worked;
        else
        {
            if (gun.level * (1 + gun.mods.Count) <= playerInventory.gunParts)
            {
                gun.mods.Add(AddMod(zoneLevel, gun));
                FinishWeaponText(gun);
                guntext.text = gun.text;
                worked = 1;
                weaponStats.gun = gun;
                weaponStats.UpdateUI();
                UpdateStats();
                playerInventory.gunParts -= gun.level * (1 + gun.mods.Count);
                
            }
            else return -1;


        }
        return worked;
    }

    public void UpdateStats()
    {
        stats.text = CreateGunStats(gun);
    }

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Bullet"))
        {
            weaponStats.ResetUI();
            

            gun = collision.transform.GetComponent<PlayerProjectile>().Gun;
            guntext.text = gun.text;

            UpdateStats();

            offline.SetActive(false);
            weaponStats.gun = this.gun;
            weaponStats.UpdateUI();
            online.SetActive(true);
            
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
