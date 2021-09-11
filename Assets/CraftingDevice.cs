using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDevice : GunGenerator
{
    private GameObject player;
    public WeaponStatsCrafting weaponStats;
    public TMPro.TextMeshPro guntext;

    public GunOfAType gun;
    private HashSet<Mod> exteriorMods;
    private HashSet<Mod> interiorMods;
    private HashSet<Mod> specialMods;

    public GameObject online;
    public GameObject offline;


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
        if (gradeWeight[0]+ gradeWeight[1]+ gradeWeight[2] <= 5)
        {
            Debug.Log("Yep");
            gun.mods.Add(AddMod(zoneLevel, gun, gradeWeight));
            FinishWeaponText(gun);
            guntext.text = gun.text;
            worked = true;
            weaponStats.gun = gun;
            weaponStats.UpdateUI();
        }
        return worked;
    }


    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            gradeWeight[0] = 300;
            gradeWeight[1] = 300;
            gradeWeight[2] = 2;
            gun = collision.transform.GetComponent<PlayerProjectile>().Gun;
            guntext.text = gun.text;
            exteriorMods = gun.GetExteriorMods();
            interiorMods = gun.GetInteriorMods();
            specialMods = gun.GetSpecialMods();

            gradeWeight[0] = interiorMods.Count*300;
            gradeWeight[1] = exteriorMods.Count*300;
            gradeWeight[2] = specialMods.Count;
            offline.SetActive(false);
            weaponStats.gun = this.gun;
            online.SetActive(true);
            
        }


    }


}
