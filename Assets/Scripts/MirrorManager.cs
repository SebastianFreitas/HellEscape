using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : PlayerAcess
{
    RoomGenerator roomGen;

    [SerializeField] TMPro.TextMeshPro lifeText;
    [SerializeField] TMPro.TextMeshPro boonText;
    [SerializeField] TMPro.TextMeshPro missionChanceText;
    [SerializeField] TMPro.TextMeshPro missionTotalModsText;
    [SerializeField] TMPro.TextMeshPro levelText;

    int life = 0;
    int boon = 0;
    internal float missionChance = 0;
    int level = 0;
    int maxMods = 0;

    [SerializeField] TMPro.TextMeshPro availablePoints;


    [SerializeField] MissionSelector misSelector;

    private void Start()
    {
        roomGen = player.GetComponentInParent<GameMan>().roomGen;
        LoadMirror();
    }

    private void UpdateUI()
    {

        availablePoints.text = $"{currentPoints} Available points";

        lifeText.text = $"+{life} Max life";
        boonText.text = $"+{boon}% Boon drop chance";
        levelText.text = $"+{level} item drop level";
        missionChanceText.text = $"+{missionChance}% chance to locate paths";
        missionTotalModsText.text = $"+{maxMods} maximum path mods";
    }

    internal enum MirrorBoon
    {
        nope,
        MaxLife,
        MissionChance,
        ItemLevel,
        MissionMaxMods,
        BoonChance
    }

    internal (MirrorBoon,int)[] powerList = new (MirrorBoon, int)[] {
        (MirrorBoon.BoonChance, 0),
        (MirrorBoon.MissionChance, 0),
        (MirrorBoon.ItemLevel, 0),
        (MirrorBoon.MaxLife, 0),
        (MirrorBoon.MissionMaxMods, 0)
    };

    private int currentPoints;


    void LoadMirror()
    {
        ResetMirror();

        if (PlayerPrefs.HasKey("PathLevel")) currentPoints = PlayerPrefs.GetInt("PathLevel");
        else currentPoints = 0;

        foreach (var x in powerList)
        {
            if (PlayerPrefs.HasKey(x.Item1.ToString()))
            {
                var value = PlayerPrefs.GetInt(x.Item1.ToString());

                for (int a = 0; a < value; a++)
                {
                    InsertPoint(x.Item1, 1);
                }
            }
        }
        UpdateUI();
    }
    private void OnDestroy()
    {
         foreach (var x in powerList) PlayerPrefs.SetInt(x.Item1.ToString(),  x.Item2);
    }


    internal bool InsertPoint(MirrorBoon type, int choice)
    {
        if (currentPoints >= 0)
        {
            if (currentPoints > 0 && choice > 0)
            {
                currentPoints--;
            }
            else if (choice < 0 && (GetPowerList(type) > 0))
            {
                currentPoints++;
            }
            else return false;
        }
        else
        {
            return false;
        }

        var worked = false;

        switch (type)
        {
            case MirrorBoon.BoonChance:
                
                worked = BoonChance(choice);
                break;

            case MirrorBoon.ItemLevel:

                worked = ItemLevel(choice);
                break;

            case MirrorBoon.MaxLife:

                worked = MaxLife(choice);
                break;

            case MirrorBoon.MissionChance:

                worked = MissionChance(choice);
                break;
            case MirrorBoon.MissionMaxMods:
                worked = MissionMaxMods(choice);
                break;
        }

        if (!worked) currentPoints++;
        UpdateUI();
        return worked;
    }

    private int GetPowerList(MirrorBoon type)
    {
        foreach(var current in powerList)
        {
            if (current.Item1 == type) return current.Item2;
        }

        return -1;
    }


    private bool MissionMaxMods(int choice)
    {
        var worked = false;
        if ((powerList[4].Item2 <= 5 && choice > 0) || (choice < 0))
        {
            misSelector.maxMods += choice;
            maxMods += choice;
            powerList[4].Item2 += choice;
            worked = true;
        }
        return worked;
    }


    private bool MissionChance(int choice)
    {
        var worked = false;
        if ((powerList[1].Item2 <= 20 && choice > 0) || ( choice < 0))
        {
            misSelector.additionalChance +=  3*choice;
            missionChance +=  3*choice;
            powerList[1].Item2 +=3* choice;
            worked = true;
        }
        return worked;
    }

    private bool MaxLife(int choice)
    {

        var worked = false;
        if ((powerList[3].Item2 <= 45 && choice > 0) || (choice < 0))
        {

            playerHP.ChangeMaxHP(5 * choice);
            life += 5 * choice;
            powerList[3].Item2 += 5 * choice;

            worked = true;
        }
        return worked;


    }

    private bool ItemLevel(int choice)
    {

        var worked = false;
        if ((powerList[2].Item2 <= 50 && choice > 0) || (choice < 0))
        {

            playerInv.weaponLevel +=  choice;
            level +=  choice;
            powerList[2].Item2 += choice;
            worked = true;
        }
        return worked;

    }

    private bool BoonChance(int choice)
    {
        var worked = false;
        if ((powerList[0].Item2 <= 100 && choice > 0) || (choice < 0))
        {

            roomGen.mirrorBoonChance += choice;
            boon +=  choice;
            powerList[0].Item2 += choice;
            worked = true;
        }
        return worked;

    }

    private void ResetMirror()
    {
        misSelector.additionalChance = 0;
        missionChance = 0;

        playerHP.SetHPToBase();
        life = 0;

        playerInv.weaponLevel = 10;
        level = 0;

        roomGen.mirrorBoonChance = 0;
        boon = 0;
    }
  
}