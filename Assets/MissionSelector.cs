using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MissionSelector : ModDataRoom
{
    public PlayerInventory playerInventory;



    public Material green;
    public Material blue;

    public Material red;

    internal GeneratedMission mission = null;

    public GameObject portal;

    public GameMan manager;

    //public GameObject[] monitors;
    public MonitorMission[] missions;
    public GeneratedMission currentMission;

    internal int level = 1;

    public int totalChance { get; private set; }

    private int additionalChance;
    public MeshRenderer[] meshEngagePath;
    public MeshRenderer[] meshSearchPath;
    public MeshRenderer[] meshReloadSearch;
    private bool beenLong = true;


    private void OnEnable()
    {
        beenLong = true;
    }
    internal void OpenPortal()
    {
        if (mission != null) portal.SetActive(true);
        else StartCoroutine(ErrorWaiter(meshEngagePath));
    }

    internal void StartSelectedMission()
    {
         manager.StartRun(mission);
    }

    internal void TurnBlue(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = blue;
    }

    public void TurnGreen(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = green;
    }

    public void TurnRed(MeshRenderer[] materials)
    {
        foreach (var x in materials) x.material = red;
    }

    internal void UIUnselect(GeneratedMission mi)
    {
        foreach(MonitorMission mon in missions)
        {
            if (mon.mission != mi) mon.UIUnselect();
        }
    }

    internal void ReloadMissions()
    {
        if (beenLong)
        {
            used = false;
            beenLong = false;
            StartCoroutine( SearchPath());
            StartCoroutine(BeenLong());
        }


    }

    bool used = false;
    internal IEnumerator SearchPath()
    {
        if (!used)
        {
            used = true;
            TurnRed(meshSearchPath);
            totalChance = 3;
            foreach (MonitorMission mis in missions)
            {
                mis.gameObject.SetActive(false);
            }
            foreach (MonitorMission mis in missions)
            {
                if (totalChance > 0)
                {
                    mis.gameObject.SetActive(true);
                    mis.RefreshMission();
                }

                totalChance--;
                yield return new WaitForSecondsRealtime(Random.Range(1f, 3f));

            }
        }

        
    }

    IEnumerator ErrorWaiter(MeshRenderer[] materials)
    {
        TurnRed(materials);
        yield return new WaitForSecondsRealtime(.4f);
        TurnGreen(materials);
    }

    IEnumerator BeenLong()
    {
        beenLong = false;
        yield return new WaitForSecondsRealtime(10f);
        beenLong = true;
    }

}
