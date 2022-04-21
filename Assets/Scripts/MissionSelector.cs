using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class MissionSelector : ModDataRoom
{
    public PlayerInventory playerInventory;


    public AudioSource source;
    public AudioClip wrong;
    public AudioClip poweringUP;
    public AudioClip correct;

    public float volume;
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

    public MeshRenderer[] meshEngagePath;
    public MeshRenderer[] meshSearchPath;
    public MeshRenderer[] meshReloadSearch;
    private bool beenLong = true;

    public AudioClip click;


    [SerializeField] MirrorManager mirror;

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
            AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
            StartCoroutine(ErrorWaiter(meshEngagePath));
        }
    }
    [SerializeField] internal GameObject buttons;
    private void DisableSelector()
    {


        StopCoroutine("SearchPath");
        StopCoroutine("SearchPath");
        foreach (MonitorMission mis in missions)
        {
            mis.gameObject.SetActive(false);
        }
        buttons.SetActive(false);
    }

    internal void EnableSelector()
    {


        buttons.SetActive(true);

        used = false;
        TurnGreen(meshSearchPath);
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
            if (mon.mission != mi && mon.isActiveAndEnabled) mon.UIUnselect();
        }
    }

    internal void ReloadMissions()
    {

        if (beenLong)
        {
            used = false;
            beenLong = false;
            StartCoroutine(SearchPath());
            StartCoroutine(BeenLong());
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
        }


    }

    bool used = false;

    internal float additionalChance = 0;
    internal IEnumerator SearchPath()
    {
        manager.startedRun = true;
        mirror.gameObject.SetActive(false);

        if (!used)
        {
            AudioSource.PlayClipAtPoint(click, transform.position, .1f);
            source.PlayOneShot(poweringUP, .1f);
            used = true;
            TurnRed(meshSearchPath);


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
                    if (Random.Range(1, 100) < 50f + additionalChance)//13
                    {

                        mis.gameObject.SetActive(true);
                        mis.RefreshMission();
                        
                        AudioSource.PlayClipAtPoint(correct, transform.position, 1f);
                        cantFind = false;
                    }
                    else AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
                    totalChance--;
                    yield return new WaitForSecondsRealtime(1f);
                } 
            }
        }
        else AudioSource.PlayClipAtPoint(wrong, transform.position, 1f);
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

}
