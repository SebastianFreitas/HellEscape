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

    public static int[] maxModsWeight = { 0, 10, 20 ,10 ,4 ,1};
    public static int[] modsWeight = { 80,50,2,50,20,6,5,20,20,100,100,100};


    public static Mod[] mods =
        new Mod[]{

                new Mod(1,5,   "Area Level",                Grade.interior, OperatorType.plus,    80,4),
                new Mod(1,25,   "Mission Length",            Grade.interior, OperatorType.plus,    10,5),
                new Mod(1,5,   "??1?11",                    Grade.interior, OperatorType.non,     2,6), //bloodline, mobs that are not skulls have a chance of spawning 2 skulls
                new Mod(1,5,   "Double Trash",              Grade.interior, OperatorType.non,     50,7), //Double the trash
                new Mod(1,5,   "Explosive Trash",           Grade.interior, OperatorType.non,     20,8), //Trash has a chance to become explosive 
                new Mod(1,5,   "33!3!!",                    Grade.interior, OperatorType.non,     6,9), //Trash can instead spawn as skulls
                new Mod(1,1,   "High Entity",               Grade.interior, OperatorType.non,     10,10), //Boss Entity + 3 length
                new Mod(1,5,   "Dangerous Entities",        Grade.interior, OperatorType.non,     20,11), //duo or 5 elite fight.
                new Mod(1,5,   "WorkBench",                 Grade.interior, OperatorType.non,     20,12), //Find a workbench, half prices.

                new Mod(15,30, "Monster health",            Grade.interior, OperatorType.plus,          100,1),
                new Mod(1,3,   "Monster Damage",            Grade.interior, OperatorType.plus,          100,2),
                new Mod(1,3,   "Monster Speed",             Grade.interior, OperatorType.increased,     100,3)
               

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

    public int[] GetMaxModsWeight()
    {
        int[] ret = new int[mods.Length];
        int i = 0;
        foreach(Mod mod in mods)
        {
            ret[i] = mod.tier;
            i++;
        }
        return ret;
    }

    private void CreatePositives(GeneratedMission result)
    {
        var bonus = result.mods.Count ;
        result.increasedChanceElite         += bonus + Random.Range(0, bonus);
        result.increasedChanceSpecialRooms  += bonus + Random.Range(0, bonus); 
        result.increasedMonsterDrops        += bonus + Random.Range(0, bonus);
        result.distance = Random.Range(3, 7) + result.aditionalLength;
    }
    private void CreateMissionText(GeneratedMission result)
    {
        result.goodText = GetMissionGood(result);
        result.badText = GetMissionBad(result);
        result.text = result.badText + result.goodText;
    }
    internal string GetMissionGood(GeneratedMission result)
    {
        result.createPositives();
        string text = "";
        text += result.distance + " Units Located" + "\n";
        text += result.increasedChanceElite + 5+ "% elite chance" + "\n";
        text += result.increasedChanceSpecialRooms+ 25 + "% special room chance" + "\n";
        text += (float)(1 + (((float)result.increasedMonsterDrops * 3) / 100)) + "% weapon drop chance" + "\n\n";
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
            //var i = mods[GetRandomWeightedIndex(GetMaxModsWeight())];
            //var x = new Mod(i);
            if (!ContainsMod(mission, x))
            {
              //  i.tier -= 100;
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
            case OperatorType.non:
                ret =  mod.text;
                break;
        }
        return ret;
    }

    private void ModToStat(GeneratedMission mission, Mod x, int value)
    {
        switch (x.basicText)
        {
            case "Monster health":
                mission.aditionalLife           = value;
                break;

            case "Monster Damage":
                mission.aditionalDamage         = value;
                break;                           
                                                 
            case "Monster Speed":         
                mission.increasedActionSpeed    = value;
                break;                           
                                                 
            case "Area Level":                   
                mission.aditionalAreaLevel      = value;
                break;                            
                                                  
            case "Mission Length":                
                mission.aditionalLength         = value;
                break;   

            case "??1?11"://bloodline
                mission.bloodline = true;
                break;

            case "Double Trash":
                mission.doubleTrash = true;
                break;

            case "Explosive Trash":
                mission.explosiveTrash = true;
                break;

            case "33!3!!":
                mission.trashToSkulls = true;
                break;

            case "High Entity":
                mission.boss = true;
                break;
            case "Dangerous Entities":
                mission.miniBoss = true;
                break;
            case "WorkBench":
                mission.workbench = true;
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
        public bool workbench       = false;
        public bool miniBoss        = false;
        public bool boss            = false;
        public bool trashToSkulls   = false;
        public bool explosiveTrash  = false;
        public bool doubleTrash     = false;
        public bool bloodline       = false;

        public int aditionalLife           = 0;
        public int aditionalDamage         = 0;
        public int increasedActionSpeed    = 0;
        public int aditionalAreaLevel      = 0;
        public int aditionalLength         = 0;


        public int increasedMonsterDrops       = 0;
        public int increasedChanceSpecialRooms = 0;
        public int increasedChanceElite        = 0;
        public int distance = 0;

        public int level = 0;

        public HashSet<Mod> mods = new HashSet<Mod>();
        public string text      = "";
        public string badText   = "";
        public string goodText  = "";

        public void createPositives()
        {
            var result = mods.Count * 2 +Random.Range(1,mods.Count*2);
            increasedMonsterDrops       = result;
            increasedChanceElite        = mods.Count + Random.Range(1, mods.Count);
            increasedChanceSpecialRooms = mods.Count * 2 + Random.Range(1, mods.Count * 4);

        }
    }
}
