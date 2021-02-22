using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModGenerator : MonoBehaviour
{
    
    enum operatorType{
        plus,
        increased,
    }

    enum grade{
        interior,
        exterior,
        special
    }
    struct Mod
    {
        public float upperBound;
        public float lowerBound;
        public string text;
        public grade grade; //inner outter special

        public operatorType op;

        public Mod(float lowerBound, float upperBound, string text, grade type, operatorType op) 
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.text = text;
            this.grade = type;
            this.op = op;
        }
    }

    struct GunMods{

        public float averageDamage;

        public float attackRate;

        public float weaponDamage;
        public float coldDamage;
        public float FireDamage;
        public float PoisonDamage;

        public int maxRicochets;

        public Dictionary<grade,HashSet<Mod>> allModifiers;
    }
 //+4 weapon damage
 //4% increased cold dmg

        static Mod[] mods = 
           new Mod[]{
                 new Mod(4,11,"Weapon  Damage", grade.interior, operatorType.plus),
                 new Mod(1,3,"Bullet Ricochet", grade.interior, operatorType.plus),
                 new Mod(4,11,"Fire Damage", grade.interior, operatorType.plus)
           };


        private int upgradeTier(Mod mod, int level){
                return 0;
        }        
        
        GunMods createWeapon(int maxLevel)
        {
            GunMods ret = new GunMods();
            ret.allModifiers = new Dictionary<grade, HashSet<Mod>>();
            ret.allModifiers[grade.interior] = new HashSet<Mod>();
            ret.allModifiers[grade.exterior] = new HashSet<Mod>();
            ret.allModifiers[grade.special] = new HashSet<Mod>();

            /*for(int i = 0; i<Random.Range(1,8); i++)
            {
                var newMod = AddMod(ret);
                var grad = newMod.grade; 
                ret.allModifiers[grad].Add(newMod);
            } */
                

            return ret;
        }

        /*private Mod AddMod(GunMods gun)
        {

        }*/



                /*{"+# Bullet Ricochet",
        "+# Fire Damage",
        "+# Cold Damage",
        "+# Corruption damage",
        "+#% Weapon  Damage",
        "+#% Headshot Damage",
        "+#% Weapon Fire Rate"};*/



    






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
}
