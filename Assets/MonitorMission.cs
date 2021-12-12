using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorMission : ModDataRoom
{
    internal GeneratedMission mission;

    internal MissionSelector selector;

    public MeshRenderer[] meshes;

    internal TMPro.TextMeshPro text;

    private void Start()
    {
        selector = GetComponentInParent<MissionSelector>();
        mission = CreateMission();
        text = GetComponentInChildren<TMPro.TextMeshPro>();
        text.text = mission.text;
        UIUnselect();
    }

    public void UISelect()
    {
        selector.TurnGreen(meshes);
        selector.mission = mission;
    }

    internal void UIUnselect()
    {
        selector.TurnBlue(meshes);
    }

    internal void RefreshMission()
    {
        mission = CreateMission();
        text.text = mission.text;
    }
}
