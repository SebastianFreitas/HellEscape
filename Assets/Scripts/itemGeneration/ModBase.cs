using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModBase : MonoBehaviour
{
    
    public float baseDamage = 50f;
    public float baseFireRate = 2f;
    public int BaseRicochets = 3;
    public enum operatorType{
        plus,
        increased,
        reduced,
    }

    public enum grade{
        interior,
        exterior,
        special
    }
    public struct BaseMod
    {
        public int upperBound;
        public int lowerBound;
        public string text;
        public grade grade; 
        public operatorType op;

        public int tier;

        public BaseMod(int lowerBound, int upperBound, string text, grade type, operatorType op, int tier) 
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.text = text;
            this.grade = type;
            this.op = op;
            this.tier = tier;
        }

        public static bool operator == (BaseMod c1, BaseMod c2) 
        {
            return c1.text.Equals(c2.text);
        }

        public static bool operator != (BaseMod c1, BaseMod c2) 
        {
            return c1.text.Equals(c2.text);
        }
    }

    public struct GunMods{

        public float averageDamage;
        public float fireRate;
        public float weaponDamage;
        public float coldDamage;
        public float fireDamage;
        public float poisonDamage;
        public int maxRicochets;
        public int level;

        public Dictionary<grade,HashSet<BaseMod>> allModifiers;

    }


    public static BaseMod[] modsInterior = 
        new BaseMod[]{
                new BaseMod(4,11,  "Weapon  Damage",           grade.interior, operatorType.plus, 0),
                new BaseMod(1,3,   "Bullet Ricochet",          grade.interior, operatorType.plus, 0),
                new BaseMod(2,3,   "Weapon Fire Rate",         grade.interior, operatorType.increased, 0)
                //new BaseMod(5,10,  "Headshot Damage",          grade.interior, operatorType.increased, 0)
        };

    public static BaseMod[] modsExterior = 
        new BaseMod[]{
                new BaseMod(4,11,  "Weapon  Damage",           grade.interior, operatorType.plus, 0),
                new BaseMod(1,3,   "Bullet Ricochet",          grade.interior, operatorType.plus, 0),
                new BaseMod(2,3,   "Weapon Fire Rate",         grade.interior, operatorType.increased, 0)
        };

    public static BaseMod[] modSpecial = 
        new BaseMod[]{
                new BaseMod(4,11,  "Weapon  Damage",           grade.interior, operatorType.plus, 0),
                new BaseMod(1,3,   "Bullet Ricochet",          grade.interior, operatorType.plus, 0),
                new BaseMod(2,3,   "Weapon Fire Rate",         grade.interior, operatorType.increased, 0)
        };

   /* public static BaseMod[] modsExterior = 
        new BaseMod[]{           
                new BaseMod(10,15, "Grenade Throwing Speed",   grade.exterior, operatorType.increased, 0),
                new BaseMod(10,15, "Grenade Damage",           grade.exterior, operatorType.increased, 0),
                new BaseMod(5,10,  "Grenade cooldown",         grade.exterior, operatorType.reduced, 0),
                new BaseMod(5,10,  "Movement Speed",           grade.exterior, operatorType.increased, 0),
                new BaseMod(2,5,   "Grenade Duration",         grade.exterior, operatorType.increased, 0)
        }; */ 

    public static int[] InteriorWeight ={1,1,1};
    
    public static int[] ExteriorWeight ={1,1,1};  //

    public static int[] SpecialWeight ={1,1,1}; 

       

    //int[] gradeWeights ={300,300,2};//chances of rolling grade type on an empty item
    public static int[] maxModsWeight ={1000,900,700,500,300,100,10,1};   //chances for total mods an item will have when rolled

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
