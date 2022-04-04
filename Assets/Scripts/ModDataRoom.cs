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

    public static int[] maxModsWeight = { 0, 10, 20 ,10 ,4 ,2, 1};
    public static int[] modsWeight = { 80,50,2,50,20,6,5,20,20,100,100,100};


    public static Mod[] mods =
        new Mod[]{

                new Mod(1,5,   "Weapon Level",              Grade.interior, OperatorType.plus,                       5,     4),
                new Mod(1,25,  "Mission Length",            Grade.interior, OperatorType.plus,                       10,    5),
                new Mod(1,5,   "??1?11",                    Grade.interior, OperatorType.non,                        1,     6), //bloodline, mobs that are not skulls have a chance of spawning 2 skulls
                new Mod(1,1,   "Double drop chance",        Grade.interior, OperatorType.non,                        2,     7),
                new Mod(1,5,   "Monsters potentially found per room",       Grade.interior, OperatorType.plus,       20,    8),
                new Mod(1,5,   "All side rooms are special",Grade.interior, OperatorType.non,                        2,     9),
                new Mod(1,15,  "Chance for encounters to drop health",        Grade.interior, OperatorType.plus,     10,    10),
                new Mod(1,5,   "Danger",                    Grade.interior, OperatorType.non,                        1,     11),
                new Mod(10,50, "Reduced movement speed",    Grade.interior, OperatorType.reduced,                    50,    12),
                new Mod(2,4,   "Located Boons",         Grade.interior, OperatorType.plus,                           20,    13),
                new Mod(1,5,   "ERROR",         Grade.interior, OperatorType.non,                                    1,     14),
                new Mod(1,5,   "Located Additional Rewards",         Grade.interior, OperatorType.non,               30,    15),
                new Mod(1,5,   "Gun parts drops",         Grade.interior, OperatorType.plus,                         1,     16),
                new Mod(1,5,   "50% reduced healing",       Grade.interior, OperatorType.non,                        1,     21),
                new Mod(15,30, "Monster health",            Grade.interior, OperatorType.plus,                       100,    1),
                new Mod(1,10,  "Monster damage",            Grade.interior, OperatorType.plus,                       100,    2),
                new Mod(1,10,  "Monster speed",             Grade.interior, OperatorType.increased,                  100,    3),
                
                new Mod(1,5,   "Cannot use grenade",         Grade.interior, OperatorType.non,           1,17),
                new Mod(1,5,   "50% increased healing",         Grade.interior, OperatorType.non,           1,18),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,19),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,20),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,22),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,23),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,24),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,25),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,26),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,27),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,28),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,29),
                new Mod(1,5,   "Danger",         Grade.interior, OperatorType.non,           1,30),

        };



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
        text += (float)(1 + (((float)result.increasedMonsterDrops * 3) / 100)) + "% weapon drop chance" + "\n\n";//(float)(1 + (((float)result.increasedMonsterDrops * 3) / 100)) + "% weapon drop chance" + "\n\n";
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

    private int[] GenerateModsWheight()
    {
        int[] result = new int[mods.Length];
        int i = 0;
        foreach(Mod current in mods)
        {
            result[i] = current.tier;
            i++;
        }

        return result;
    }

    private void AddMod(GeneratedMission mission)
    {
        while (true)
        {
            var x = new Mod(mods[GetRandomWeightedIndex(GenerateModsWheight())]);
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

            case "Monster damage":
                mission.aditionalDamage         = value;
                break;                           
                                                 
            case "Monster speed":         
                mission.increasedActionSpeed    = value;
                break;                           
                                                 
            case "Weapon Level":                   
                mission.additionalWeaponLevel   = value;
                break;                            
                                                  
            case "Mission Length":                
                mission.aditionalLength         = value;
                break;   

            case "??1?11"://bloodline
                mission.bloodline = true;
                break;

            case "Double drop chance":
                mission.dropModifier += value;
                break;

            case "Monsters found potentially per room":
                mission.encounterMobCount += value;
                break;

            case "All side rooms are special":
                mission.everythingSpecial = true;
                break;

            case "Chance for encounters to drop health":
                mission.healthChanceEncounter += value;
                break;

            case "Danger":
                mission.encounterMobCount += 10;
                mission.increasedActionSpeed += 50;
                break;

            case "Reduced movement speed":
                mission.playerReducedMovementSpeed += value;
                break;

            case "Located Boons":
                mission.additionalBoons += value;
                break;

            case "ERROR":
                mission.error = true;
                break;

            case "Located Additional Rewards":
                mission.additionalRewards = true;
                break;

            case "Gun parts drops":
                mission.additionalGunParts += value;
                break;

            case "50% reduced healing":
                mission.halfHealing = true;
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
        public bool bloodline       = false;

        public int aditionalLife           = 0;
        public int aditionalDamage         = 0;
        public int increasedActionSpeed    = 0;
        public int additionalWeaponLevel   = 0;
        public int aditionalLength         = 0;
        public int distance = 0;

        internal int dropModifier = 1;

        public int increasedMonsterDrops       = 0;
        public int increasedChanceSpecialRooms = 0;
        public int increasedChanceElite        = 0;

        public int encounterMobCount = 4;

        public int level = 0;

        public HashSet<Mod> mods = new HashSet<Mod>();
        public string text      = "";
        public string badText   = "";
        public string goodText  = "";
        internal bool doubleLife = false;
        internal bool doubleDrops = false;
        internal bool deathExplosion = false;
        internal bool tick = false;
        internal bool doubleMobs  = false;
        internal bool everythingSpecial = false;
        internal int healthChanceEncounter = 0;
        internal int playerReducedMovementSpeed = 0;
        internal int additionalBoons = 0;
        internal bool error = false;
        internal bool additionalRewards = false;
        internal int additionalGunParts = 0;
        internal bool halfHealing = false;

        public void createPositives()
        {
  
            if (everythingSpecial) increasedChanceSpecialRooms = 75;
            else increasedChanceSpecialRooms = mods.Count * 2 + Random.Range(1, mods.Count * 4) ;

            increasedMonsterDrops       = mods.Count * 2 + Random.Range(1, mods.Count * 2) * dropModifier;
            increasedChanceElite        = mods.Count + Random.Range(1, mods.Count);
     

        }
    }
}
