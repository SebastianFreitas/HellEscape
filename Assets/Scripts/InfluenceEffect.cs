using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InfluenceEffect : PlayerAcess
{

    internal enum BlueEffect
    {
        ColdDamage,
        Chill,
        Invisible,

    }

    internal enum RedEffect
    {
  
        KockBack,
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
            ("Monster melee hits reduce movement speed", BlueEffect.Chill),
            ("Monsters become invisible temporarily", BlueEffect.Invisible),
            ("Monsters gain health and deal more damage", BlueEffect.ColdDamage)
        }; 

    internal List<(string, RedEffect)> redTexts = new List<(string, RedEffect)>
        {
            ("Monsters push you back further", RedEffect.KockBack),
            ("Monsters act faster", RedEffect.FasterMob),
            ("Monsters explode on death and deal extra damage", RedEffect.MobExplodes),       
        };

    internal List<(string, GoodEffect)> goodTexts = new List<(string, GoodEffect)>
        {
            ("+25% elite chance", GoodEffect.MobEliteChance),
            ("Monsters have double Health and drop chance", GoodEffect.MobHPDoubleDrop),
            ("Every monster is Doubled", GoodEffect.MoreMob)
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
    internal InfluenceEffect(VoidBoon.BoonType contact)
    {
        base.Awake();
 
        switch (contact)
        {
            case VoidBoon.BoonType.Blue:
                goodRed = true;
                influenceType = RoomActivator.AreaType.Blue;
                break;

            case VoidBoon.BoonType.Red:
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

        generator.influence = VoidBoon.BoonType.Blue;

        SearchGood();

        SearchBlue();
    }

    internal void PickRed(RoomGenerator roomGen)
    {

        generator = roomGen;
        generator.influence = VoidBoon.BoonType.Red;

        SearchGood();
        SearchRed();
    }

    private void SearchRed()
    {
        switch (redEffect)
        {
            case RedEffect.FasterMob:
                FasterMob();
                break;
            case RedEffect.MobExplodes:
                MobExplodes();
                break;
            case RedEffect.KockBack:
                KockBack();
                break;
        }
    }
    private void SearchBlue()
    {
        switch (blueEffect)
        {
            case BlueEffect.Invisible:
                Invisible();
                break;
            case BlueEffect.ColdDamage:
                ColdDamage();
                break;
            case BlueEffect.Chill:
                Chill();
                break;
        }
    }

    private void SearchGood()
    {

        switch (goodEffect)
        {
            case GoodEffect.MobEliteChance:
                MobEliteChance();
                break;
            case GoodEffect.MobHPDoubleDrop:
                MobHPDoubleDrop();
                break;
            case GoodEffect.MoreMob:
                MoreMob();
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
        generator.mission.doubleMobs = true;
    }
    //Red---------------------------------------------------------------------------------------------------------------
    internal void FasterMob()
    {
        generator.mission.increasedActionSpeed += 50;
    }

    internal void MobExplodes()
    {
        generator.mission.deathExplosion = true;
        generator.mission.aditionalDamage += Random.Range(1, 5);
    }

    internal void KockBack()
    {
        generator.mission.kockBack += 100;
    }

    //Blue---------------------------------------------------------------------------------------------------------------
    private void Chill()
    {
        generator.mission.chill = true;
    }
    private void ColdDamage()
    {
        generator.mission.aditionalDamage += Random.Range(1, 5);
        generator.mission.aditionalLife += 100;
    }
    private void Invisible()
    {
        generator.mission.invisible = true;
    }
} 