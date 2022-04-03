using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Doorway[] doorways;

    internal int roomType;
    internal Transform[] propSpawns;
    internal GameObject[] objects;
 
    internal GameObject Explosive;


    internal GameObject[] skulls;
    internal GameObject portal;

    internal GameObject skull;
    internal bool locked = true;
    internal int monstersAlive = 0;
    internal int[] weight;

    internal GameObject player;
    internal PlayerMovement playerMovement;

    internal GameObject[] Layouts;

    internal int areaLevel = 1;

    private Transform[] enemies;
    internal ModDataRoom.GeneratedMission mission;

    internal Bounds roomBounds;
    private bool m_Started;


    internal ModDataRoom.GeneratedMission mis;

    private void Start()
    {
        m_Started = true;
        GetBounds();  
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            
            Gizmos.DrawWireCube(roomBounds.center, roomBounds.size );
        }

    }

    public void GetBounds()
    {
        roomBounds = new Bounds(transform.position, Vector3.one);

        var colliders = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var col in colliders)
        {
            roomBounds.Encapsulate(col.bounds);
        }

    }

    

    /*
    private void Start()
    {
        portal.SetActive(false);
        StartCoroutine(WatchEnemies());
        StartCoroutine(TimerPortalUnlock());
    }

    private void OnEnable()
    {
        StartCoroutine(WatchEnemies());
        StartCoroutine(TimerPortalUnlock());
    }*/



    public void PickLayout()
    {
        var x = Random.Range(0, Layouts.Length);

        var y = Layouts[x].GetComponentsInChildren<Monster>();
        foreach (Monster z in y)
        {
            z.player = this.player;
            z.transform.parent = transform;
            z.level = areaLevel;
            monstersAlive++;
            ApplyMissionModsToMonster(z);
            z.UpdateStatsToLevel();
            if (mission.bloodline) BloodLine(z);
        }

        Layouts[x].SetActive(true);

    }

    private void BloodLine(Monster z)
    {
        Skull x = Instantiate(skull.GetComponent<Skull>(), z.transform.position, z.transform.rotation, transform) as Skull;
        x.player = this.player;
        x.transform.parent = transform;
        x.level = areaLevel;
        monstersAlive++;
        ApplyMissionModsToMonster(x);
        x.UpdateStatsToLevel();
    }

    private void ApplyMissionModsToMonster(Monster z)
    {
        z.health += mission.aditionalLife;
        z.damage += mission.aditionalDamage;
    }





    public int GetRandomWeightedIndex(int[] weights)
    {
        // Get the total sum of all the weights.
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }

        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int index = 0;
        int lastIndex = weights.Length - 1;//elementCount
        while (index < lastIndex)
        {
            // Do a probability check with a likelihood of weights[index] / weightSum.
            if (Random.Range(0, weightSum) < weights[index])
            {
                return index;
            }

            // Remove the last item from the sum of total untested weights and try again.
            weightSum -= weights[index++];
        }

        // No other item was selected, so return very last index.
        return index;
    }








}
