using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class InfluenceEffect
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



}