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

                new Mod(1,5,   "Weapon level",              Grade.interior, OperatorType.plus,                       100,     4),
                //new Mod(1,25,  "Mission length",            Grade.interior, OperatorType.plus,                       100,    5),
            
                new Mod(1,1,   "Double drop chance",        Grade.interior, OperatorType.non,                        25,     7),
                new Mod(1,5,   "Monsters potentially found per room",       Grade.interior, OperatorType.plus,       100,    8),
                new Mod(1,5,   "All side rooms are special",Grade.interior, OperatorType.non,                        25,     9),
                new Mod(1,15,  "Chance for encounters to drop health",        Grade.interior, OperatorType.non,     100,    10),
                new Mod(1,5,   "Danger",                    Grade.interior, OperatorType.non,                        4,     11),
                new Mod(10,50, "Movement speed",    Grade.interior, OperatorType.reduced,                    100,    12),
                new Mod(2,4,   "Located boons",         Grade.interior, OperatorType.plus,                           50,    13),
                new Mod(1,5,   "ERROR",         Grade.interior, OperatorType.non,                                    4,     14),
                new Mod(1,5,   "Located additional rewards",         Grade.interior, OperatorType.non,               10,    15),
                new Mod(1,5,   "Gun parts drops",         Grade.interior, OperatorType.plus,                         100,     16),
                new Mod(1,5,   "50% reduced healing",       Grade.interior, OperatorType.non,                        100,     21),
                new Mod(15,100, "Monster health",            Grade.interior, OperatorType.plus,                      250,    1),
                new Mod(1,10,  "Monster damage",            Grade.interior, OperatorType.plus,                       250,    2),
                new Mod(1,25,  "Monster action speed",             Grade.interior, OperatorType.increased,           250,    3),
                
                new Mod(1,5,   "Cannot use grenade",         Grade.interior, OperatorType.non,                       100,      17),
                new Mod(1,5,   "UnknownX",         Grade.interior, OperatorType.non,                                 8,      18),//monsters become bigger deal double damage and drop 1 more gunpart
                new Mod(1,5,   "UnknownY",         Grade.interior, OperatorType.non,                                 8,      19),
                new Mod(1,5,   "Monsters are immune to fire damage",         Grade.interior, OperatorType.non,       100,      22),
                new Mod(1,5,   "Monsters are immune to cold damage",         Grade.interior, OperatorType.non,       100,      20),
                new Mod(1,5,   "Monsters are immune to poison damage",         Grade.interior, OperatorType.non,     100,      23),
                new Mod(1,5,   "Monsters are immune to physical damage",         Grade.interior, OperatorType.non,   100,      24),
                

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

    private List<Mod> Yep()
    {
        var let = new List<(Mod, int)>();
        foreach (var i in mods)
        {
            let.Add((i, i.tier));
        }

        var ret = new List<Mod>();
        foreach(var i in let)
        {
            for(int a = 0; a < i.Item2; a++)
            {
                ret.Add(i.Item1);
            }
        }
        return ret;
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
        result.createPositives();
        string text = "";
        //text += result.additionalBoons + " Boons Located" + "\n";
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

    private void AddMod(GeneratedMission mission)
    {
        var list = Yep();
        while (true)
        {
            var x = new Mod(list[Random.Range(0, list.Count)]);
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

            case "Monster action speed":
                mission.increasedActionSpeed    = value;
                break;

            case "Weapon level":
                mission.additionalWeaponLevel   = value;
                break;

            case "Mission length":
                mission.aditionalLength         = value;
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

            case "Movement speed":
                mission.playerReducedMovementSpeed += value;
                break;

            case "Located boons":
                mission.additionalBoons += value;
                break;

            case "ERROR":
                mission.error = true;
                break;

            case "Located additional rewards":
                mission.additionalRewards = true;
                break;

            case "Gun parts drops":
                mission.additionalGunParts += value;
                break;

            case "50% reduced healing":
                mission.halfHealing = true;
                break;

            case "Cannot use grenade":
                mission.noGrenade = false;
                break;

            case "Monsters are immune to fire damage":
                mission.fireImmunity = true;
                break;

            case "Monsters are immune to cold damage":
                mission.coldImmunity = true;
                break;

            case "Monsters are immune to poison damage":
                mission.poisonImmunity = true;
                break;

            case "Monsters are immune to physical damage":
                mission.physicalImmunity = true;
                break;

            case "UnknownX":
                mission.additionalGunParts +=1;
                mission.isBig = true;
                break;

            case "UnknownY":
                mission.additionalGunParts +=5;
                mission.increasedActionSpeed += 100;
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
        public float increasedActionSpeed    = 0;
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
        internal int additionalBoons = 1;
        internal bool error = false;
        internal bool additionalRewards = false;
        internal int additionalGunParts = 0;
        internal bool halfHealing = false;
        internal bool fireImmunity = false;
        internal bool coldImmunity = false;
        internal bool poisonImmunity = false;
        internal bool physicalImmunity = false;
        internal bool noGrenade = true;
        internal bool isBig = false;
        internal bool chill;
        internal bool invisible;
        internal int kockBack;
        internal float monsterHealthDropChance;
        internal float monsterWeaponDropChance;

        public void createPositives()
        {

            if (everythingSpecial) increasedChanceSpecialRooms = 75;
            else increasedChanceSpecialRooms = mods.Count * 2 + Random.Range(1, mods.Count * 4) ;

            increasedMonsterDrops       = mods.Count * 2 + Random.Range(1, mods.Count * 2) * dropModifier;
            increasedChanceElite        = mods.Count + Random.Range(1, mods.Count);


        }
    }
}
