

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
        };//0f, baseFireRate, baseDamage, BaseRicochets, 0, new Dictionary<grade,HashSet<BaseMod>>());

        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        int nextGrade;

        for (int i = 0; i < totalMods; i++)
        {
            nextGrade = GenerateGrade(ret);
            switch (nextGrade)
            {
                case 0:
                    ret.mods.Add(GenerateInteriorMod(ret));
                    break;
                case 1:
                    ret.mods.Add(GenerateExteriorMod(ret));
                    break;
                case 2:
                    ret.mods.Add(GenerateSpecialMod(ret));
                    break;
            }
           
        }
        ConvergeModStats(ret);
        return ret;
    }

    private void FinishWeapon(GunMods gun)
    {
        //gun.interiorMods.Select(x => ApplyValues(gun, x));


    }

    private void ConvergeModStats(GunMods gun)
    {
        foreach(BaseMod mod in gun.mods)
        {
            UpdateMod(mod, gun.level);
            if (mod.grade == grade.interior) GenerateInteriorStats(gun, mod);
            else if (mod.grade == grade.exterior) GenerateInteriorStats(gun, mod);
            else GenerateInteriorStats(gun, mod);
            CreateText(mod);
        }

    }


    //Creates tier and text
    private void UpdateMod(BaseMod mod, int level)
    {
        int diference = mod.upperBound - mod.lowerBound;
        mod.tier = 1 + GetRandomWeightedIndex(Enumerable.Range(0, level).ToArray());//Select(x => x * 10)
        diference = diference * mod.tier;
        mod.upperBound += diference;
        mod.lowerBound += diference;
        mod.upperBound = UnityEngine.Random.Range(mod.lowerBound, mod.upperBound);
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

    private BaseMod GenerateInteriorMod(GunMods gun)
    {
        BaseMod aux;
        while (true)
        {
            aux =  modsInterior[GetRandomWeightedIndex(InteriorWeight)];
            if (!ContainsMod(gun, aux)) break;
        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier);
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

    public void CreateText(BaseMod mod)
    {
        switch (mod.op)
        {
            case operatorType.plus:
                mod.text = "+" + mod.upperBound + " " + mod.text;
                break;

            case operatorType.increased:
                mod.text = mod.upperBound + "% increased " + mod.text;
                break;

            case operatorType.reduced:
                mod.text = mod.upperBound + "% reduced " + mod.text;
                break;
        }
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