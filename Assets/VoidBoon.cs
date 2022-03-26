using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class VoidBoon
{
    internal BoonType influence;
    internal BoonName name;

    internal string text;

    internal List<(int, BoonName)> boonListWeight = new List<(int, BoonName)>();

    internal List<(string, BoonName)> boonListText = new List<(string, BoonName)>();
    internal VoidBoon()
    {
        GetBoonListWeight();

        GetBoonListText();

        List<BoonName> weightList = GetWeightList(boonListWeight);
        name = weightList[Random.Range(0, weightList.Count)];

        foreach(var current in boonListText)
        {
            if (current.Item2 == name) text = current.Item1;
        }
    }

    private void GetBoonListText()
    {
        boonListWeight.Add((100, BoonName.Greed));
        boonListWeight.Add((100, BoonName.MaxHp1));
    }

    private void GetBoonListWeight()
    {
        boonListText.Add(("Lose 50% of you max hp \n deal +10 fire damage", BoonName.Greed));
        boonListText.Add(("Gain 10 Max Health", BoonName.MaxHp1));
    }

    private List<BoonName> GetWeightList(List<(int, BoonName)> boonList)
    {
        List<BoonName> weightList = new List<BoonName>();
        foreach(var current in boonList)
        {
            for (int i = 0; i < current.Item1; i++) weightList.Add(current.Item2);
        }
        return weightList;
    }

    internal enum BoonType
    {
        Blue,
        Red
    }

    internal enum BoonName
    {
        MaxHp1,
        Greed
    }

    internal PlayerBasicMovement playerMov;
    internal PlayerHpManager playerHP;
    internal PlayerInventory playerInv;
    internal GameObject player;

    internal void AcceptBoon()
    {
        switch (name)
        {
            case BoonName.MaxHp1:
                Maxhp1();
                break;

            case BoonName.Greed:
                Greed();
                break;
        }

    }

    internal void Maxhp1()
    {
        text = "Gain 10 Max health";
        playerHP.ChangeMaxHP(10);
    }

    //red
    internal void Greed()
    {
        text = "Lose 50% of you max hp \n deal +10 fire damage";
        //
        //
    }
}