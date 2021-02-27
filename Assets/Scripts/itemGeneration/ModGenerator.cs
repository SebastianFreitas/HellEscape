

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModGenerator : ModBase
{
    public GunMods CreateWeapon(int maxLevel)
    {
        GunMods ret = new GunMods
        {
            mods = new HashSet<BaseMod>(),
            fireRate = base.baseFireRate,
            weaponDamage = baseDamage,
            maxRicochets = BaseRicochets,
            level = maxLevel
        };

        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        int nextGrade;
        

        for (int i = 0; i < totalMods; i++)
        {

            nextGrade = GenerateGrade(ret);
            switch (nextGrade)
            {
                case 0:
                    var intMod= GenerateInteriorMod(ret);
                    intMod = UpdateMod(intMod, maxLevel);
                    GenerateInteriorStats(ret, intMod);
                    intMod.text = CreateText(intMod);
                    ret.mods.Add(intMod);
                    break;
                case 1:
                    var extMod = GenerateInteriorMod(ret);
                    extMod = UpdateMod(extMod, maxLevel);
                    GenerateExteriorStats(ret, extMod);
                    extMod.text = CreateText(extMod);
                    ret.mods.Add(extMod);
                    break;
                case 2:
                    var newMod = GenerateInteriorMod(ret);
                    newMod = UpdateMod(newMod, maxLevel);
                    GenerateSpecialStats(ret, newMod);
                    newMod.text = CreateText(newMod);
                    ret.mods.Add(newMod);
                    break;
            }
           
        }
        return ret;
    }

    private BaseMod GenerateInteriorMod(GunMods gun)
    {
        BaseMod aux;
        while (true)
        {
            aux = modsInterior[GetRandomWeightedIndex(InteriorWeight)];
            if (!ContainsMod(gun, aux)) break;
        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier);
    }

    private void FinishWeapon(GunMods gun)
    {
        //gun.mods.Select(x => ConvergeModStats(gun, x));


    }

    private void ConvergeModStats(GunMods gun, BaseMod mod)
    {
            UpdateMod(mod, gun.level);
            if (mod.grade == grade.interior) GenerateInteriorStats(gun, mod);
            else if (mod.grade == grade.exterior) GenerateInteriorStats(gun, mod);
            else GenerateInteriorStats(gun, mod);
            CreateText(mod);

    }


    //Creates tier and text
    private BaseMod UpdateMod(BaseMod mod, int level)
    {
        int diference = mod.upperBound - mod.lowerBound;
        mod.tier = 1 + GetRandomWeightedIndex(Enumerable.Range(0, level).ToArray());//Select(x => x * 10)
        diference = diference * mod.tier;
        mod.upperBound += diference;
        mod.lowerBound += diference;
        mod.upperBound = UnityEngine.Random.Range(mod.lowerBound, mod.upperBound);

        return mod;
    }

    public void GenerateInteriorStats(GunMods gun, BaseMod mod)
    {
        switch (mod.text)
        {
            case "Weapon Damage":
                gun.weaponDamage += mod.upperBound;
                break;

            case "Bullet Ricochet":
                gun.maxRicochets += mod.upperBound;
                break;

            case "Weapon Fire Rate":
                gun.fireRate = gun.fireRate * (1 + (mod.upperBound / 100));
                break;
        }
    }

    public void GenerateExteriorStats(GunMods gun, BaseMod mod)
    {
        switch (mod.text)
        {
            case "Weapon Damage":
                gun.weaponDamage += mod.upperBound;
                break;

            case "Bullet Ricochet":
                gun.maxRicochets += mod.upperBound;
                break;

            case "Weapon Fire Rate":
                gun.fireRate = gun.fireRate * (1 + (mod.upperBound / 100));
                break;
        }
    }

    public void GenerateSpecialStats(GunMods gun, BaseMod mod)
    {
        switch (mod.text)
        {
            case "Weapon Damage":
                gun.weaponDamage += mod.upperBound;
                break;

            case "Bullet Ricochet":
                gun.maxRicochets += mod.upperBound;
                break;

            case "Weapon Fire Rate":
                gun.fireRate = gun.fireRate * (1 + (mod.upperBound / 100));
                break;
        }
    }

    private int GenerateGrade(GunMods gun)
    {
        var interior = 0;
        var exterior = 0;
        var special = 0;

        foreach (var mod in gun.mods)
        {
            if (mod.grade == grade.interior) interior++;
            else if (mod.grade == grade.exterior) exterior++;
            else special++;
        }

        int[] gradeWeights = {
        300 - (interior * 100),
        300 - (exterior * 100),
        2 - special };

        return GetRandomWeightedIndex(gradeWeights);
    }



    private BaseMod GenerateExteriorMod(GunMods gun)
    {
        BaseMod aux;
        while (true)
        {
            aux = modsInterior[GetRandomWeightedIndex(ExteriorWeight)];
            if (!ContainsMod(gun, aux)) break;
        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier);
    }

    private BaseMod GenerateSpecialMod(GunMods gun)
    {
        BaseMod aux;
        while (true)
        {
            aux = modsInterior[GetRandomWeightedIndex(SpecialWeight)];
            if (!ContainsMod(gun, aux)) break;
        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier);
    }

    //gets a weapon and a mod and sees if the gun doesnt have the mod
    private bool ContainsMod(GunMods gun, BaseMod modifier)
    {

        foreach (BaseMod mod in gun.mods)
        {
            if (mod == modifier) return true;
        }
        return false;

    }

    public string CreateText(BaseMod mod)
    {
        string ret = "";
        switch (mod.op)
        {
            case operatorType.plus:
                ret = "+" + mod.upperBound + " " + mod.text;
                break;

            case operatorType.increased:
                ret = mod.upperBound + "% increased " + mod.text;
                break;

            case operatorType.reduced:
                ret = mod.upperBound + "% reduced " + mod.text;
                break;
        }

        return ret;
    }

    public string GenerateText(GunMods gun)
    {
        string text = "";
        foreach (var mod in gun.mods)
        {
            text += mod.text + "\n"; //string.Join(mod.text, "\n");
            //Console.WriteLine(mod.text);
        }

        return text;
    }
}