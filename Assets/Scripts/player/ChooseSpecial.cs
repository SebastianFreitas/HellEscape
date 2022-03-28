
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ChooseSpecial:MonoBehaviour
{
    [SerializeField] GameObject healDrop;
    [SerializeField] GameObject bench;
    internal void DropHeal()
    {
        Instantiate(healDrop, transform.localPosition, transform.rotation, transform);
        DisableEvent();
    }

    private void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    internal void SpawnBench()
    {
        GetComponentInParent<RoomActivator>().SpawnCraftingBench();
        DisableEvent();
    }

    internal void NormalEncounter()
    {
        var x = GetComponentInParent<RoomActivator>();
        x.SpawnEncounter(Random.Range(2, x.mission.encounterMobCount), false);
    }

    internal void HardEncountner()
    {
        var x = GetComponentInParent<RoomActivator>();
        x.SpawnEncounter(Random.Range(2, x.mission.encounterMobCount), true);
    }

    internal void ExtremeEncounter()
    {
        var x = GetComponentInParent<RoomActivator>();
        x.SpawnEncounter(Random.Range(x.mission.encounterMobCount, x.mission.encounterMobCount *2), true);
    }
}