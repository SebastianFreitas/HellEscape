using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModDataRoom : MonoBehaviour
{
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

    public static int[] maxModsWeight = { 0, 90, 90, 80, 55, 20, 10, 5, 1 };
    public static int[] modsWeight = { 10, 10, 10, 2, 2};


    public static Mod[] mods =
        new Mod[]{
                new Mod(15,30, "Monster Max health",        Grade.interior, OperatorType.plus,          0,1),
                new Mod(1,3,   "Monster Damage",            Grade.interior, OperatorType.plus,          0,2),
                new Mod(1,3,   "Monster Action Speed",      Grade.interior, OperatorType.increased,     0,3),
                new Mod(1,5,   "Area Level",                Grade.interior, OperatorType.plus,          0,4),
                new Mod(1,5,   "Mission Length",            Grade.interior, OperatorType.increased,     0,5)
               

        };


    //increased monster item drop rate
    //increased monster 

    public GeneratedMission CreateMission()
    {
        GeneratedMission result = new GeneratedMission();
        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;


        for(int i = 0; i<totalMods; i++) AddMod(result);

        CreatePositives(result);

        CreateMissionText(result);
        

        return result;
    }

    private void CreatePositives(GeneratedMission result)
    {
        var bonus = result.mods.Count ;
        result.increasedChanceElite         += bonus + Random.Range(0, bonus);
        result.increasedChanceSpecialRooms  += bonus + Random.Range(0, bonus); 
        result.increasedMonsterDrops        += bonus + Random.Range(0, bonus);
    }

    private void CreateMissionText(GeneratedMission result)
    {
        result.goodText = GetMissionGood(result);
        result.badText = GetMissionBad(result);
        result.text = result.badText + result.goodText;
    }
    internal string GetMissionGood(GeneratedMission result)
    {
        string text = "";
        text += result.increasedChanceElite + "% elite chance" + "\n";
        text += result.increasedChanceSpecialRooms + "% to find special rooms" + "\n";
        text += result.increasedMonsterDrops + "% more weapon drop chance" + "\n\n";
        return text;
    }

    internal string GetMissionBad(GeneratedMission result)
    {
        string text = "";
        if (result.mods != null)
        {
            foreach (Mod mod in result.mods)
            {
                text += mod.text + "\n";
            }
        }
        return text;
    }

    private void AddMod(GeneratedMission mission)
    {
        while (true)
        {
            var x = new Mod(mods[GetRandomWeightedIndex(modsWeight)]);
            if (!ContainsMod(mission, x))
            {
                
                var value = Random.Range(x.lowerBound, x.upperBound);
                x.upperBound = value;
                x.text = CreateText(x);
                mission.mods.Add(x);
                ModToStat(mission, x, value);
                break;
            }
        }

    }

    private string CreateText(Mod mod)
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

            case OperatorType.minus:
                ret = mod.upperBound + " " + mod.text;
                break;
        }
        return ret;
    }

    private void ModToStat(GeneratedMission mission, Mod x, int value)
    {
        switch (x.basicText)
        {
            case "Monster Max health":
                mission.aditionalLife           = value;
                break;

            case "Monster Damage":
                mission.aditionalDamage         = value;
                break;                           
                                                 
            case "Monster Action Speed":         
                mission.increasedActionSpeed    = value;
                break;                           
                                                 
            case "Area Level":                   
                mission.aditionalAreaLevel      = value;
                break;                            
                                                  
            case "Mission Length":                
                mission.aditionalLength         = value;
                break;                            

        }
    }

    private bool ContainsMod(GeneratedMission mission, Mod mod)
    {
        if (mission.mods.Count == 0) return false;

        foreach (Mod x in mission.mods)
        {
            if (x.id == mod.id) return true;
        }

        return false;

    }

    public class GeneratedMission
    {
       public int aditionalLife           = 0;
       public int aditionalDamage         = 0;
       public int increasedActionSpeed    = 0;
       public int aditionalAreaLevel      = 0;
       public int aditionalLength         = 0;


       public int increasedMonsterDrops       = 0;
       public int increasedChanceSpecialRooms = 0;
       public int increasedChanceElite        = 0;

        public int level = 0;

        public HashSet<Mod> mods = new HashSet<Mod>();
        public string       text = "";
        public string badText = "";
        public string goodText = "";

        public void createPositives()
        {
            var result = mods.Count * 2;
            increasedMonsterDrops       = result;
            increasedChanceElite        = result;
            increasedChanceSpecialRooms = result;

        }
    }
}
