

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModGenerator : ModBase
{
    public int[] gradeWeight = { 300, 300, 2 };
    public GunMods CreateWeapon(int maxLevel)
    {
        GunMods ret = new GunMods
        {
            mods = new HashSet<BaseMod>(),
            fireRate        = baseFireRate,
            weaponDamage    = baseDamage,
            maxRicochets    = BaseRicochets,
            level = maxLevel
        };

        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        int nextGrade;
        

        for (int i = 0; i < totalMods; i++)
        {

            nextGrade = GetRandomWeightedIndex(gradeWeight);
            switch (nextGrade)
            {
                case 0:
                    gradeWeight[nextGrade] -= 100;
                    var intMod = GenerateInteriorMod(ret);
                    if (intMod.tier == 1)
                    {
                        var extraMod = CreateExtraMod(intMod, maxLevel);
                        extraMod = UpdateMod(extraMod, maxLevel, -1);
                        switch (extraMod.text)
                        {
                            case "Weapon Damage":
                                ret.weaponDamage += extraMod.upperBound;
                                break;

                            case "Bullet Ricochet":
                                ret.maxRicochets += extraMod.upperBound;
                                break;

                            case "Weapon Fire Rate":
                                ret.fireRate *= (1 + (extraMod.upperBound / 100));
                                break;
                        }
                        extraMod.text = CreateText(extraMod);
                        ret.mods.Add(extraMod);

                    }
                    intMod = UpdateMod(intMod, maxLevel,1);
                    switch (intMod.text)
                    {
                        case "Weapon Damage":
                            ret.weaponDamage += intMod.upperBound;
                            break;

                        case "Bullet Ricochet":
                            ret.maxRicochets += intMod.upperBound;
                            break;

                        case "Weapon Fire Rate":
                            ret.fireRate *= (1 + (intMod.upperBound / 100));
                            break;
                    }
                    intMod.text = CreateText(intMod);
                    ret.mods.Add(intMod);
                    break;
                case 1:
                    gradeWeight[nextGrade] -= 100;
                    var extMod = GenerateExteriorMod(ret);
                    extMod = UpdateMod(extMod, maxLevel,1);
                    switch (extMod.text)
                    {
                        case "Movement Speed":
                            ret.movementSpeed += extMod.upperBound;
                            break;

                        case "Bullet Ricochet":
                            ret.maxRicochets += extMod.upperBound;
                            break;

                        case "Weapon Fire Rate":
                            ret.fireRate *= (1 + (extMod.upperBound / 100));
                            break;
                    }
                    extMod.text = CreateText(extMod);
                    ret.mods.Add(extMod);
                    break;
                case 2:
                    gradeWeight[nextGrade] -= 1;
                    var newMod = GenerateSpecialMod(ret);
                    newMod = UpdateMod(newMod, maxLevel,1);
                    switch (newMod.text)
                    {
                        case "Weapon Damage":
                            ret.weaponDamage += newMod.upperBound;
                            break;

                        case "Bullet Ricochet":
                            ret.maxRicochets += newMod.upperBound;
                            break;

                        case "Weapon Fire Rate":
                            ret.fireRate = ret.fireRate * (1 + (newMod.upperBound / 100));
                            break;
                    }
                    newMod.text = CreateText(newMod);
                    ret.mods.Add(newMod);
                    break;
            }

        }
        ret.text = GenerateText(ret);
        return ret;
    }

    private BaseMod CreateExtraMod(BaseMod mod, int level)
    {
        BaseMod aux = modExtra[0];  
        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }




    //Creates tier 
    private BaseMod UpdateMod(BaseMod mod, int level, int reverse)
    {
        int diference = mod.upperBound - mod.lowerBound;
        //if (reverse < 0) mod.tier = 1 + GetRandomWeightedIndex(Enumerable.Range(0, level).Select(x => x * 10).ToArray());//
        if (reverse < 0)  return mod;
        mod.tier = 1 + GetRandomWeightedIndex(Enumerable.Range(0, level).Select(x => x * 10).Reverse().ToArray());//
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
            if (mod.grade == Grade.interior) interior++;
            else if (mod.grade == Grade.exterior) exterior++;
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
            aux = modsInterior[GetRandomWeightedIndex(InteriorWeight)];
            if (!ContainsMod(gun, aux)) break;

        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    private BaseMod GenerateExteriorMod(GunMods gun)
    {
        BaseMod aux;
        while (true)
        {
            aux = modsExterior[GetRandomWeightedIndex(ExteriorWeight)];
            if (!ContainsMod(gun, aux)) break;
        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    private BaseMod GenerateSpecialMod(GunMods gun)
    {
        BaseMod aux;
        while (true)
        {
            aux = modSpecial[GetRandomWeightedIndex(SpecialWeight)];
            if (!ContainsMod(gun, aux)) break;

        }

        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    //gets a weapon and a mod and sees if the gun doesnt have the mod
    private bool ContainsMod(GunMods gun, BaseMod modifier)
    {

        foreach (BaseMod mod in gun.mods)
        {
            if (mod.id == modifier.id) return true;
        }
        return false;

    }

    public string CreateText(BaseMod mod)
    {
        string ret = "";
        switch (mod.op)
        {
            case OperatorType.plus:
                ret = "+" + mod.upperBound + " " + mod.text;
                break;

            case OperatorType.increased:
                ret = mod.upperBound + "% increased " + mod.text;
                break;

            case OperatorType.reduced:
                ret = mod.upperBound + "% reduced " + mod.text;
                break;
            case OperatorType.decreased:
                ret = mod.upperBound + "% decreased " + mod.text;
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
            Debug.Log(mod.text);
            //Console.WriteLine(mod.text);
        }

        return text;
    }
}