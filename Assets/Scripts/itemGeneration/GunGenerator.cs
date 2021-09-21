

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunGenerator : ModData
{

    public int[] typeWeight = { 50, 10, 10, 10 };
    public GunOfAType CreateWeapon(int maxLevel)
    {
        GunOfAType ret = new GunOfAType
        {
            mods = new HashSet<Mod>(),
            baseRate = 0,
            basePhysicalDamage = 0,
            baseBounces = 0,
            basebounceSpeed = 0,
            baseBulletsPerShot = 1,
            level = maxLevel,
            increasedFireRate = 0,
            increasedDamage = 0,
            increasedBounces = 0,
            increasedSpeed = 0,
            baseFireDamage = 0,
            baseColdDamage =0,
            basePoisonDamage = 0,
        };
        var type = RandomiseGunType();
        ret = ret.ChangeType(ret, type);

        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        var newMod = new Mod();


        for (int i = 0; i < totalMods; i++)
        {
            newMod = AddMod(maxLevel, ret);
        }
        ret.GenerateTotalDamage();
        return FinishWeaponText(ret);
    }

    public GunOfAType FinishWeaponText(GunOfAType ret)
    {
        ret.text = CreateGunText(ret);
        ret.finalFireRate = ret.GetFireRate();
        ret.finalDamage = ret.GetDamage();
        ret.finalBounces = ret.GetBounces();
        ret.averageDamage = ret.GetAverageDamage();
        return ret;
    }

    public Mod AddMod(int maxLevel, GunOfAType ret)
    {
        var newMod = new Mod();
        int nextGrade = GetRandomWeightedIndex(ret.gradeWeight);
        switch (nextGrade)
        {
            case 0:
                ret.gradeWeight[nextGrade] -= 100;
                newMod = GenerateInteriorMod(ret);
                newMod = UpdateMod(newMod, maxLevel, 1);
                break;
            case 1:
                ret.gradeWeight[nextGrade] -= 100;
                newMod = GenerateExteriorMod(ret);
                newMod = UpdateMod(newMod, maxLevel, 1);
                break;
            case 2:
                ret.gradeWeight[nextGrade] -= 1;
                newMod = GenerateSpecialMod(ret);
                newMod = UpdateMod(newMod, maxLevel, 1);
                break;
        }

        ModToStat(ret, newMod);
        newMod.text = CreateModText(newMod);
        ret.mods.Add(newMod);
        ret.GenerateTotalDamage();
        return newMod;
    }

    private static void ModToStat(GunOfAType ret, Mod newMod)
    {
        switch (newMod.basicText)
        {
            case "Physical Damage":
                if (newMod.op.Equals(OperatorType.plus)) ret.addedPhysicalDamage += newMod.upperBound;
                else ret.increasedPhysicalDamage += newMod.upperBound;
                break;

            case "Critical Damage":
                ret.increasedCriticalDamage += newMod.upperBound;
                break;

            case "Fire Damage":
                ret.addedFireDamage += newMod.upperBound;
                break;

            case "Cold Damage":
                ret.addedColdDamage += newMod.upperBound;
                break;

            case "Poison Damage":
                ret.addedPoisonDamage += newMod.upperBound;
                break;

            case "Ricochets":
                ret.baseBounces += newMod.upperBound;
                break;

            case "Fire Rate":
                ret.increasedFireRate += newMod.upperBound;
                break;

            case "Movement Speed":
                ret.increasedSpeed += newMod.upperBound;
                break;

            case "Bullets per Shot":
                ret.increasedBounces += newMod.upperBound;
                break;

            case "Bullet Speed":
                ret.increasedshotSpeed += newMod.upperBound;
                break;

            case "Ricochets follow enemies":
                ret.increasedRicochetGuide += newMod.upperBound;
                break;
        }
    }

    public GunOfAType CreateWeaponEmpty()
    {
        GunOfAType ret = new GunOfAType();
        ret = ret.ChangeType(ret, GunType.normal);

        ret.mods = new HashSet<Mod>();

        ret.text = CreateGunText(ret);
        ret.finalFireRate = ret.GetFireRate();
        ret.finalDamage = ret.GetDamage();
        ret.finalBounces = ret.GetBounces();
        ret.averageDamage = ret.GetAverageDamage();

        return ret;
    }
    private GunType RandomiseGunType()
    {
        var x = UnityEngine.Random.Range(0, 3);

        GunType ret = GunType.normal;
        switch (x)
        {
            case 0:
                ret = GunType.normal;
                break;

            case 1:
                ret = GunType.sniper;
                break;

            case 2:
                ret = GunType.machinegun;
                break;

            case 3:
                ret = GunType.shotgun;
                break;
        }

        return ret;
    }

    private Mod UpdateMod(Mod mod, int level, int reverse)
    {
        int diference = mod.upperBound - mod.lowerBound;
        if (reverse < 0) return mod;

        int[] x = Enumerable.Range(1, level).ToArray();

        mod.tier = 1 + GetRandomWeightedIndex(x);//
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
        if (gun.mods.Count == 0) return false;
        foreach (Mod mod in gun.mods)
        {
            if (mod.id == modifier.id) return true;
        }
        return false;

    }

    public string CreateModText(Mod mod)
    {
        mod.basicText = mod.text;
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
        text += "-----------------------\n-----------------------\n";
        if (gun.mods != null)
        {
            foreach (var mod in gun.mods)
            {
                text += mod.text + "\n";
            }

        }

        return text;
    }

    public string CreateGunStats(GunOfAType gun)
    {
        string text = "";
        text += gun.GetAverageDamage() + "\n";
        text += gun.GetDamage() + "\n";
        text += gun.GetFireRate() + "\n";
        text += gun.GetBounces() + "\n";

        return text;
    }

    public void RemoveMod(GunOfAType gun, Mod mod)
    {
        gun.mods.Remove(mod);
        switch (mod.grade.ToString())
        {
            case "interior":
                gun.gradeWeight[0] += 100;
                break;

            case "exterior":
                gun.gradeWeight[1] += 100;
                break;

            case "special":
                gun.gradeWeight[2] += 1;
                break;
        }
        mod.upperBound = -mod.upperBound;
        ModToStat(gun, mod);
        gun.GenerateTotalDamage();

    }
}