using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModGenerator : ModBase
{       
    public GunMods createWeapon(int maxLevel)
    {
        GunMods ret = new GunMods();
        ret.allModifiers = new Dictionary<grade, HashSet<BaseMod>>();
        ret.allModifiers[grade.interior] = new HashSet<BaseMod>();
        ret.allModifiers[grade.exterior] = new HashSet<BaseMod>();
        ret.allModifiers[grade.special]  = new HashSet<BaseMod>();

        ret.fireRate        = baseDamage;
        ret.weaponDamage    = baseFireRate;
        ret.maxRicochets    = BaseRicochets;
        ret.level           = maxLevel;

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
        finishWeapon(ret);
        return ret;
    }

    private void finishWeapon(GunMods gun)
    {

        foreach (var byGrade in gun.allModifiers)
        {
            foreach (BaseMod mod in byGrade.Value)
            {
                UpdateMod(mod, gun.level);
                if (mod.grade == grade.interior) GenerateInteriorStats(gun, mod);
                else if (mod.grade == grade.exterior) GenerateInteriorStats(gun, mod);
                else GenerateInteriorStats(gun, mod);
                CreateText(mod);
                
            }
        }

    }


    //Creates tier and text
    private void UpdateMod(BaseMod mod, int level)
    {
        int diference = mod.upperBound-mod.lowerBound;
        mod.tier = 1 + GetRandomWeightedIndex(Enumerable.Range(0,level).Select(x => x * 10).Reverse().ToArray());
        diference = diference*mod.tier;
        mod.upperBound+= diference;
        mod.lowerBound+= diference;
        mod.upperBound = Random.Range(mod.lowerBound, mod.upperBound); 
    }

    public void GenerateInteriorStats(GunMods gun, BaseMod mod)
    {
        switch (mod.text)
        {
            case "Weapon Damage":
            gun.weaponDamage+= mod.upperBound;
                break;

            case "Bullet Ricochet":
            gun.maxRicochets+= mod.upperBound;
                break;

            case "Weapon Fire Rate":
            gun.fireRate = gun.fireRate* (1+(mod.upperBound/100));
                break;
        }
    }

    public void GenerateExteriorStats(GunMods gun, BaseMod mod)
    {

    }

    public void GenerateSpecialStats(GunMods gun, BaseMod mod)
    {

    }

    private int GenerateGrade(GunMods gun)
    {
        int[] gradeWeights = {
        300 - (gun.allModifiers[grade.interior].Count * 100),
        300 - (gun.allModifiers[grade.exterior].Count * 100),
        2 - gun.allModifiers[grade.special].Count };

        return GetRandomWeightedIndex(gradeWeights);   
    }
    
    private BaseMod GenerateInteriorMod(GunMods gun)
    {
        BaseMod ret;
        while(true){
            ret = modsInterior[GetRandomWeightedIndex(InteriorWeight)];
            //if(!ContainsMod(gun, ret)) break;
            break;
        }
        return ret;
    }

    private BaseMod GenerateExteriorMod(GunMods gun)
    {
        BaseMod ret;
        while(true){
            ret = modsExterior[GetRandomWeightedIndex(ExteriorWeight)];
            //if(!ContainsMod(gun, ret)) 
            break;
        }
        return ret;
    }

    //gets a weapon and a mod and sees if the gun doesnt have the mod
    private bool ContainsMod(GunMods gun, BaseMod modifier)
    {
        HashSet<BaseMod> gunModsByGrade = gun.allModifiers[modifier.grade]; 
        foreach (BaseMod mod in gunModsByGrade)
        {
            if (mod == modifier) return true;
        }
        return false;
        
    }

    public void CreateText(BaseMod mod)
    {
        switch (mod.op)
            {
                case operatorType.plus:
                mod.text = "+"+ mod.upperBound +" "+mod.text;
                    break;

                case operatorType.increased:
                mod.text = mod.upperBound +"% increased "+mod.text;
                    break;

                case operatorType.reduced:
                mod.text = mod.upperBound +"% reduced "+mod.text;
                    break;
            }
    }

    public string GenerateText(GunMods gun)
    {
        string text = "";
        foreach (var byGrade in gun.allModifiers)
        {
            foreach (BaseMod mod in byGrade.Value)
            {
                text+= string.Join(mod.text, "\n");
            }
        }
        return text;
    }
}
