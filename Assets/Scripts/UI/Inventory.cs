using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : GunGenerator
{
    public GameObject[] slots;
    public GunOfAType[] layouts = new GunOfAType[8];


    public TextUI fragmentText;
    public TextUI layoutsText;
    public int fragments = 11110;

    private int previousEquipedGun = -1;

    public GameMan manager;
    internal PlayerInventory playerInventory;


    private void OnDisable()
    {
        int index = 0;
        foreach(GameObject current in slots)
        {
            var x = current.GetComponent<Slot>().gun;
            if(x != null)
            {
                PlayerPrefs.SetInt("WeaponExists" + index,1);
                PlayerPrefs.SetString("StoredEnumStringType"+index, x.type.ToString());
                PlayerPrefs.SetInt("maxLevel"+index, x.level);


                PlayerPrefs.SetInt("GradeWeight0" + index, x.gradeWeight[0]);
                PlayerPrefs.SetInt("GradeWeight1" + index, x.gradeWeight[1]);
                PlayerPrefs.SetInt("GradeWeight2" + index, x.gradeWeight[2]);
                int indexMod = 0;
                foreach(var mod in x.mods)
                {

                    PlayerPrefs.SetString("StoredEnumStringGrade" + index + indexMod, mod.grade.ToString());
                    PlayerPrefs.SetInt("id" + index +indexMod, mod.id);
                    PlayerPrefs.SetInt("value" + index + indexMod, mod.upperBound);
                    PlayerPrefs.SetInt("tier" + index + indexMod, mod.tier);

                    indexMod++;
                }
                for(; indexMod < 6; indexMod++)
                {
                    PlayerPrefs.SetInt("id" + index + indexMod,-1);
                }

            } else PlayerPrefs.SetInt("WeaponExists" + index, -1);

            index++;
        }

        foreach(var current in layouts)
        {
            var x = current;
            if (current != null)
            {
                PlayerPrefs.SetInt("WeaponExists" + index, 1);
                PlayerPrefs.SetString("StoredEnumStringType" + index, x.type.ToString());
                PlayerPrefs.SetInt("maxLevel" + index, x.level);

                PlayerPrefs.SetInt("GradeWeight0" + index, x.gradeWeight[0]);
                PlayerPrefs.SetInt("GradeWeight1" + index, x.gradeWeight[1]);
                PlayerPrefs.SetInt("GradeWeight2" + index, x.gradeWeight[2]);
                int indexMod = 0;
                foreach (var mod in x.mods)
                {

                    PlayerPrefs.SetString("StoredEnumStringGrade" + index + indexMod, mod.grade.ToString());
                    PlayerPrefs.SetInt("id" + index + indexMod, mod.id);
                    PlayerPrefs.SetInt("value" + index + indexMod, mod.upperBound);
                    PlayerPrefs.SetInt("tier" + index + indexMod, mod.tier);

                    indexMod++;
                }
                for (; indexMod < 6; indexMod++)
                {
                    PlayerPrefs.SetInt("id" + index + indexMod, -1);
                }

            }
            else PlayerPrefs.SetInt("WeaponExists" + index, -1);

            index++;
        }
    }

    private void OnEnable()
    {


        for (int i = 0; i < 12; i++)
        {
            if (PlayerPrefs.HasKey("WeaponExists" + i))
            {
                if (PlayerPrefs.GetInt("WeaponExists" + i) > 0)
                {
                    Mod[] mods = new Mod[6];
                    for (int modCounter = 0; modCounter < 6; modCounter++)
                    {
                        if (PlayerPrefs.GetInt("id" + i + modCounter) > 0)
                        {
                            int id = PlayerPrefs.GetInt("id" + i + modCounter);
                            int value = PlayerPrefs.GetInt("value" + i + modCounter);
                            int tier = PlayerPrefs.GetInt("tier" + i + modCounter);
                            Grade grade = (Grade)Enum.Parse(typeof(Grade), PlayerPrefs.GetString("StoredEnumStringGrade" + i + modCounter));

                            mods[modCounter] = CreateModSeed(id, value, grade, tier);
                        }
                        else mods[modCounter] = CreateModSeed(-1, -1, Grade.extra, 0);

                    }
                    int maxLevel = PlayerPrefs.GetInt("maxLevel" + i);
                    GunType type = (GunType)Enum.Parse(typeof(GunType), PlayerPrefs.GetString("StoredEnumStringType" + i));

                    var zero = PlayerPrefs.GetInt("GradeWeight0" + i);//, x.gradeWeight[0]);
                    var one = PlayerPrefs.GetInt("GradeWeight1" + i);//, x.gradeWeight[1]);
                    var two =PlayerPrefs.GetInt("GradeWeight2" + i);//, x.gradeWeight[2]);

                    AddWeapon(CreateWeaponSeed(maxLevel, type, mods, zero, one ,two), true);


                }
                else
                {
                    if (i < 4)  slots[i].GetComponent<Slot>().gun = null;

                }
            }
            if (i < 4) slots[i].GetComponent<Slot>().UpdateInventoryText();
        }



        playerInventory = manager.player.GetComponent<PlayerInventory>();
        StartCoroutine(GiveGunToSlots());
        SetGunParts(playerInventory.gunParts.ToString());
        SetGunLayouts();

        EquipWeaponShortcut(1);
        EquipWeaponShortcut(2);
        EquipWeaponShortcut(3);
        EquipWeaponShortcut(1);

    }
    void Start()
    {

        //PlayerPrefs.SetInt("WeaponExists" + 0, -1);
        playerInventory = manager.player.GetComponent<PlayerInventory>();
        StartCoroutine(GiveGunToSlots());
        SetGunParts(playerInventory.gunParts.ToString());
        SetGunLayouts();

    }

    private void SetGunLayouts()
    {
        var x = "";
        var i = GetLayoutLength();
        while (i != 0)
        {
            x += "|";
            i--;
        }
        layoutsText.UpdateText(x);
    }

    internal void RemoveWeapon(GunOfAType gun, bool isAdded)
    {
        foreach (GameObject slot in slots)
        {
            var x = slot.GetComponent<Slot>();
            if (x.gun == gun) x.DismantleGun();

        }

        RemoveLayout(gun);

        if (isAdded) AddGunLayout(gun);
    }

    public bool RemoveLayout(GunOfAType gun)
    {
        for (int a = 0; a < 8; a++)
        {
            if (layouts[a] == gun)
            {
                layouts[a] = null;
                SetGunLayouts();
                return true;
            }
        }
        return false;
    }

    internal int GetLayoutLength()
    {
        int x = 0;
        foreach (var y in layouts) if (y != null) x++;

        return x;
    }

    internal int GetSlotsLength()
    {
        int x = 0;
        foreach (var y in slots) if (y.GetComponent<Slot>().gun != null) x++;

        return x;
    }

    IEnumerator GiveGunToSlots()
    {
        yield return new WaitForSeconds(1f);
       
        for (int i = 0; i < 4; i++)
        {
            var currentSlot = slots[i].GetComponent<Slot>();
            if (currentSlot.gun != null) slots[i].SetActive(true);

            
        }
    }

    private void Update()
    {
        if      (Input.GetKeyDown("1")) EquipWeaponShortcut(1);
        else if (Input.GetKeyDown("2")) EquipWeaponShortcut(2);
        else if (Input.GetKeyDown("3")) EquipWeaponShortcut(3);
        else if (Input.GetKeyDown("4")) EquipWeaponShortcut(4);
    }

    public bool AddWeapon(GunOfAType gun, bool canBeLayout)
    {
        var i = 0;
        for (; i < 4; i++)
        {
            if (slots[i].GetComponent<Slot>().gun == null)
            {
                slots[i].GetComponent<Slot>().AddWeapon(gun);
                return true;
            }
        }

        if (canBeLayout) return AddGunLayout(gun);
        else return true;
    }

    internal void EquipWeapon(GunOfAType gun)
    {
        int i = 1;
        foreach (var item in slots)
        {
            if (gun == item.GetComponent<Slot>().gun)
            {
                EquipWeaponShortcut(i);
            }
            i++;
        }
    }

    public bool AddGunLayout(GunOfAType gun)
    {
        for (int a = 0; a < 8; a++)
        {
            if (layouts[a] == null)
            {
                layouts[a] = gun;
                SetGunLayouts();
                return true;
            }
        }
        return false;
    }

    public void SetGunParts(string z)
    {
        fragmentText.UpdateText(z);
    }

    public void EquipWeaponShortcut(int number)
    {
        if (number-1 != previousEquipedGun && number <5)
        {
            slots[number-1].GetComponent<Slot>().EquipGun();
            if (previousEquipedGun != -1)
            {
                var prevGun = slots[previousEquipedGun].GetComponent<Slot>();
                prevGun.UnEquipGun();
            
            }
            previousEquipedGun = number - 1;
        }
    }
}
