using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModData : MonoBehaviour
{
    public static Mod[] modsInterior =
            new Mod[]{
                new Mod(4,10,  "Weapon Damage",            Grade.interior, OperatorType.increased, 0, 1),
                new Mod(1,2,   "Bullet Ricochet",          Grade.interior, OperatorType.plus, 0,2),
                new Mod(2,3,   "Weapon Fire Rate",         Grade.interior, OperatorType.increased, 0,3),
                //new Mod(5,10,  "Headshot Damage",          grade.interior, operatorType.increased, 0)

                new Mod(290,310,  "Weapon Damage",           Grade.interior, OperatorType.increased, 1,1),
                new Mod(95,105,   "Weapon Fire Rate",         Grade.interior, OperatorType.increased, 1,3),
                new Mod(8,9,   "Bullets per Shot",          Grade.interior, OperatorType.plus, 1,4)

            };

    public static Mod[] modsExterior =
        new Mod[]{
                new Mod(1,3,  "Movement Speed",            Grade.exterior, OperatorType.increased, 0,101),
                new Mod(1,3,   "Bullet Ricochet",          Grade.exterior, OperatorType.plus, 0,102),
                new Mod(2,3,   "Weapon Fire Rate",         Grade.exterior, OperatorType.increased, 0,103)
        };

    public static Mod[] modSpecial =
        new Mod[]{
                new Mod(4,10,  "Weapon  Damage",           Grade.special, OperatorType.plus, 0,201),
                new Mod(1,3,   "Bullet Ricochet",          Grade.special, OperatorType.plus, 0,202),
                new Mod(2,3,   "Weapon Fire Rate",         Grade.special, OperatorType.increased, 0,203)
        };


    public static int[] InteriorWeight = { 10, 10, 10, 2, 2, 2 };

    public static int[] ExteriorWeight = { 1, 1, 1 };  //

    public static int[] SpecialWeight = { 1, 1, 1 };

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
    public Grade grade;
    public OperatorType op;

    public int tier;
    public int id;

    public Mod(int lowerBound, int upperBound, string text, Grade type, OperatorType op, int tier, int id)
    {
        this.lowerBound = lowerBound;
        this.upperBound = upperBound;
        this.text = text;
        this.grade = type;
        this.op = op;
        this.tier = tier;
        this.id = id;
    }
    public Mod()
    {
    }
}

public class GunOfAType
{
    public float baseRate;
    public int baseDamage;
    public int baseBounces;
    public int level;
    public int baseBulletsPerShot; 
    public GunType type;

    public float increasedFireRate;
    public float increasedDamage;
    public int additionalBounces;
    public float increasedSpeed;

    public string text;

    public HashSet<Mod> mods;

    public float finalFireRate;
    public float finalDamage;
    public int finalBounces;
    public float averageDamage;

    public float GetFireRate()
    {
        return baseRate * (1 + (increasedFireRate / 100));
    }

    public float GetDamage()
    {

        return (float)baseDamage * (1 + (increasedDamage / 100));

    }

    public int GetBounces()
    {
        return baseBounces + additionalBounces;
    }

    public float GetAverageDamage()
    {
        return GetDamage() * GetFireRate() * baseBulletsPerShot;
    }

    public GunOfAType ChangeType(GunOfAType gun, GunType type)
    {
        switch (type)
        {
            case GunType.normal:
                baseRate = 2f;
                baseDamage = 50;
                baseBounces = 2;
                baseBulletsPerShot = 1;
                break;
            case GunType.shotgun:
                baseRate = 1f;
                baseDamage = 20;
                baseBounces = 1;
                baseBulletsPerShot = 8;
                break;
            case GunType.machinegun:
                baseRate = 5f;
                baseDamage = 20;
                baseBounces = 1;
                baseBulletsPerShot = 1;
                break;
            case GunType.sniper:
                baseRate = .5f;
                baseDamage = 200;
                baseBounces = 10;
                baseBulletsPerShot = 1;
                break;
        }
        return gun;
    }
}
