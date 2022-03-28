using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InfluenceEffect : PlayerAcess
{

    internal enum BlueEffect
    {
        ColdDamage,
        EnemyWaits,
        Traps,
        SlowPlayer,
    }

    internal enum RedEffect
    {
  
        Tick,
        FasterMob,
        MobExplodes,

    }

    internal enum GoodEffect
    {
        MoreMob,
        MobHPDoubleDrop,
        MobEliteChance

    }


    internal List<(string, BlueEffect)> blueTexts = new List<(string, BlueEffect)>
        {
            ("Trap damage reduces max hp", BlueEffect.Traps),
            ("You have half movement speed", BlueEffect.SlowPlayer),
            ("Enemies are sometimes slower and become more powerful", BlueEffect.EnemyWaits)

        }; 
    internal List<(string, RedEffect)> redTexts = new List<(string, RedEffect)>
        {
            ("Monsters jitter uncontrollably", RedEffect.Tick),
            ("Monster act faster", RedEffect.FasterMob),
            ("Monster explode on death", RedEffect.MobExplodes),       
        };

    internal List<(string, GoodEffect)> goodTexts = new List<(string, GoodEffect)>
        {
            ("+25% base elite chance", GoodEffect.MobEliteChance),
            ("Enemies have double Health and drop chance", GoodEffect.MobHPDoubleDrop),
            ("Every enemy is Doubled", GoodEffect.MoreMob)

        };


    internal string blueBadText;
    internal string redBadText;
    internal string blueGoodText = "";
    internal string redGoodText = "";

    internal BlueEffect blueEffect;
    internal RedEffect redEffect;
    internal GoodEffect goodEffect;

    private bool goodBlue = false;
    private bool goodRed = false;

    internal RoomActivator.AreaType influenceType;
    internal InfluenceEffect(RoomActivator.AreaType contact)
    {


        switch (contact)
        {
            case RoomActivator.AreaType.Blue:
                goodRed = true;
                influenceType = RoomActivator.AreaType.Blue;
                break;

            case RoomActivator.AreaType.Red:
                goodBlue = true;
                influenceType = RoomActivator.AreaType.Red;
                break;
        }

        CreateBadRed();
        CreateBadBlue();
        CreateGood();


    }
    RoomGenerator generator;
    internal void PickBlue(RoomGenerator roomGen)
    {
        generator = roomGen;

        SearchGood();

        SearchBlue();
    }

    internal void PickRed(RoomGenerator roomGen)
    {
        generator = roomGen;

        SearchGood();
        SearchRed();
    }

    private void SearchRed()
    {
        switch (redEffect)
        {
            case RedEffect.FasterMob:
                break;
            case RedEffect.MobExplodes:
                break;
            case RedEffect.Tick:
                break;
        }
    }
    private void SearchBlue()
    {
        switch (blueEffect)
        {
            case BlueEffect.ColdDamage:
                break;
            case BlueEffect.EnemyWaits:
                break;
            case BlueEffect.Traps:
                break;
        }
    }

    private void SearchGood()
    {

        switch (goodEffect)
        {
            case GoodEffect.MobEliteChance:
                break;
            case GoodEffect.MobHPDoubleDrop:
                break;
            case GoodEffect.MoreMob:
                break;
        }
    }

    private void CreateBadRed()
    {
        RedEffect a = (RedEffect)Random.Range(0, System.Enum.GetValues(typeof(RedEffect)).Length);
        redEffect = a;
        foreach (var current in redTexts)
        {
            if (current.Item2 == a) redBadText = current.Item1;
        }


    }

    private void CreateBadBlue()
    {
        BlueEffect a = (BlueEffect)Random.Range(0, System.Enum.GetValues(typeof(BlueEffect)).Length);
        blueEffect = a;
        foreach (var current in blueTexts)
        {
            if (current.Item2 == a) blueBadText = current.Item1;
        }
    }

    private void CreateGood()
    {

        List<GoodEffect> x = new List<GoodEffect>();

        GoodEffect type = (GoodEffect)Random.Range(0, System.Enum.GetValues(typeof(GoodEffect)).Length);
        foreach (var current in goodTexts)
        {
            if (current.Item2 == type && !x.Contains(type)) 
            {
                if (goodBlue)
                {
                    blueGoodText = current.Item1;
                    goodEffect = type;
                    x.Add(current.Item2);
                    break;
                }
                else if (goodRed)
                {
                    redGoodText = current.Item1;
                    goodEffect = type;
                    break;
                }
            }

        }
        

    }

    internal void MobEliteChance()
    {
        generator.mission.increasedChanceElite += 25;
    }

    internal void MobHPDoubleDrop()
    {
        generator.mission.doubleLife = true;
        generator.mission.doubleDrops = true;
    }

    internal void MoreMob()
    {
        generator.mission.encounterMobCount *= 2;
    }
    ///////////Red
    internal void FasterMob()
    {
        generator.mission.increasedActionSpeed += 40;
    }

    internal void MobExplodes()
    {
        generator.mission.deathExplosion = true;
    }

    internal void Tick()
    {
        generator.mission.tick = true;
    }

} 