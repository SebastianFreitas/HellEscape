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
    internal VoidBoon(bool isInfluence, BoonType type)
    {
        influence = type;

        if (isInfluence)
        {
           switch (type)
            {
                case BoonType.Red:

                    GetBoonListTextRed();
                    GetBoonListWeightRed();
                    break;

                case BoonType.Blue:
                    GetBoonListTextBlue();
                    GetBoonListWeightBlue();
                    break;
            }
        }
        else
        {
            GetBoonListWeight();
            GetBoonListText();
        }



        List<BoonName> weightList = GetWeightList(boonListWeight);
        name = weightList[Random.Range(0, weightList.Count)];

        foreach(var current in boonListText)
        {
            if (current.Item2 == name) text = current.Item1;
        }
    }

    private void GetBoonListText()
    {

        boonListText.Add(("Gain 10 Max Health", BoonName.MaxHp1));
        boonListText.Add(("Gain 20 Max Health", BoonName.MaxHp2));
        boonListText.Add(("Gain 30 Max Health", BoonName.MaxHp3));
        //convert all of your damage randomly each shot
        //heal full hp gain 100 max hp at most


    }

    private void GetBoonListTextRed()
    {
        boonListText.Add(("Lose 20 max hp \n deal +10 fire damage", BoonName.Greed));
        //whenever you dash deal aoe fire dmg
        //convert all of your damage to fire damage

    }

    private void GetBoonListTextBlue()
    {
        boonListText.Add(("", BoonName.Greed));
        //cold damage becomes horizontal slices
        //lose fire rate gain damage
        //next time you die come back to life at 50% hp


    }

    private void GetBoonListWeight()
    {
        boonListWeight.Add((100, BoonName.MaxHp1));
        boonListWeight.Add((10, BoonName.MaxHp2));
        boonListWeight.Add((1, BoonName.MaxHp3));
    }

    private void GetBoonListWeightRed()
    {
        boonListWeight.Add((1, BoonName.Greed));

    }

    private void GetBoonListWeightBlue()
    {

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
        Red,
        Normal
    }

    internal enum BoonName
    {
        MaxHp2,
        Greed,
        MaxHp1,
        MaxHp3
    }

    internal PlayerBasicMovement playerMov;
    internal PlayerHpManager playerHP;
    internal PlayerInventory playerInv;
    internal GameObject player;

    internal void AcceptOrRemoveBoon(bool gain)
    {
        switch (name)
        {
            case BoonName.MaxHp2:
                Maxhp1(gain);
                break;

            case BoonName.Greed:
                Greed(gain);
                break;
        }

    }

    internal void Maxhp1(bool gain)
    {
        if (gain)
        {
            playerHP.ChangeMaxHP(10);
            playerInv.listBoons.Add(this);
        } 
        else
        {
            playerHP.ChangeMaxHP(-10);
            playerInv.listBoons.Remove(this);
        }

    }

    //red
    internal void Greed(bool gain)
    {
        if (gain)
        {
            playerHP.ChangeMaxHP(-20);
            playerInv.additionalFireDamage += 10;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerHP.ChangeMaxHP(20);
            playerInv.additionalFireDamage -= 10;
            playerInv.listBoons.Remove(this);
        }
    }
}