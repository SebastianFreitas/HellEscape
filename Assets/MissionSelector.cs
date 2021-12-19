using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelector : ModDataRoom
{
    public PlayerInventory playerInventory;



    public Material green;
    public Material blue;

    

    internal GeneratedMission mission;

    public GameObject portal;

    public GameMan manager;

    public GameObject[] monitors;
    public MonitorMission[] missions;
    public GeneratedMission currentMission;


    internal void OpenPortal()
    {
        portal.SetActive(true);
    }

    internal void StartSelectedMission()
    {
        if (mission != null) manager.StartRun(0, mission);
    }

    internal void TurnBlue(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = blue;
    }

    public void TurnGreen(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = green;
    }

    internal void UIUnselect(GeneratedMission mi)
    {
        foreach(MonitorMission mon in missions)
        {
            if (mon.mission != mi) mon.UIUnselect();
        }
    }

}
