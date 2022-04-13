using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    private int maxPoints;

    internal enum MirrorBoon
    {
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
            if (PlayerPrefs.HasKey(x.Item1.ToString())) powerList[i].Item2 = PlayerPrefs.GetInt(x.Item1.ToString());
            i++;
        }
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
        switch (type)
        {
            case MirrorBoon.BoonChance:
                BoonChance(choice);
                break;

            case MirrorBoon.ItemLevel:
                ItemLevel(choice);
                break;

            case MirrorBoon.MaxLife:
                MaxLife(choice);
                break;

            case MirrorBoon.RunLength:
                RunLength(choice);
                break;
        }   

    }

    private void RunLength(int choice)
    {
        throw new System.NotImplementedException();
    }

    private void MaxLife(int choice)
    {
        throw new System.NotImplementedException();
    }

    private void ItemLevel(int choice)
    {
        throw new System.NotImplementedException();
    }

    private void BoonChance(int choice)
    {
        throw new System.NotImplementedException();
    }
}