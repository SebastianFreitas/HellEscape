

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunGenerator : ModData
{
    public int[] gradeWeight = { 300, 300, 2 };
    public GunOfAType CreateWeapon(int maxLevel, GunType type)
    {
        GunOfAType ret = new GunOfAType
        {
            mods = new HashSet<Mod>(),
            baseRate = 0,
            baseDamage = 0,
            baseBounces = 0,
            bounceSpeed = 0,
            baseBulletsPerShot = 1,
            level = maxLevel,
            increasedFireRate = 0,
            increasedDamage = 0,
            additionalBounces = 0,
            increasedSpeed = 0
        };

        ret = ret.ChangeType(ret, type);

        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        int nextGrade;
        var newMod = new Mod();


        for (int i = 0; i < totalMods; i++)
        {
            nextGrade = GetRandomWeightedIndex(gradeWeight);
            switch (nextGrade)
            {
                case 0:
                    gradeWeight[nextGrade] -= 100;
                    newMod = GenerateInteriorMod(ret);
                    newMod = UpdateMod(newMod, maxLevel, 1);
                    break;
                case 1:
                    gradeWeight[nextGrade] -= 100;
                    newMod = GenerateExteriorMod(ret);
                    newMod = UpdateMod(newMod, maxLevel, 1);
                    break;
                case 2:
                    gradeWeight[nextGrade] -= 1;
                    newMod = GenerateSpecialMod(ret);
                    newMod = UpdateMod(newMod, maxLevel, 1);

                    break;
            }

            switch (newMod.text)
            {
                case "Weapon Damage":
                    ret.increasedDamage += newMod.upperBound;
                    break;

                case "Bullet Ricochet":
                    ret.baseBounces += newMod.upperBound;
                    break;

                case "Weapon Fire Rate":
                    ret.increasedFireRate += newMod.upperBound;
                    break;
                case "Movement Speed":
                    ret.increasedSpeed += newMod.upperBound;
                    break;
                case "Bullets per Shot":
                    ret.baseBulletsPerShot += newMod.upperBound;
                    break;
            }
            newMod.text = CreateModText(newMod);
            ret.mods.Add(newMod);
        }


        ret.text = CreateGunText(ret);
        ret.finalFireRate = ret.GetFireRate();
        ret.finalDamage = ret.GetDamage();
        ret.finalBounces = ret.GetBounces();
        ret.averageDamage = ret.GetAverageDamage();
        return ret;
    }


    private Mod UpdateMod(Mod mod, int level, int reverse)
    {
        int diference = mod.upperBound - mod.lowerBound;
        if (reverse < 0) return mod;

        mod.tier = 1 + GetRandomWeightedIndex(modWeight);//
        diference = diference * mod.tier;
        mod.upperBound += diference;
        mod.lowerBound += diference;
        mod.upperBound = UnityEngine.Random.Range(mod.lowerBound, mod.upperBound);

        return mod;
    }

    private Mod GenerateInteriorMod(GunOfAType gun)
    {
        Mod aux;
        while (true)
        {
            aux = modsInterior[GetRandomWeightedIndex(InteriorWeight)];
            if (!ContainsMod(gun, aux)) break;

        }

        return new Mod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    private Mod GenerateExteriorMod(GunOfAType gun)
    {
        Mod aux;
        while (true)
        {
            aux = modsExterior[GetRandomWeightedIndex(ExteriorWeight)];
            if (!ContainsMod(gun, aux)) break;
        }

        return new Mod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    private Mod GenerateSpecialMod(GunOfAType gun)
    {
        Mod aux;
        while (true)
        {
            aux = modSpecial[GetRandomWeightedIndex(SpecialWeight)];
            if (!ContainsMod(gun, aux)) break;

        }

        return new Mod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    private bool ContainsMod(GunOfAType gun, Mod modifier)
    {

        foreach (Mod mod in gun.mods)
        {
            if (mod.id == modifier.id) return true;
        }
        return false;

    }

    public string CreateModText(Mod mod)
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
            case OperatorType.minus:
                ret = mod.upperBound + " " + mod.text;
                break;
            case OperatorType.non:
                ret = mod.text;
                break;
        }

        return ret;
    }

    public string CreateGunText(GunOfAType gun)
    {
        string text = "";
        text += gun.GetAverageDamage() + "  DPS\n";
        text += gun.GetDamage() + "   Damage\n";

        text += gun.GetFireRate() + "  Fire rate\n";
        text += gun.GetBounces() + "  Max Ricochets\n";
        text += "---------------------------------\n";

        foreach (var mod in gun.mods)
        {
            text += mod.text + "\n";
        }

        return text;
    }
}