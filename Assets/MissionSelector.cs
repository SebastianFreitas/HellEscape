using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSelector : ModDataRoom
{
    public PlayerInventory playerInventory;



    public Material green;
    public Material yellow;
    public Material red;
    public Material blue;

    public TMPro.TextMeshPro test;

    private GeneratedMission mission;

    public GameObject portal;

    public GameMan manager;

    void Start()
    {

        mission = CreateMission();
        test.text = mission.text;
    }

    internal void OpenPortal()
    {
        portal.SetActive(true);
    }

    internal void StartSelectedMission()
    {
        manager.StartRun(0, mission);
    }
}
