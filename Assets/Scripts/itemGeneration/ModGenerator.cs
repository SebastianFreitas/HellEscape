

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
            baseRate = baseFireRate,
            baseDamage = baseDamage,
            baseBounces = BaseRicochets,
            level = maxLevel,
            increasedFireRate = 0,
            additionalDamage =0,
            additionalBounces=0,
            increasedSpeed=0
 
        };

        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        int nextGrade;
        var newMod = new BaseMod();
        var extra = false;





        for (int i = 0; i < totalMods; i++)
        {
            nextGrade = GetRandomWeightedIndex(gradeWeight);
            switch (nextGrade)
            {
                case 0:
                    gradeWeight[nextGrade] -= 100;
                    newMod = GenerateInteriorMod(ret);
                    if (newMod.tier == 1) extra = true;
                    newMod = UpdateMod(newMod, maxLevel,1);
                    break;
                case 1:
                    gradeWeight[nextGrade] -= 100;
                    newMod = GenerateExteriorMod(ret);
                    if (newMod.tier == 1) extra = true;
                    newMod = UpdateMod(newMod, maxLevel,1);
                    break;
                case 2:
                    gradeWeight[nextGrade] -= 1;
                    newMod = GenerateSpecialMod(ret);
                    if (newMod.tier == 1) extra = true;
                    newMod = UpdateMod(newMod, maxLevel,1);

                    break;
            }

            if (extra)
            {
                extra = false;
                var extraMod = CreateExtraMod(newMod, maxLevel);
                extraMod = UpdateMod(extraMod, maxLevel, -1);
                switch (extraMod.text)
                {
                    case "Max Fire Rate":
                        ret.baseRate = 0.5f;
                        break;

                }
                extraMod.text = CreateText(extraMod);
                ret.mods.Add(extraMod);
            }
            switch (newMod.text)
            {
                case "Weapon Damage":
                    ret.additionalDamage += newMod.upperBound;
                    break;

                case "Bullet Ricochet":
                    ret.baseBounces += newMod.upperBound;
                    break;

                case "Weapon Fire Rate":
                    ret.increasedFireRate +=  newMod.upperBound;
                    break;
                case "Movement Speed":
                    ret.increasedSpeed += newMod.upperBound;
                    break;
            }
            newMod.text = CreateText(newMod);
            ret.mods.Add(newMod);
        }
        ret.text = GenerateText(ret);
        

        return ret;
    }

    private BaseMod CreateExtraMod(BaseMod mod, int level)
    {
        BaseMod aux = modExtra[0];  
        return new BaseMod(aux.lowerBound, aux.upperBound, aux.text, aux.grade, aux.op, aux.tier, aux.id);
    }

    private BaseMod UpdateMod(BaseMod mod, int level, int reverse)
    {
        int diference = mod.upperBound - mod.lowerBound;
        if (reverse < 0)  return mod;

        mod.tier = 1 + GetRandomWeightedIndex(modWeight);//
        diference = diference * mod.tier;
        mod.upperBound += diference;
        mod.lowerBound += diference;
        mod.upperBound = UnityEngine.Random.Range(mod.lowerBound, mod.upperBound);

        return mod;
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
        text += " DPS: " + gun.GetAverageDamage() + "\n";
        text += "Fire rate: " + gun.GetFireRate() + "\n";
        text += "Max Ricochets: " + gun.GetBounces() + "\n";

        foreach (var mod in gun.mods)
        {
            text += mod.text + "\n"; 
        }

        return text;
    }
}