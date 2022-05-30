using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class VoidBoon
{
    internal BoonType influence;
    internal BoonName name;

    internal string text;


    internal List<(int, string, BoonName)> BoonList = new List<(int, string, BoonName)>();


    private bool gotBoons = false;
    private int weight;
    internal VoidBoon()
    {
        player = GameObject.FindGameObjectsWithTag("Dude")[0];
        playerHP = player.GetComponent<PlayerHpManager>();
        playerInv = player.GetComponent<PlayerInventory>();
        playerMov = player.GetComponent<PlayerBasicMovement>();

        if (!gotBoons)
        {
            gotBoons = true;
            GetBoons();
        }


        List<BoonName> weightList = GetWeightList();
        name = weightList[Random.Range(0, weightList.Count)];

        RemoveBoon();

    }

    private void RemoveBoon()
    {
        foreach (var current in BoonList)
        {
            if (current.Item3 == name)
            {
                text = current.Item2;
                weight = current.Item1;
            }

        }



        BoonList.Remove((weight,text, name));
       
    }

    internal void RefreshBoon()
    {

        List<BoonName> weightList = GetWeightList();
        name = weightList[Random.Range(0, weightList.Count)];

        RemoveBoon();

    }

    private void GetBoons()
    {
        //LIFE----------------------------------------------------------------
        BoonList.Add((10,     "Gain 10 Max Health",       BoonName.MaxHp1));
        BoonList.Add((5,      "Gain 20 Max Health",       BoonName.MaxHp2));
        BoonList.Add((1,      "Gain 60 Max Health",        BoonName.MaxHp3));
        BoonList.Add((2,      "Gain 40 Max Health and heal fully",            BoonName.MaxHpHeal));
        BoonList.Add((5,      "Gain 10 Max Health and 10 physical damage",    BoonName.MaxHPPhys));
        //SPEED----------------------------------------------------------------
        BoonList.Add((10,     "Gain 20% more movement speed and shot speed",  BoonName.Speed));
        BoonList.Add((10,     "Ricochets get increasingly stronger",          BoonName.RicochectStack));
        BoonList.Add((11,     "Gain 100% shot speed",                         BoonName.ShootSpeed));
        //FIRE----------------------------------------------------------------
        BoonList.Add((10,     "Lose 20 max hp \n Deal +10 fire damage", BoonName.FireGreed));
        BoonList.Add((5,      "Fire damage does not destroy the bullet on impact", BoonName.RicochetExplosive));
        BoonList.Add((10,     "Fire damage explodes in a 20% larger area", BoonName.IncreasedFireArea));
        BoonList.Add((5,      "Fire damage explodes in a 50% larger area", BoonName.IncreasedFireArea2));
        BoonList.Add((1,      "Fire damage explodes in a 100% larger area", BoonName.IncreasedFireArea3));
        BoonList.Add((1,      "Fire damage explodes in a 90% smaller area\n Fire damage is doubled", BoonName.DoubleFire));
        BoonList.Add((1, "Delayed Explosion", BoonName.DelayedFire));
        BoonList.Add((1, "Explosions from fire damage gain more push back force", BoonName.PushForceFire));
        //POISON----------------------------------------------------------------
        //COLD------------------------------------------------------------------

        //not done -------------------------------------------------------------

        //AddBoon(1, "Explosions from fire damage shoot bullets", BoonName.ScrapFire);

        BoonList.Add((11111, "Explosions from fire damage pull instead of pushing", BoonName.PullFire));
        BoonList.Add((1, "Whenever you are pushed back from your own fire explosions gain more movement speed for a duration", BoonName.FireMovement));


        //for (int i = 0; i < boonListWeight.Count; i++)
        //{
        //    if (PlayerHasBoon(boonListWeight[i].Item2))
        //    {
        //        boonListWeight.Remove(boonListWeight[i]);
        //        boonListText.Remove(boonListText[i]);
        //    }
        //}
    }

    private bool PlayerHasBoon(BoonName name)
    {
        foreach (var item in playerInv.listBoons)
        {
            if (item.name == name) return true;
        }

        return false;
    }

    private List<BoonName> GetWeightList()
    {
        List<BoonName> weightList = new List<BoonName>();
        foreach(var current in BoonList)
        {
            for (int i = 0; i < current.Item1; i++) weightList.Add(current.Item3);
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
        FireGreed,
        MaxHp1,
        MaxHp3,
        Speed,
        RicochetExplosive,
        IncreasedFireArea,
        IncreasedFireArea2,
        IncreasedFireArea3,
        MaxHpHeal,
        MaxHPPhys,
        RicochectStack,
        ShootSpeed,
        DoubleFire,
        DelayedFire,
        ScrapFire,
        PushForceFire,
        PullFire,
        FireMovement
    }

    internal PlayerBasicMovement playerMov;
    internal PlayerHpManager playerHP;
    internal PlayerInventory playerInv;
    internal GameObject player;

    internal void AcceptOrRemoveBoon(bool gain)
    {
        switch (name)
        {
            //LIFE----------------------------------------------------------------
            case BoonName.MaxHp1:
                Maxhp1(gain, 10);
                break;
            case BoonName.MaxHp2:
                Maxhp1(gain, 40);
                break;
            case BoonName.MaxHp3:
                Maxhp1(gain, 60);
                break;
            case BoonName.MaxHpHeal:
                MaxHpHeal(gain);
                break;
            case BoonName.MaxHPPhys:
                MaxHPPhys(gain);
                break;

            //SPEED----------------------------------------------------------------
            case BoonName.Speed:
                Speed(gain);
                break;
            case BoonName.RicochectStack:
                RicochectStack(gain);
                break;
            case BoonName.ShootSpeed:
                RicochectStack(gain);
                break;

            //FIRE----------------------------------------------------------------
            case BoonName.FireGreed:
                FireGreed(gain);
                break;
            case BoonName.RicochetExplosive:
                RicochetExplosive(gain);
                break;
            case BoonName.IncreasedFireArea:
                IncreasedFireArea(gain, 20);
                break;
            case BoonName.IncreasedFireArea2:
                IncreasedFireArea(gain, 50);
                break;
            case BoonName.IncreasedFireArea3:
                IncreasedFireArea(gain, 100);
                break;
            case BoonName.DoubleFire:
                DoubleFire(gain);
                break;

            case BoonName.DelayedFire:
                DelayedFire(gain);
                break;

            case BoonName.PushForceFire:
                PushForceFire(gain);
                break;
            case BoonName.PullFire:
                PullFire(gain);
                break;
        }

    }
    internal void DelayedFire(bool gain)
    {
        if (gain)
        {

            playerInv.delayedFire = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.delayedFire = false;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void PullFire(bool gain)
    {
        if (gain)
        {

            playerInv.pullfire = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.pullfire = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void PushForceFire(bool gain)
    {
        if (gain)
        {

            playerInv.pushForceModifier += 1;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.pushForceModifier -= 1;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void Maxhp1(bool gain, int value)
    {
        if (gain)
        {
            playerHP.ChangeMaxHP(value);
            playerInv.listBoons.Add(this);
        } 
        else
        {
            playerHP.ChangeMaxHP(-value);
            playerInv.listBoons.Remove(this);
        }

    }

    internal void MaxHpHeal(bool gain)
    {
        if (gain)
        {
            playerHP.ChangeMaxHP(40);
            playerHP.HealForMax();
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerHP.ChangeMaxHP(-40);
            playerInv.listBoons.Remove(this);
        }

    }

    internal void MaxHPPhys(bool gain)
    {
        if (gain)
        {
            playerHP.ChangeMaxHP(10);
            playerInv.additionalPhysicalDamage += 10;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerHP.ChangeMaxHP(-10);
            playerInv.additionalPhysicalDamage -= 10;
            playerInv.listBoons.Remove(this);
        }

    }

    internal void FireGreed(bool gain)
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

    internal void Speed(bool gain)
    {
        if (gain)
        {

            playerInv.increasedBulletSpeed += 20;
            playerInv.increasedMovementSpeed += 20;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.increasedBulletSpeed -= 20;
            playerInv.increasedMovementSpeed -= 20;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void DoubleFire(bool gain)
    {
        if (gain)
        {
            playerInv.fireMultiplier = 2;
            playerInv.increasedFireArea += -90;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.fireMultiplier = 1;
            playerInv.increasedFireArea += 90;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void ShootSpeed(bool gain)
    {
        if (gain)
        {

            playerInv.increasedBulletSpeed += 100;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.increasedBulletSpeed -= 100;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void RicochectStack(bool gain)
    {
        if (gain)
        {

            playerInv.RicochetStack = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.RicochetStack = false;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void RicochetExplosive(bool gain)
    {
        if (gain)
        {

            playerInv.explosiveRicochet = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.explosiveRicochet = false;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void IncreasedFireArea(bool gain, float value)
    {
        if (gain)
        {

            playerInv.increasedFireArea += value;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.increasedFireArea -= value;
            playerInv.listBoons.Remove(this);
        }
    }
}