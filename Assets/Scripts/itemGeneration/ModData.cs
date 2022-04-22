using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModData : MonoBehaviour
{
    public static Mod[] modsInterior =
            new Mod[]{
                new Mod(1,3,   "Fire Damage",               Grade.interior, OperatorType.plus,      0,1),
                new Mod(1,3,   "Cold Damage",               Grade.interior, OperatorType.plus,      0,2),
                new Mod(1,3,   "Poison Damage",             Grade.interior, OperatorType.plus,      0,3),
                new Mod(2,5,   "Physical Damage",           Grade.interior, OperatorType.increased, 0,4),
                new Mod(2,5,   "Critical Damage",           Grade.interior, OperatorType.increased, 0,5),
                

            };

    public static Mod[] modsExterior =
        new Mod[]{
                new Mod(1, 3,   "Movement Speed",            Grade.exterior, OperatorType.increased, 0,1),
                new Mod(1,6,    "Ricochets",                 Grade.exterior, OperatorType.increased, 0,2),
                new Mod(1, 3,   "Fire Rate",                 Grade.exterior, OperatorType.increased, 0,3),
                new Mod(2,5,    "Bullet Speed",              Grade.exterior, OperatorType.increased, 0,4),
                new Mod(1, 5,   "Bullet Size",              Grade.exterior, OperatorType.increased, 0,5),
              
        };

    public static Mod[] modSpecial =
        new Mod[]{
                new Mod(2,6,   "Fire Damage",               Grade.special, OperatorType.plus,      0,1),
                new Mod(2,6,   "Cold Damage",               Grade.special, OperatorType.plus,      0,2),
                new Mod(2,6,   "Poison Damage",             Grade.special, OperatorType.plus,      0,3),
                new Mod(4,10,  "Physical Damage",           Grade.special, OperatorType.increased, 0,4),
                new Mod(4,10,  "Critical Damage",           Grade.special, OperatorType.increased, 0,5),
                new Mod(4,10,  "Delayed Shot",              Grade.special, OperatorType.non, 0,6),

                new Mod(4,10,  "Horizontal Shot",           Grade.special, OperatorType.non, 0,7),
                new Mod(4,10,  "Zero-gravity Shot",         Grade.special, OperatorType.non, 0,8),
               
                new Mod(4,10,  "Piercing Shot",             Grade.special, OperatorType.non, 0,9),

        };


    public static int[] InteriorWeight = { 1, 1, 1, 10, 10};

    public static int[] ExteriorWeight = { 3, 3, 1, 3, 3};  //

    public static int[] SpecialWeight = { 30, 30, 30, 30, 20, 2, 5, 5, 5};// 1, 1, 1, 1, 1, 1 };

    public static int[] modWeight = { 1000, 900, 800, 700, 600, 500, 400, 300, 200, 100 };



    //int[] gradeWeights ={300,300,2};//chances of rolling grade type on an empty item
    public static int[] maxModsWeight = { 700, 900, 900, 700, 500, 100, 10, 1 };   //chances for total mods an item will have when rolled

    public int GetRandomWeightedIndex(int[] weights)
    {
        // Get the total sum of all the weights.
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;//elementCount
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }

        // No other item was selected, so return very last index.
        return index;
    }
}

public enum OperatorType
{
    plus,
    increased,
    reduced,
    decreased,
    non,
    minus
}

public enum Grade
{
    interior,
    exterior,
    special,
    extra
}

public enum GunType
{
    normal,
    shotgun,
    machinegun,
    sniper
}

public class Mod
{
    public int upperBound;
    public int lowerBound;
    public string text;
    public string basicText;
    public Grade grade;
    public OperatorType op;

    public int tier;
    public int id;

    public Mod(int lowerBound, int upperBound, string text, Grade type, OperatorType op, int tier, int id)
    {
        this.lowerBound = lowerBound;
        this.upperBound = upperBound;
        this.text = text;
        this.basicText = text;
        this.grade = type;
        this.op = op;
        this.tier = tier;
        this.id = id;
    }

    public Mod(Mod a)
    {
        this.upperBound = a.upperBound;
        this.lowerBound = a.lowerBound;
        this.text = a.text;
        this.basicText = a.basicText;
        this.grade = a.grade;
        this.op = a.op;
        this.tier = a.tier;
        this.id = a.id;
    }

    public Mod()
    {
        this.upperBound = 0;
        this.lowerBound = 0;
        this.text = "";
        this.basicText = "";
        this.grade = Grade.extra;
        this.op = OperatorType.non;
        this.tier = 1;
        this.id = -1;
    }
}

//[System.Serializable]
public class GunOfAType : ScriptableObject
{
    public float[] totalDamage = {0,0,0,0};

    public float baseRate;
    public int basePhysicalDamage;
    public int baseBounces;
    public int basebounceSpeed;
    public int level;
    public int baseBulletsPerShot;
    public int baseshotSpeed;
    public int basebulletSize;

    public int baseFireDamage;
    public int baseColdDamage;
    public int basePoisonDamage;

    public GunType type;

    public int addedPhysicalDamage;
    public int addedFireDamage;
    public int addedPoisonDamage;
    public int addedColdDamage;

    public float increasedPhysicalDamage;
    public float increasedFireDamage;
    public float increasedColdDamage;
    public float increasedPoisonDamage;

    public float increasedFireRate;
    public float increasedDamage;
    public int   increasedBounces;
    public float increasedSpeed;
    public float increasedBulletSize = 0;
    public float increasedshotSpeed;
    public float increasedCriticalDamage;

    public float increasedRicochetGuide;

    public string text;

    public HashSet<Mod> mods;
    public int[] gradeWeight = { 200, 200, 2 };

    public float finalFireRate;
    public float finalDamage;
    public int finalBounces;
    public float averageDamage;

    public bool isBase = false;
    internal float delayedBullet;
    internal float horizontalShot;
    internal float slowbullet;
    internal float piercing;

    public float GetShotSpeed()
    {
        return baseshotSpeed * (1 + (increasedshotSpeed / 100));
    }
    public float GetFireRate()
    {
        return baseRate * (1 + (increasedFireRate / 100));
    }

    public float GetDamage()
    {
        return GetPhysicalDamage() + GetFireDamage() + GetColdDamage() + GetPoisonDamage();
    }

    public float GetCriticalDamage()
    {
        return GetDamage() * 2 * (1 + increasedCriticalDamage/100);
    }
    public void GenerateTotalDamage()
    {
        totalDamage[0] += GetPhysicalDamage();
        totalDamage[1] += GetFireDamage();
        totalDamage[2] += GetColdDamage();
        totalDamage[3] += GetPoisonDamage();
    }

    public float GetPhysicalDamage()
    {

        return (float)(basePhysicalDamage + addedPhysicalDamage) * (1 + (increasedPhysicalDamage / 100));

    }

    public float GetFireDamage()
    {

        return (float)(baseFireDamage + addedFireDamage) * (1 + (increasedFireDamage / 100));

    }

    public float GetColdDamage()
    {

        return (float)(baseColdDamage + addedColdDamage) * (1 + (increasedColdDamage / 100));

    }

    public float GetPoisonDamage()
    {

        return (float)(basePoisonDamage + addedPoisonDamage) * (1 + (increasedPoisonDamage / 100));

    }

    public int GetBounces()
    {
        return baseBounces * (1 + (increasedBounces / 100));
    }

    public float GetAverageDamage()
    {
        return GetDamage() * GetFireRate() * baseBulletsPerShot;
    }

    public HashSet<Mod> GetExteriorMods()
    {

        var ret = new HashSet<Mod>();

        foreach(Mod x in mods)
        {
            if (x.grade.ToString() == "Exterior") ret.Add(x);
        }

        return ret;
    }

    public HashSet<Mod> GetInteriorMods()
    {

        var ret = new HashSet<Mod>();

        foreach (Mod x in mods)
        {
            if (x.grade.ToString() == "Interior") ret.Add(x);
        }

        return ret;
    }

    public HashSet<Mod> GetSpecialMods()
    {

        var ret = new HashSet<Mod>();

        foreach (Mod x in mods)
        {
            if (x.grade.ToString() == "Special") ret.Add(x);
        }

        return ret;
    }

    public GunOfAType ChangeType(GunOfAType gun, GunType type)
    {
        gun.type = type;

        switch (type)
        {
            case GunType.normal:
                baseRate = 2f;
                basePhysicalDamage = 50;
                baseBounces = 5;
                baseBulletsPerShot = 1;
                baseshotSpeed = 2500;
                basebounceSpeed = 250;
                break;

            case GunType.shotgun:
                baseRate = 1f;
                basePhysicalDamage = 20;
                baseBounces = 5;
                baseBulletsPerShot = 8;
                baseshotSpeed = 1500;
                basebounceSpeed = 150;
                break;

            case GunType.machinegun:
                baseRate = 5f;
                basePhysicalDamage = 20;
                baseBounces = 5;
                baseBulletsPerShot = 1;
                baseshotSpeed = 1500;
                basebounceSpeed = 150;
                break;

            case GunType.sniper:
                baseRate = .5f;
                basePhysicalDamage = 200;
                baseBounces = 10;
                baseBulletsPerShot = 1;
                baseshotSpeed = 5000;
                basebounceSpeed = 500;
                break;
        }
        return gun;
    }

    public override bool Equals(object obj)
    {
        return obj is GunOfAType g &&
               level == g.level &&
               this.type == g.type &&
               EqualityComparer<HashSet<Mod>>.Default.Equals(mods, g.mods);

    }
}
