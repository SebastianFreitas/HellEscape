using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : PlayerAcess
{
    RoomGenerator roomGen;

    [SerializeField] TMPro.TextMeshPro lifeText;
    [SerializeField] TMPro.TextMeshPro boonText;
    [SerializeField] TMPro.TextMeshPro lengthText;
    [SerializeField] TMPro.TextMeshPro levelText;

    int life = 0;
    int boon = 0;
    internal float missionChance = 0;
    int level = 0;

    [SerializeField] TMPro.TextMeshPro availablePoints;


    [SerializeField] MissionSelector misSelector;

    private void Start()
    {
        roomGen = player.GetComponentInParent<GameMan>().roomGen;
    }

    private void UpdateUI()
    {

        availablePoints.text = $"{currentPoints} Available points";

        lifeText.text = $"+{life} Max life";
        boonText.text = $"+{boon}% Boon drop chance";
        lengthText.text = $"+{missionChance}% chance to locate paths";
        levelText.text = $"+{level} item drop level";
    }

    internal enum MirrorBoon
    {
        nope,
        MaxLife,
        MissionChance,
        ItemLevel,
        BoonChance
    }

    internal (MirrorBoon,int)[] powerList = new (MirrorBoon, int)[] {
        (MirrorBoon.BoonChance, 0),
        (MirrorBoon.MissionChance, 0),
        (MirrorBoon.ItemLevel, 0),
        (MirrorBoon.MaxLife, 0)
    };

    private int maxPoints;
    private int currentPoints;

    private void OnEnable()
    {
        if (transform.root.GetComponent<GameMan>().startedRun)
        {

            transform.root.GetComponent<GameMan>().startedRun = false;
            int b = 0;
            foreach (var x in powerList)
            {
                PlayerPrefs.SetInt(x.Item1.ToString(), x.Item2);
                b++;

                for (int a = 0; a < x.Item2; a++)
                {
                    InsertPoint(x.Item1, -1);
                }
            }
        }

        powerList = new (MirrorBoon, int)[] {
        (MirrorBoon.BoonChance, 0),
        (MirrorBoon.MissionChance, 0),
        (MirrorBoon.ItemLevel, 0),
        (MirrorBoon.MaxLife, 0)
        };

        roomGen = player.GetComponentInParent<GameMan>().roomGen;

        if (PlayerPrefs.HasKey("PathLevel"))
        {
            maxPoints = PlayerPrefs.GetInt("PathLevel");
 
        }
        else maxPoints = 0;

        int i = 0;

       
        currentPoints = maxPoints;// - usedPoints;
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
            i++;
        }
        UpdateUI();
    }

    private void OnDisable()
    {
        if (transform.root.GetComponent<GameMan>().startedRun) return;
        
        int i = 0;
        foreach (var x in powerList)
        {
            PlayerPrefs.SetInt(x.Item1.ToString(),  x.Item2);
            i++;

            for (int a = 0; a < x.Item2; a++)
            {
                InsertPoint(x.Item1, -1);
            }
        }
        

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


        switch (type)
        {
            case MirrorBoon.BoonChance:
                powerList[0].Item2 += choice;
                BoonChance(choice);
                break;

            case MirrorBoon.ItemLevel:
                powerList[2].Item2 += choice;
                ItemLevel(choice);
                break;

            case MirrorBoon.MaxLife:
                powerList[3].Item2 += choice;
                MaxLife(choice);
                break;

            case MirrorBoon.MissionChance:
                powerList[1].Item2 += choice;
                MissionChance(choice);
                break;
            case MirrorBoon.nope:
                break;
        }
        UpdateUI();
        return true;
    }

    private int GetPowerList(MirrorBoon type)
    {
        foreach(var current in powerList)
        {
            if (current.Item1 == type) return current.Item2;
        }

        return -1;
    }

    private void MissionChance(int choice)
    {
        misSelector.additionalChance += 1 * choice;
        missionChance += 1 * choice;   
    }

    private void MaxLife(int choice)
    {      

        playerHP.ChangeMaxHP(5 * choice);
        life += 5 * choice;

    }

    private void ItemLevel(int choice)
    {
        playerInv.weaponLevel += 1 * choice;
        roomGen.weaponLevel += 1 * choice;
        level += 1 * choice;
    }

    private void BoonChance(int choice)
    {
        roomGen.mirrorBoonChance += 1 * choice;
        boon += 1 * choice;
    }

  
}