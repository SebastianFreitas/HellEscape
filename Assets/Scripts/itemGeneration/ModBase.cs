using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModBase : MonoBehaviour
{

    public int baseDamage = 50;
    public float baseFireRate = 2f;
    public int BaseRicochets = 1;
    public enum OperatorType
    {
        plus,
        increased,
        reduced,
        decreased
    }

    public enum Grade
    {
        interior,
        exterior,
        special,
        extra
    }
    public struct BaseMod
    {
        public int upperBound;
        public int lowerBound;
        public string text;
        public Grade grade;
        public OperatorType op;

        public int tier;
        public int id;

        public BaseMod(int lowerBound, int upperBound, string text, Grade type, OperatorType op, int tier, int id)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.text = text;
            this.grade = type;
            this.op = op;
            this.tier = tier;
            this.id = id;
        }

    }



    public struct GunMods
    {

        public float baseRate;
        public int baseDamage;
        public int baseBounces;
        public int level;

        public float increasedFireRate;
        public int additionalDamage;
        public int additionalBounces;
        public float increasedSpeed;

        public string text;

        public HashSet<BaseMod> mods;



        public GunMods( float fireRate, int weaponDamage, int maxRicochets, int lvl,
                        string text, HashSet<BaseMod> mods, float movementSpeed,
                        float increasedFireRate, int additionalDamage, int additionalBounces, float increasedSpeed)
        {
            this.baseRate = fireRate;
            this.baseDamage = weaponDamage;
            this.baseBounces = maxRicochets;
            this.level = lvl;//wtf
            this.mods = mods;
            this.text = text;
            this.increasedSpeed = movementSpeed;
            this.increasedFireRate = increasedFireRate;
            this.additionalDamage = additionalDamage;
            this.additionalBounces = additionalBounces;
            this.increasedSpeed = increasedSpeed;


        }

        public float GetFireRate()
        {
            return baseRate*(1+increasedFireRate/100);
        }

        public int GetDamage()
        {
            return baseDamage + additionalDamage;
        }

        public int GetBounces()
        {
            return baseBounces + additionalBounces;
        }

        public float GetAverageDamage()
        {
            return GetDamage() * GetFireRate();
        }

    }


    public static BaseMod[] modsInterior =
        new BaseMod[]{
                new BaseMod(4,10,  "Weapon  Damage",           Grade.interior, OperatorType.plus, 0, 1),
                new BaseMod(195,2005,  "Weapon  Damage",          Grade.interior, OperatorType.plus, 1,1),
                new BaseMod(1,2,   "Bullet Ricochet",          Grade.interior, OperatorType.plus, 0,3),
                new BaseMod(2,3,   "Weapon Fire Rate",         Grade.interior, OperatorType.increased, 0,4)
                //new BaseMod(5,10,  "Headshot Damage",          grade.interior, operatorType.increased, 0)
        };

    public static BaseMod[] modsExterior =
        new BaseMod[]{
                new BaseMod(1,3,  "Movement Speed",            Grade.exterior, OperatorType.increased, 0,101),
                new BaseMod(1,3,   "Bullet Ricochet",          Grade.exterior, OperatorType.plus, 0,102),
                new BaseMod(2,3,   "Weapon Fire Rate",         Grade.exterior, OperatorType.increased, 0,103)
        };

    public static BaseMod[] modSpecial =
        new BaseMod[]{
                new BaseMod(4,10,  "Weapon  Damage",           Grade.special, OperatorType.plus, 0,201),
                new BaseMod(1,3,   "Bullet Ricochet",          Grade.special, OperatorType.plus, 0,202),
                new BaseMod(2,3,   "Weapon Fire Rate",         Grade.special, OperatorType.increased, 0,203)
        };

    public static BaseMod[] modExtra =
    new BaseMod[]{
                new BaseMod(75,75,  "Max Fire Rate",               Grade.extra, OperatorType.reduced, 0,301)
    };

    /* public static BaseMod[] modsExterior = 
         new BaseMod[]{           
                 new BaseMod(10,15, "Grenade Throwing Speed",   grade.exterior, operatorType.increased, 0),
                 new BaseMod(10,15, "Grenade Damage",           grade.exterior, operatorType.increased, 0),
                 new BaseMod(5,10,  "Grenade cooldown",         grade.exterior, operatorType.reduced, 0),
                 new BaseMod(5,10,  "Movement Speed",           grade.exterior, operatorType.increased, 0),
                 new BaseMod(2,5,   "Grenade Duration",         grade.exterior, operatorType.increased, 0)
         }; */

    public static int[] InteriorWeight = { 10, 25, 10,10 };

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

