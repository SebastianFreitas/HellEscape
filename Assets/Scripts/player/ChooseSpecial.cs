
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ChooseSpecial:MonoBehaviour
{
    private RoomActivator roomActi;

    private void Start()
    {
        roomActi = GetComponentInParent<RoomActivator>();
    }
    internal void DropHeal()
    {
        roomActi.SpawnHeal();
        DisableEvent();
    }

    private void DisableEvent()
    {
        gameObject.SetActive(false);
    }

    internal void SpawnBench()
    {
        roomActi.SpawnCraftingBench();
        DisableEvent();
    }

    internal void NormalEncounter()
    {
        roomActi.SpawnEncounter(Random.Range(2, roomActi.mission.encounterMobCount), false);
        DisableEvent();
    }

    internal void HardEncountner()
    {
        roomActi.SpawnEncounter(Random.Range(2, roomActi.mission.encounterMobCount), true);
        DisableEvent();
    }

    internal void ExtremeEncounter()
    {
        roomActi.SpawnEncounter(Random.Range(roomActi.mission.encounterMobCount, roomActi.mission.encounterMobCount *2), true);
        DisableEvent();
    }
}