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

    public static int[] maxModsWeight = { 100, 90, 80, 70, 25, 10, 5, 2, 1};


    public static Mod[] mods =
        new Mod[]{
                new Mod(15,30, "Monster Max health",      Grade.interior, OperatorType.plus,      0,1),
                new Mod(1,3,   "Monster Damage",            Grade.interior, OperatorType.plus,      0,2),
                new Mod(1,3,   "Monster Action Speed",      Grade.interior, OperatorType.increased,      0,3),
                new Mod(1,5,   "Area Level",        Grade.interior, OperatorType.plus, 0,4),
                new Mod(1,5,   "Mission Length",    Grade.interior, OperatorType.increased, 0,5),
                new Mod(1,5,   "Movement Speed",    Grade.interior, OperatorType.reduced, 0,6)

        };


    //increased monster item drop rate
    //increased monster 

    public GeneratedMission CreateMission()
    {
        GeneratedMission result = new GeneratedMission();
        int totalMods = GetRandomWeightedIndex(maxModsWeight) + 1;


        for(int i = 0; i<totalMods; i++)
        {
            AddMod(result);
        }

        return result;
    }

    private void AddMod(GeneratedMission result)
    {
        throw new System.NotImplementedException();
    }

    public class GeneratedMission
    {
        int aditionalLife           = 0;
        int aditionalDamage         = 0;
        int increasedActionSpeed    = 0;
        int aditionalAreaLevel      = 0;
        int aditionalLength         = 0;

        int increasedMonsterDrops       = 0;
        int increasedChanceSpecialRooms = 0;
        int increasedChanceElite        = 0;

        public HashSet<Mod> mods;
        public string       text;

        public void createPositives()
        {
            var result = mods.Count * 2;
            increasedMonsterDrops       = result;
            increasedChanceElite        = result;
            increasedChanceSpecialRooms = result;

        }
    }
}
