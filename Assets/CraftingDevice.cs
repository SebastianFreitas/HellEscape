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


    public int zoneLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponentInParent<Room>().player;
        
    }



    public bool AddNewMod()
    {
        var worked = false;
        if (gun.mods.Count == 6) return worked;
        else
        {
            gun.mods.Add(AddMod(zoneLevel, gun));
            FinishWeaponText(gun);
            guntext.text = gun.text;
            worked = true;
            weaponStats.gun = gun;
            weaponStats.UpdateUI();
            UpdateStats();
        }
        /*if (gradeWeight[0]+ gradeWeight[1]+ gradeWeight[2] <= 5)
        {
            Debug.Log("Yep");

        }*/
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


}
