using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorMission : ModDataRoom
{
    internal GeneratedMission mission;

    internal MissionSelector selector;

    public MeshRenderer[] meshesGood;
    public MeshRenderer[] meshesBad;

    public TMPro.TextMeshPro textGood;
    public TMPro.TextMeshPro textBad;

    private void Start()
    {
        selector = GetComponentInParent<MissionSelector>();

        UIUnselect();
    }

    public void UISelect()
    {
        selector.TurnGreen(meshesGood);

        selector.mission = mission;
    }

    internal void UIUnselect()
    {
        selector.TurnBlue(meshesGood);
    }

  

    internal void RefreshMission(int maxMods)
    {
        mission = CreateMission(maxMods);
        textGood.text = mission.goodText;
        textBad.text = mission.badText;
    }
}
