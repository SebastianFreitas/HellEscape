using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class MissionSelector : ModDataRoom
{



    public AudioSource source;
    public AudioClip wrong;
    public AudioClip poweringUP;
    public AudioClip correct;

    public float volume;
    public Material green;
    public Material blue;

    public Material red;

    internal GeneratedMission mission = null;
    internal int maxMods = 0;

    public GameObject portal;

    private GameMan manager;

    //public GameObject[] monitors;
    public MonitorMission[] missions;

    internal int level = 1;

    public int totalChance { get; private set; }

    public MeshRenderer[] meshEngagePath;

    public TMPro.TextMeshPro reloadPrice;
    public TMPro.TextMeshPro empowerPrice;



    public MeshRenderer[] meshSearchPath;
    public MeshRenderer[] meshReloadSearch;
    private bool beenLong = true;
    public GeneratedMission currentMission;
    public AudioClip click;


    [SerializeField] MirrorManager mirror;
    [SerializeField] internal GameObject openPortal;
    private PlayerInventory playerInv;


    private void Awake()
    {
        playerInv = transform.root.GetComponent<GameMan>().player.GetComponent<PlayerInventory>();
        reloadMissions.SetActive(false);
        empowerMission.SetActive(false);

        manager = transform.root.GetComponent<GameMan>();

        foreach (MonitorMission mis in missions)
        {

            mis.gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        beenLong = true;


    }
    internal void OpenPortal()
    {
        if (mission != null)
        {

            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
            portal.SetActive(true);
            var x = portal.GetComponent<Portal>();
            x.isON = true;
            x.audioSource.PlayOneShot(x.openPortalSound);
            portal.GetComponent<VisualEffect>().enabled = true;
            portal.GetComponent<VisualEffect>().Play();

            DisableSelector();

            

        }
        else
        {
            AudioSource.PlayClipAtPoint(wrong, transform.position,.1f);
            StartCoroutine(ErrorWaiter(meshEngagePath));
        }
    }
    [SerializeField] internal GameObject buttons;
    private void DisableSelector()
    {


        StopCoroutine("SearchPath");
        foreach (MonitorMission mis in missions)
        {
            mis.gameObject.SetActive(false);
        }
        buttons.SetActive(false);
    }

    internal void EnableSelector()
    {

        mission = null;
        buttons.SetActive(true);
        openPortal.SetActive(false);

        used = false;
        TurnGreen(meshSearchPath);

        foreach (MonitorMission mis in missions)
        {

            mis.gameObject.SetActive(false);
        }
    }
    internal bool startedRun = false;
    internal void StartSelectedMission()
    {
            manager.startedRun = true;
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
            //if (mon.mission != mi && mon.isActiveAndEnabled) mon.UIUnselect();
            mon.UIUnselect();
        }
    }


    bool used = false;

    internal float additionalChance = 0;
    [SerializeField] GameObject reloadMissions;
    [SerializeField] GameObject search;
    [SerializeField] internal GameObject empowerMission;
    internal IEnumerator SearchPath()
    {
       // manager.startedRun = true;
        mirror.gameObject.SetActive(false);
        reloadMissions.SetActive(false);
        empowerMission.SetActive(false);
        search.SetActive(false);

        if (!used)
        {
            
            source.PlayOneShot(poweringUP, .1f);
            used = true;
           // TurnRed(meshSearchPath);


            totalChance = missions.Length;
            
            foreach (MonitorMission mis in missions)
            {

                mis.gameObject.SetActive(false);
            }

            yield return new WaitForSecondsRealtime(1f);

            foreach (MonitorMission mis in missions)
            {
                var cantFind = true;
                while (cantFind)
                {
                    if (totalChance <= 0) break;
                    if (Random.Range(1, 100) < 35f + additionalChance)//13
                    {

                        mis.gameObject.SetActive(true);
                        mis.RefreshMission(1 + maxMods);
                        
                        AudioSource.PlayClipAtPoint(correct, transform.position,.1f);
                        cantFind = false;
                    }
                    else AudioSource.PlayClipAtPoint(wrong, transform.position,.1f);
                    totalChance--;
                    yield return new WaitForSecondsRealtime(1f);
                } 
            }
        }
        else AudioSource.PlayClipAtPoint(wrong, transform.position,.1f);

        if (!missions[0].isActiveAndEnabled)
        {
            missions[0].gameObject.SetActive(true);
            missions[0].RefreshMission(1 + maxMods);
            AudioSource.PlayClipAtPoint(correct, transform.position,.1f);
        }

        reloadMissions.SetActive(true);
        reloadPrice.text = GetReloadMissionsPrice() + "";
    }

    internal void StartSearch()
    {
        AudioSource.PlayClipAtPoint(click, transform.position,.1f);
        reloadMissionsCounter = 1;
        search.SetActive(false);
        StartCoroutine("SearchPath");
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
        yield return new WaitForSecondsRealtime(5f);
        beenLong = true;
    }
    internal MonitorMission currentMonitor;
    internal void EmpowerSelected()
    {
        if(playerInv.gunParts >= GetEmpowerPrice())
        {

            playerInv.UpdateGunParts(-GetEmpowerPrice());

            AddMod(currentMonitor.mission);
            CreatePositives(currentMonitor.mission);

            CreateMissionText(currentMonitor.mission);
            mission = currentMonitor.mission;
            currentMonitor.RefreshUI();

            AudioSource.PlayClipAtPoint(click, transform.position, .1f);

            
            RefreshPrices();

        }
        else AudioSource.PlayClipAtPoint(wrong, transform.position, .1f);
    }

    internal void ReloadMissions()
    {
        if (playerInv.gunParts >= GetReloadMissionsPrice())
        {
            playerInv.UpdateGunParts(-GetReloadMissionsPrice());

            used = false;
            StartCoroutine(nameof(SearchPath));
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);

            
            reloadMissionsCounter++;
        }
        else AudioSource.PlayClipAtPoint(wrong, transform.position, .1f);
    }


    private int GetEmpowerPrice()
    {
        return mission.mods.Count * 4;
    }

    private int reloadMissionsCounter = 1;


    private int GetReloadMissionsPrice()
    {
        return 5*reloadMissionsCounter;
    }

    internal void RefreshPrices()
    {
        empowerPrice.text = GetEmpowerPrice() + "";
    }
}
