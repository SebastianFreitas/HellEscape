using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModGenerator : MonoBehaviour
{
    
    enum operatorType{
        plus,
        increased,
        reduced,
    }

    enum grade{
        interior,
        exterior,
        special
    }
    struct BaseMod
    {
        public float upperBound;
        public float lowerBound;
        public string text;
        public grade grade; 

        public operatorType op;

        public BaseMod(float lowerBound, float upperBound, string text, grade type, operatorType op) 
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.text = text;
            this.grade = type;
            this.op = op;
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

    struct GunMods{

        public float averageDamage;
        public float attackRate;
        public float weaponDamage;
        public float coldDamage;
        public float fireDamage;
        public float poisonDamage;
        public int maxRicochets;

        public Dictionary<grade,HashSet<BaseMod>> allModifiers;
    }


    static BaseMod[] modsInterior = 
        new BaseMod[]{
                new BaseMod(4,11,  "Weapon  Damage",           grade.interior, operatorType.plus),
                new BaseMod(1,3,   "Bullet Ricochet",          grade.interior, operatorType.plus),
                new BaseMod(2,3,   "Weapon Fire Rate",         grade.interior, operatorType.increased),
                new BaseMod(5,10,  "Headshot Damage",          grade.interior, operatorType.increased),
                new BaseMod(4,10,  "Poison Damage",            grade.interior, operatorType.plus),
                new BaseMod(4,10,  "Cold Damage",              grade.interior, operatorType.plus)
        };

    static BaseMod[] modsExterior = 
        new BaseMod[]{           
                new BaseMod(10,15, "Grenade Throwing Speed",   grade.exterior, operatorType.increased),
                new BaseMod(10,15, "Grenade Damage",           grade.exterior, operatorType.increased),
                new BaseMod(5,10,  "Grenade cooldown",         grade.exterior, operatorType.reduced),
                new BaseMod(5,10,  "Movement Speed",           grade.exterior, operatorType.increased),
                new BaseMod(2,5,   "Grenade Duration",         grade.exterior, operatorType.increased)
        };  

    static int[] InteriorWeight ={1,1,1,1,1,1};
    
    static int[] ExteriorWeight ={1,1,1,1,1};  //    

    //int[] gradeWeights ={300,300,2};//chances of rolling grade type on an empty item
    static int[] maxModsWeight ={1000,900,700,500,300,100,10,1};   //chances for total mods an item will have when rolled

    private int upgradeTier(BaseMod mod, int level){
            return 0;
    }        
    
    GunMods createWeapon(int maxLevel)
    {
        GunMods ret = new GunMods();
        ret.allModifiers = new Dictionary<grade, HashSet<BaseMod>>();
        ret.allModifiers[grade.interior] = new HashSet<BaseMod>();
        ret.allModifiers[grade.exterior] = new HashSet<BaseMod>();
        ret.allModifiers[grade.special] = new HashSet<BaseMod>();
        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;
        int nextGrade;
        BaseMod newMod = new BaseMod();  

        for(int i = 0; i<totalMods; i++)
        {
            nextGrade = GenerateGrade(ret);
            switch (nextGrade)
                {
                    case 0:
                    newMod = GenerateInteriorMod(ret);
                        break;
                    case 1:
                    newMod = GenerateExteriorMod(ret);
                        break;
                    case 2:
                    newMod = GenerateInteriorMod(ret);
                        break;
                }
            ret.allModifiers[newMod.grade].Add(newMod);
        } 
        return ret;
    }

    private int GenerateGrade(GunMods gun)
    {
        int[] gradeWeights = {
        300 - (gun.allModifiers[grade.interior].Count * 100),
        300 - (gun.allModifiers[grade.interior].Count * 100),
        2 - gun.allModifiers[grade.interior].Count };

        return GetRandomWeightedIndex(gradeWeights);
        
    }
    
    private BaseMod GenerateInteriorMod(GunMods gun)
    {
        BaseMod ret;
        while(true){
            ret = modsInterior[GetRandomWeightedIndex(InteriorWeight)];
            if(ContainsMod(gun, ret)) break;
        }
        return ret;
    }

    private BaseMod GenerateExteriorMod(GunMods gun)
    {
        BaseMod ret;
        while(true){
            ret = modsExterior[GetRandomWeightedIndex(ExteriorWeight)];
            if(ContainsMod(gun, ret)) break;
        }
        return ret;
    }

    //gets a weapon and a mod and sees if the gun doesnt have the mod
    private bool ContainsMod(GunMods gun, BaseMod modifier)
    {
        HashSet<BaseMod> gunModsByGrade = gun.allModifiers[modifier.grade]; 
        foreach (BaseMod mod in gunModsByGrade)
        {
            if (mod == modifier) return false;
        }
            return true;
        
    }




    






    void Start()
    {
        //weaponDamage.Add(int.Parse(input1[0]), input1 ); 
        //weaponDamage.Add(int.Parse(input2[0]), input2 ); 
        //weaponDamage.Add(int.Parse(input3[0]), input3 ); 

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

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
