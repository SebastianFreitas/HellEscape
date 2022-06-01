

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunGenerator : ModData
{

    public int[] typeWeight = { 50, 10, 10, 10 };
    public GunOfAType CreateWeapon(int maxLevel, bool isBase)
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
        if (isBase) type = GunType.basic;
        ret = ret.ChangeType(ret, type);
        if (maxLevel > 2) maxLevel /= 2;
        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        var newMod = new Mod();

        if (!isBase)
        {
            for (int i = 0; i < totalMods; i++)
            {
                newMod = AddMod(maxLevel, ret);
            }
        }

        ret.GenerateTotalDamage();
        return FinishWeaponText(ret);
    }

    public Mod CreateModSeed(int id, int value, Grade grade, int tier)
    {
        Mod ret = new Mod();

        if (id == -1)
        {
            ret.id = -1;
            return ret;
        }



        switch (grade)
        {
            case Grade.interior:
                ret =new Mod(modsInterior[id-1]);
                break;
            case Grade.exterior:
                ret = new Mod(modsExterior[id - 1]);
                break;
            case Grade.special:
                ret = new Mod(modSpecial[id - 1]);
                break;
        }

        ret.upperBound = value;
        ret.lowerBound = value;
        ret.tier = tier;

        return ret;
    }

    public GunOfAType CreateWeaponSeed(int maxlevel, GunType type, Mod[] mods, int weight0, int weight1, int weight2)
    {
        GunOfAType ret = new GunOfAType();

        ret.mods = new HashSet<Mod>();

        ret.ChangeType(ret, type);
        ret.level = maxlevel;

        for (int i = 0; i < mods.Length; i++)
        {
            if (mods[i].id > -1)
            {
                ModToStat(ret, mods[i]);
                mods[i].text = CreateModText(mods[i]);
                ret.mods.Add(mods[i]);
                ret.GenerateTotalDamage();
            }

        }
        ret.gradeWeight = new int[3];
        ret.gradeWeight[0] = weight0;
        ret.gradeWeight[1] = weight1;
        ret.gradeWeight[2] = weight2;
        ret.GenerateTotalDamage();
        return FinishWeaponText(ret);
    }

    public GunOfAType FinishWeaponText(GunOfAType ret)
    {
        ret.text = CreateGunText(ret);
        ret.finalFireRate = ret.GetFireRate();
        ret.finalDamage = ret.GetDamage();
        ret.finalBounces = (int)ret.GetBounces();
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
            case "Bullet Size":
                ret.increasedBulletSize += newMod.upperBound;
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
                ret.increasedBounces +=  newMod.upperBound;
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

            case "Delayed Shot":
                ret.delayedBullet += newMod.upperBound;
                break;
            //-----------------------------------------------------------
            case "Explosion Area":
                ret.fireExplosionAreaModifier += newMod.upperBound;
                break;

            case "Ignite Chance":
                ret.igniteChance += newMod.upperBound;
                break;
            //-----------------------------------------------------------
            case "Poison Rate":
                ret.poisonRateModifier += newMod.upperBound;
                break;

            case "Poison Duration":
                ret.poisonDurationModifier += newMod.upperBound;
                break;
            //-----------------------------------------------------------
            case "Freeze Chance":
                ret.freezeChance += newMod.upperBound;
                break;

            case "Cold projectile Chance":
                ret.ColdProjectileChance += newMod.upperBound;
                break;
            //-----------------------------------------------------------
            case "Double Damage chance":
                ret.doubleDamageChance += newMod.upperBound;
                break;

            case "Bleeding Chance":
                ret.bleedChance += newMod.upperBound;
                break;

        }
    }

    public GunOfAType CreateWeaponEmpty()
    {
        GunOfAType ret = new GunOfAType();
        ret = ret.ChangeType(ret, GunType.basic);

        ret.mods = new HashSet<Mod>();

        ret.isBase = true;
        ret.text = CreateGunText(ret);
        ret.finalFireRate = ret.GetFireRate();
        ret.finalDamage = ret.GetDamage();
        ret.finalBounces = (int)ret.GetBounces();
        ret.averageDamage = ret.GetAverageDamage();

        return ret;
    }
    private GunType RandomiseGunType()
    {
        var x = UnityEngine.Random.Range(0, 4);
        var list = new List<(int, GunType)>();

        list.Add((100, GunType.basic));
        list.Add((75, GunType.sniper));
        list.Add((75, GunType.machinegun));
        list.Add((75, GunType.shotgun));

        list.Add((20, GunType.BasicA1));
        list.Add((15, GunType.BasicA1PD));
        list.Add((15, GunType.BasicA1FD));
        list.Add((15, GunType.BasicA1CD));

        list.Add((20, GunType.sniperA1));
        list.Add((15, GunType.sniperA1PD));
        list.Add((15, GunType.sniperA1FD));
        list.Add((15, GunType.sniperA1CD));

        list.Add((20, GunType.machinegunA1));
        list.Add((15, GunType.machinegunA1PD));
        list.Add((15, GunType.machinegunA1FD));
        list.Add((15, GunType.machinegunA1CD));

        list.Add((20, GunType.shotgunA1));
        list.Add((15, GunType.shotgunA1PD));
        list.Add((15, GunType.shotgunA1FD));
        list.Add((15, GunType.shotgunA1CD));
        var listweight = new List<GunType>();
        foreach (var item in list)
        {
            for (int i = 0; i < item.Item1; i++)
            {
                listweight.Add(item.Item2);
            }
        }

        GunType ret = listweight[UnityEngine.Random.Range(0, listweight.Count)];

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
            if((modifier.grade == Grade.special) && (mod.grade == Grade.special))
            {
                if ((mod.id > 6) && (modifier.id > 6)) return true;
            }
            if (mod.id == modifier.id && mod.grade == modifier.grade) return true;
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
        int x = (int)gun.GetShotSpeed() / 100;
        string text = "";
        text += gun.GetPhysicalDamage().ToString("F0") + "\n";
        text += gun.GetFireDamage().ToString("F0") + "\n";
        text += gun.GetColdDamage().ToString("F0") + "\n";
        text += gun.GetPoisonDamage().ToString("F0") + "\n";

        text += gun.GetFireRate().ToString("F0") + "\n";
        text += x.ToString("F0") + "\n";

        text += gun.GetBounces().ToString("F0") + "\n";

        return text;
    }

    private static string GetBaseText(GunOfAType gun)
    {
        int x = gun.baseshotSpeed / 100;
        string text = "";
        text += gun.basePhysicalDamage.ToString("F0") + "\n";
        text += gun.baseFireDamage.ToString("F0") + "\n";
        text += gun.baseColdDamage.ToString("F0") + "\n";
        text += gun.basePoisonDamage.ToString("F0") + "\n";

        text += gun.baseRate.ToString("F0") + "\n";
        text += x.ToString("F0") + "\n";

        text += gun.baseBounces.ToString("F0") + "\n";
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