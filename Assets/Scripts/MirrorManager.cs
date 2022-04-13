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
    float boon = 0;
    int length = 0;
    int level = 0;

    [SerializeField] TMPro.TextMeshPro availablePoints;

    private void Start()
    {
        roomGen = player.GetComponentInParent<GameMan>().roomGen;
    }

    private void UpdateUI()
    {

        availablePoints.text = $"{maxPoints-usedPoints} Available points";

        lifeText.text = $"+{life} Max life";
        boonText.text = $"+{boon} Boon chance after encounter";
        lengthText.text = $"+{length} run length";
        levelText.text = $"+{level} level of item drops";
    }

    internal enum MirrorBoon
    {
        nope,
        MaxLife,
        RunLength,
        ItemLevel,
        BoonChance
    }

    internal (MirrorBoon,int)[] powerList = new (MirrorBoon, int)[] {
        (MirrorBoon.BoonChance, 0),
        (MirrorBoon.RunLength, 0),
        (MirrorBoon.ItemLevel, 0),
        (MirrorBoon.MaxLife, 0)
    };

    private int maxPoints;
    private int usedPoints = 0;
    private Hub hub;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("PathLevel"))
        {
            maxPoints = PlayerPrefs.GetInt("PathLevel");
        }
        else maxPoints = 0;

        int i = 0;
        foreach(var x in powerList)
        {
            if (PlayerPrefs.HasKey(x.Item1.ToString()))
            {
                var value = PlayerPrefs.GetInt(x.Item1.ToString());
                powerList[i].Item2 += value;
                usedPoints += value;

                for(int a =0; a <value; a++)
                {
                    InsertPoint(x.Item1, 1);
                }

            }
            i++;

            
        }
        hub = GetComponentInParent<Hub>();

        UpdateUI();
    }

    private void OnDisable()
    {
        int i = 0;
        foreach (var x in powerList)
        {
            PlayerPrefs.SetInt(x.Item1.ToString(), x.Item2);
            i++;
        }
    }

    internal void InsertPoint(MirrorBoon type, int choice)
    {


        if (usedPoints < maxPoints && choice > 0) usedPoints++;
        else if (choice < 0) usedPoints--;
        else type = MirrorBoon.nope;



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

            case MirrorBoon.RunLength:
                powerList[1].Item2 += choice;
                RunLength(choice);
                break;
            case MirrorBoon.nope:
                break;
        }
        UpdateUI();
    }

    private void RunLength(int choice)
    {
        roomGen.mirrorLength += 2*choice;
        level += 2 * choice;
        
    }

    private void MaxLife(int choice)
    {      
        playerHP.maxHealth += 5*choice;
        playerHP.health += 5 * choice;
        life += 5 * choice;

    }

    private void ItemLevel(int choice)
    {
        playerInv.weaponLevel += 1 * choice;
        level += 1 * choice;
    }

    private void BoonChance(int choice)
    {
        roomGen.mirrorLength += 1 * choice;
        boon += 1 * choice;
    }

  
}