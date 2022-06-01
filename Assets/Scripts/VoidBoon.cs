using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class VoidBoon
{
    internal BoonType influence;
    internal BoonName name;

    internal string text;

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
        FireMovement,
        InstantPoison,
        FireDeath,
        StrongerDash,
        ExtraPoisonToFire,
        TwiceFastPoison,
        VampiricPoison,
        PoisonExplosions,
        PoisonDuration,
        PoisonedCold,
        WeakerPoison,
        PoisonFollow,
        ChanceFreeze1,
        ChanceFreeze2,
        PoisonedChill,
        LongerFreeze,
        FrozenDamage,
        MoreFrozenGunparts,
        ColdShatter,
        ColdShatteringRicochet,
        AddedColdProjectile,
        PhyisToColdCrit,
        MorePhys10,
        MorePhys25,
        MorePhys50,
        DoubleDamagePhys,
        TripleCritPhys,
        FireToPhys50,
        FireToPhys100,
        ReducedSpeedMorePhys,
        MovementToPhys,
        DoublePhysCold,
        CritFireRate
    }
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

        BoonList.Add((1,      "Fire damage explodes in a 90% smaller area\n Fire damage is doubled", BoonName.DoubleFire));
        BoonList.Add((1,      "Delayed Explosion", BoonName.DelayedFire));
        BoonList.Add((5,      "Explosions from fire damage gain more push back force", BoonName.PushForceFire));
        BoonList.Add((5,      "Explosions from fire damage pull instead of pushing", BoonName.PullFire));
        BoonList.Add((5,      "Whenever you are pushed back from your own fire explosions gain more movement speed for a duration", BoonName.FireMovement));
        BoonList.Add((1,      "Gain 25% of poison damage as fire damage", BoonName.ExtraPoisonToFire));
        BoonList.Add((1,      "Your dash and double jump are stronger", BoonName.StrongerDash));
        BoonList.Add((1,      "Enemies explode on death into a fire explosion", BoonName.FireDeath));
        //POISON----------------------------------------------------------------

        BoonList.Add((1,      "Ricochets bounce into close poisoned enemies", BoonName.PoisonFollow));
        BoonList.Add((1,      "Poisoned enemies deal 25% less damage", BoonName.WeakerPoison));
        BoonList.Add((1,      "Chilled enemies take 30% more poison damage", BoonName.PoisonedCold));
        BoonList.Add((1,      "Enemies who die from your poison have +5% chance of dropping healing packs", BoonName.VampiricPoison));
        BoonList.Add((1,      "Deal all poison damage instantly \n Deal half poison damage", BoonName.InstantPoison));
        BoonList.Add((1,      "Fire explosions also deal poison damage", BoonName.PoisonExplosions));
        //COLD------------------------------------------------------------------

        BoonList.Add((1,      "Freeze condition lasts one second longer", BoonName.LongerFreeze));
        BoonList.Add((1,      "Physical damage is converted to cold on critical hits.", BoonName.PhyisToColdCrit));
        BoonList.Add((1,      "Deal 50% more damage agaisnt frozen targets", BoonName.FrozenDamage));
        BoonList.Add((1,      "Enemies killed while chilled or frozen drop more gunparts", BoonName.MoreFrozenGunparts));
        BoonList.Add((1,      "Shoot an additional cold projectile", BoonName.AddedColdProjectile));
        BoonList.Add((1,      "Ricochets shatter into extra cold projectiles", BoonName.ColdShatteringRicochet));
        BoonList.Add((1,      "Enemies that die from cold damage shatter into cold projectiles", BoonName.ColdShatter));
        BoonList.Add((1,      "Poisoned enemies are more affected by chill", BoonName.PoisonedChill));
        //PHYSICAL------------------------------------------------------------------
        BoonList.Add((1,      "Gain +10 physical damage", BoonName.MorePhys10));
        BoonList.Add((1,      "Gain +25 physical damage", BoonName.MorePhys25));
        BoonList.Add((1,      "Gain +50 physical damage", BoonName.MorePhys50));
      
        BoonList.Add((1,      "All physical damage dealt agaisnt frozen enemies is doubled", BoonName.DoublePhysCold));
        BoonList.Add((1,      "Critical hits deal 3 times the physical damage", BoonName.TripleCritPhys));
        BoonList.Add((1,      "Convert 50% of fire damage into physical damage", BoonName.FireToPhys50));
        BoonList.Add((1,      "Convert 100% of fire damage into physical damage", BoonName.FireToPhys100));
        BoonList.Add((1,      "50% reduced bullet speed \n Gain + 50 physical damage", BoonName.ReducedSpeedMorePhys));
        BoonList.Add((5,      "Sources of increased movement speed also apply to physical damage", BoonName.MovementToPhys));
        //NOTDONE ----------NOTDONE----------------NOTDONE-------------NOTDONE-----------NOTDONE-----------
        //NOTDONE ----------NOTDONE----------------NOTDONE-------------NOTDONE-----------NOTDONE-----------

        //--------
        BoonList.Add((10, "Fire damage explodes in a 20% larger area", BoonName.IncreasedFireArea));
        BoonList.Add((5, "Fire damage explodes in a 50% larger area", BoonName.IncreasedFireArea2));
        BoonList.Add((1, "Fire damage explodes in a 100% larger area", BoonName.IncreasedFireArea3));

        //--------

        BoonList.Add((1, "Poison deals damage twice as fast", BoonName.TwiceFastPoison));
        BoonList.Add((1, "Poison lasts 5 more seconds", BoonName.PoisonDuration));

        BoonList.Add((1, "+1% Chance your cold damage freezes the enemy", BoonName.ChanceFreeze1));
        BoonList.Add((1, "+2% Chance your cold damage freezes the enemy", BoonName.ChanceFreeze2));


        BoonList.Add((1, "+5% for physical damage to deal double damage", BoonName.DoubleDamagePhys));




        //  BoonList.Add((1, "Critical hits increase weapon fire rate", BoonName.CritFireRate));


        //AddBoon(1, "Explosions from fire damage shoot bullets", BoonName.ScrapFire);


        Debug.Log("This is the total boons -> "+BoonList.Count);


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



    internal PlayerBasicMovement playerMov;
    internal PlayerHpManager playerHP;
    internal PlayerInventory playerInv;
    internal GameObject player;

    internal void AcceptOrRemoveBoon(bool gain)
    {
        switch (name)
        {
            //LIFE---------------------------LIFE---------------------LIFE----------------
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

            //SPEED---------------------SPEED-----------------------SPEED--------------------
            case BoonName.Speed:
                Speed(gain);
                break;
            case BoonName.RicochectStack:
                RicochectStack(gain);
                break;
            case BoonName.ShootSpeed:
                RicochectStack(gain);
                break;

            //FIRE---------------------FIRE--------------------------FIRE-----------------
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
            case BoonName.FireMovement:
                FireMovement(gain);
                break;

            case BoonName.FireDeath:
                FireDeath(gain);
                break;
            case BoonName.StrongerDash:
                StrongerDash(gain);
                break;
            case BoonName.ExtraPoisonToFire:
                ExtraPoisonToFire(gain);
                break;

            //POISON------------------------POISON----------------------------------------
            case BoonName.InstantPoison:
                InstantPoison(gain);
                break;

            case BoonName.TwiceFastPoison:
                TwiceFastPoison(gain);
                break;

            case BoonName.VampiricPoison:
                VampiricPoison(gain);
                break;
            case BoonName.PoisonExplosions:
                PoisonExplosions(gain);
                break;
            case BoonName.PoisonDuration:
                PoisonDuration(gain);
                break;

            case BoonName.PoisonedCold:
                PoisonedCold(gain);
                break;
            case BoonName.WeakerPoison:
                WeakerPoison(gain);
                break;
            case BoonName.PoisonFollow:
                PoisonFollow(gain);
                break;
            //COLD------------------------COLD----------------------------------------
            case BoonName.ColdShatteringRicochet:
                ColdShatteringRicochet(gain);
                break;

            case BoonName.PhyisToColdCrit:
                PhyisToColdCrit(gain);
                break;

            case BoonName.AddedColdProjectile:
                AddedColdProjectile(gain);
                break;
            case BoonName.ColdShatter:
                ColdShatter(gain);
                break;
            case BoonName.MoreFrozenGunparts:
                MoreFrozenGunparts(gain);
                break;

            case BoonName.FrozenDamage:
                FrozenDamage(gain);
                break;
            case BoonName.LongerFreeze:
                LongerFreeze(gain);
                break;
            case BoonName.PoisonedChill:
                PoisonedChill(gain);
                break;

            case BoonName.ChanceFreeze2:
                ChanceFreeze(gain, 2);
                break;

            case BoonName.ChanceFreeze1:
                ChanceFreeze(gain, 1);
                break;
            //PHYSICAL------------------------PHYSICAL----------------------------------------
            case BoonName.MorePhys10:
                MorePhys(gain,10);
                break;

            case BoonName.MorePhys25:
                MorePhys(gain, 25);
                break;

            case BoonName.MorePhys50:
                MorePhys(gain, 50);
                break;

            case BoonName.DoubleDamagePhys:
                DoubleDamagePhys(gain);
                break;

            case BoonName.TripleCritPhys:
                TripleCritPhys(gain);
                break;

            case BoonName.FireToPhys50:
                FireToPhys(gain, .5f);
                break;
            case BoonName.FireToPhys100:
                FireToPhys(gain, 1f);
                break;
            case BoonName.ReducedSpeedMorePhys:
                ReducedSpeedMorePhys(gain);
                break;

            case BoonName.MovementToPhys:
                MovementToPhys(gain);
                break;

            case BoonName.DoublePhysCold:
                DoublePhysCold(gain);
                break;
        }

    }
















    //DONE------------------------------DONE---------------------------------
    //DONE------------------------------DONE---------------------------------
    //DONE------------------------------DONE---------------------------------
    //DONE------------------------------DONE---------------------------------
    internal void ColdShatteringRicochet(bool gain)
    {
        if (gain)
        {

            playerInv.coldShatterBounces += 1;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.coldShatterBounces -= 1;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void AddedColdProjectile(bool gain)
    {
        if (gain)
        {

            playerInv.additionalColdProj += 1;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.additionalColdProj -= 1;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void ColdShatter(bool gain)
    {
        if (gain)
        {

            playerInv.coldShatter += 1;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.coldShatter -= 1;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void PoisonedChill(bool gain)
    {
        if (gain)
        {

            playerInv.poisonedChill += .25f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.poisonedChill -= .25f;
            playerInv.listBoons.Remove(this);
        }
    }

    internal void PoisonExplosions(bool gain)
    {
        if (gain)
        {

            playerInv.fireToPoisonExplosions += 1;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.fireToPoisonExplosions -= 1;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void InstantPoison(bool gain)
    {
        if (gain)
        {

            playerInv.instantPoison = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.instantPoison = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void FireDeath(bool gain)
    {
        if (gain)
        {

            playerInv.fireDeath = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.fireDeath = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void MovementToPhys(bool gain)
    {
        if (gain)
        {

            playerInv.movementToPhys += 1f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.movementToPhys -= 1f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void ReducedSpeedMorePhys(bool gain)
    {
        if (gain)
        {

            playerInv.additionalPhysicalDamage += 50;
            playerInv.increasedBulletSpeed -= 50;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.additionalPhysicalDamage -= 50;
            playerInv.increasedBulletSpeed += 50;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void MoreFrozenGunparts(bool gain)
    {
        if (gain)
        {

            playerInv.moreFrozenGunparts = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.moreFrozenGunparts = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void FrozenDamage(bool gain)
    {
        if (gain)
        {

            playerInv.extraDamageWhileFrozen += .5f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.extraDamageWhileFrozen -= .5f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void ExtraPoisonToFire(bool gain)
    {
        if (gain)
        {

            playerInv.poisonToFixeAsExtra += .25f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.poisonToFixeAsExtra -= .25f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void FireToPhys(bool gain, float value)
    {
        if (gain)
        {

            playerInv.fireToPhys += (int)value;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.fireToPhys -= (int)value;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void TripleCritPhys(bool gain)
    {
        if (gain)
        {

            playerInv.critGlobalMultiplier = 3f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.critGlobalMultiplier = 2f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void DoubleDamagePhys(bool gain)
    {
        if (gain)
        {

            playerInv.chancePhysDoubleDamage += 5;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.chancePhysDoubleDamage -= 5;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void PhyisToColdCrit(bool gain)
    {
        if (gain)
        {

            playerInv.phyisToColdCrit = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.phyisToColdCrit = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void DoublePhysCold(bool gain)
    {
        if (gain)
        {

            playerInv.doublePhysOnFreezes = 2f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.doublePhysOnFreezes = 1f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void LongerFreeze(bool gain)
    {
        if (gain)
        {

            playerInv.additionalFreezeDuration += 1f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.additionalFreezeDuration -= 1f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void ChanceFreeze(bool gain, float value)
    {
        if (gain)
        {

            playerInv.freezeChance += (int)value;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.freezeChance -= (int)value;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void MorePhys(bool gain, float value)
    {
        if (gain)
        {

            playerInv.additionalPhysicalDamage += (int)value;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.additionalPhysicalDamage -= (int)value;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void VampiricPoison(bool gain)
    {
        if (gain)
        {

            playerInv.vampiricBonus = 5;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.vampiricBonus = 0;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void PoisonedCold(bool gain)
    {
        if (gain)
        {

            playerInv.chillPoison = 1.3f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.chillPoison = 1f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void WeakerPoison(bool gain)
    {
        if (gain)
        {

            playerInv.weakerPoison = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.weakerPoison = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void PoisonFollow(bool gain)
    {
        if (gain)
        {

            playerInv.poisonFollow = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.poisonFollow = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void PoisonDuration(bool gain)
    {
        if (gain)
        {

            playerInv.poisonDuration = 10f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.poisonDuration = 0f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void TwiceFastPoison(bool gain)
    {
        if (gain)
        {

            playerInv.poisonSpeedDouble = 2f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.poisonSpeedDouble = 1f;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void FireMovement(bool gain)
    {
        if (gain)
        {

            playerInv.fireMovement = true;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerInv.fireMovement = false;
            playerInv.listBoons.Remove(this);
        }
    }
    internal void StrongerDash(bool gain)
    {
        if (gain)
        {

            playerMov.strongerDash = 2f;
            playerInv.listBoons.Add(this);
        }
        else
        {
            playerMov.strongerDash = 1f;
            playerInv.listBoons.Remove(this);
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