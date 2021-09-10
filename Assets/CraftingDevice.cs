using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDevice : GunGenerator
{
    private GameObject player;
    public TMPro.TextMeshPro guntext;

    private GunOfAType gun;
    private HashSet<Mod> exteriorMods;
    private HashSet<Mod> interiorMods;
    private HashSet<Mod> specialMods;

    public int zoneLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.GetComponentInParent<Room>().player;
    }



    public void AddNewMod()
    {
        if (gradeWeight[0]+gradeWeight[1]+gradeWeight[2] != 0)
        {
            gun.mods.Add(AddMod(zoneLevel, gun));
            FinishWeaponText(gun);
            guntext.text = gun.text;
        }

    }


    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gun = collision.transform.GetComponent<PlayerProjectile>().Gun;
            guntext.text = gun.text;
            //exteriorMods = gun.GetExteriorMods();
            //interiorMods = gun.GetInteriorMods();
            //specialMods = gun.GetSpecialMods();

            Debug.Log(gradeWeight[0]);

            //base.gradeWeight[0] -= interiorMods.Count*100;
            //base.gradeWeight[1] -= exteriorMods.Count*100;
            //base.gradeWeight[2] -= specialMods.Count;

        }


    }


}
