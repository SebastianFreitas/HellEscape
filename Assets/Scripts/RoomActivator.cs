using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour
{
    [SerializeField] GameObject[] doorMesh;

    [SerializeField] GameObject lights;

    [SerializeField] GameObject trapLayouts;

    internal Transform spawnPos;
    private Transform spawnPosBoss;

    internal int numberOfEnemies = 0;
    private BoxCollider[] boxColliders;
    private Light[] lightsComponent;

    internal RoomGenerator roomgen;

    private GameMan gameMan;
    internal enum RoomType
    {
        Boss,
        Encounter,
        Special,
        Main,
        Corridor,
        Boon
    }

    internal enum AreaType
    {
        Red,
        Blue,
    }

    internal enum SpecialType
    {
        Crafting,
        Heal,
        weapon,
        Hard,
        Extreme,
        Mutation,
    }

    internal enum MainType
    {
        Shop,
        choiceSpecial,
        itemOrDrop,
        redOrBlue,
        switchInfluence,
        Exit,
    }

    internal RoomType roomType;
    internal MainType mainType;
    internal VoidBoon.BoonType influcence = VoidBoon.BoonType.Red;

    internal ModDataRoom.GeneratedMission mission;
    internal bool beenTrigered = false;

    internal bool isSideRoom = false;

    private void Start()
    {
        gameMan = transform.root.GetComponent<GameMan>();

        if (transform.childCount > 0)
        {
            spawnPos = transform.GetChild(0);
            if(roomType == RoomType.Boss) spawnPosBoss = transform.GetChild(1);
        }
        roomgen = GetComponentInParent<RoomGenerator>();
        lightsComponent = lights.GetComponentsInChildren<Light>();

        foreach (GameObject door in doorMesh) door.SetActive(false);

        boxColliders = transform.GetComponents<BoxCollider>();

        //if (trapLayouts)
        //{
        //    foreach (Transform child in trapLayouts.transform) child.gameObject.SetActive(false);

            
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {

            if (!isSideRoom)roomgen.ManageRoomsEficiency(transform.GetComponentInParent<Room>());

            if (!beenTrigered)
            {
               beenTrigered = true;

                switch (roomType)
                {
                    case RoomType.Boss:
                    {
                        SpawnBoss();
                           // trapLayouts.transform.GetChild(Random.Range(0, trapLayouts.transform.childCount - 1)).gameObject.SetActive(true);
                            break;
                    }

                    case RoomType.Encounter:
                    {
                        SpawnEncounter(Random.Range(3,mission.encounterMobCount), false);
                            //trapLayouts.transform.GetChild(Random.Range(0, trapLayouts.transform.childCount - 1)).gameObject.SetActive(true);
                            break;
                    }

                    case RoomType.Special:
                    {
                        SpawnSpecial();
                        break;
                    }
                    case RoomType.Main:
                    {
                        SpawnMain();
                        break;
                    }
                    case RoomType.Corridor:
                    {
                        TurnLightsRed();
                        break;
                    }
                    //case RoomType.Trap:
                    //{
                    //    ActivateTraps();
                    //    break;
                    //}
                    case RoomType.Boon:
                    {
                        Instantiate(roomgen.itemRoom, spawnPos.position, spawnPos.rotation, transform);
                        TurnLightsRed();
                        break;
                    }
                }
            }

        }
    }

    private void ActivateTraps()
    {
        if (trapLayouts)
        {
            foreach (Transform child in trapLayouts.transform) child.gameObject.SetActive(false);

            //trapLayouts.transform.GetChild(trapLayouts.transform.childCount -2).gameObject.SetActive(true);
        }
    }

    private void SpawnMain()
    {

        switch (mainType)
        {
            case MainType.choiceSpecial:
                Instantiate(roomgen.choseSpecial, spawnPos.position, spawnPos.rotation, transform);
               // foreach (Transform child in trapLayouts.transform) child.gameObject.SetActive(false);

                break;

            case MainType.itemOrDrop:
                Instantiate(roomgen.itemRoom, spawnPos.position, spawnPos.rotation, transform);
                TurnLightsRed();

                break;

            case MainType.redOrBlue:
                Instantiate(roomgen.influenceItem, spawnPos.position, spawnPos.rotation, transform);
                ActivateTraps();
                TurnLightsRed();

                break;

            case MainType.Shop:
                Instantiate(roomgen.shop, spawnPos.position, spawnPos.rotation, transform);
                TurnLightsRed();
                break;

            case MainType.switchInfluence:
                Instantiate(roomgen.changeInfluence, spawnPos.position, spawnPos.rotation, transform);
                CloseDoors();

                break;

            case MainType.Exit:
                SpawnExit();

                break;
        }
    }

    private void SpawnSpecial()
    {
        SpecialType type = (SpecialType)Random.Range(0, System.Enum.GetValues(typeof(SpecialType)).Length);

        switch (type)
        {
            case SpecialType.Crafting:
                {
                    SpawnCraftingBench();
                    TurnLightsRed();
                    break;
                }

            case SpecialType.Hard:
                {
                    SpawnEncounter(Random.Range(2, mission.encounterMobCount), true);
                   // trapLayouts.transform.GetChild(Random.Range(0, trapLayouts.transform.childCount - 1)).gameObject.SetActive(true);

                    break;
                }

            case SpecialType.Heal:
                {
                    SpawnHeal();
                    TurnLightsRed();
                    break;
                }
            case SpecialType.Extreme:
                {
                    SpawnEncounter(Random.Range(mission.encounterMobCount, mission.encounterMobCount*2), true);
                   // trapLayouts.transform.GetChild(Random.Range(0, trapLayouts.transform.childCount - 1)).gameObject.SetActive(true);
                    break;
                }
            case SpecialType.weapon:
                {
                    SpawnWeapon();
                    TurnLightsRed();
                    break;
                }
            case SpecialType.Mutation:
                {
                    Instantiate(roomgen.itemRoom, spawnPos.position, spawnPos.rotation, transform);
                    TurnLightsRed();
                    break;
                }
        }
    }
    internal Monster SpawnByInfluence(Vector3 position, Quaternion rotation, bool elite, bool Isboss, Monster mob)
    {
        Monster currentMob;

        gameMan.isInEncounter = true;

        currentMob = Instantiate(mob, position, rotation, transform);

        currentMob.isElite = elite;
        return currentMob;
       
    }

    internal void SpawnWeapon()
    {

        Instantiate(roomgen.weaponDrop, spawnPos.position + Vector3.up, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    internal void SpawnHeal()
    {
        Instantiate(roomgen.healthPack, spawnPos.position +Vector3.up, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    internal void SpawnCraftingBench()
    {
        Instantiate(roomgen.CraftingBench, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    internal void SpawnEncounter(int totalMobs, bool elite)
    {
        if (roomgen.mission.doubleMobs) totalMobs *= 2;

        int index = 0;
        int pick = Random.Range(roomgen.encounterCounter/2, roomgen.encounterCounter);
        Monster x;

        for (int i = 0; i < totalMobs; i++)
        {
            var chosenCollider = boxColliders[Random.Range(0, boxColliders.Length)];


            x = roomgen.encounters[pick][index];
            index++;
            if (index == 4) index = 0;

            Vector3 randomPoint = RandomPointInBounds(chosenCollider.bounds);
            SpawnByInfluence(randomPoint, Quaternion.identity, elite, false, x);
            numberOfEnemies++;
        }

        CloseDoors();
        ClearTrigger();

        if(roomgen.encounterCounter < roomgen.encounters.Count)
            roomgen.encounterCounter++;
    }

    private void SpawnBoss()
    {
       
        var x = Random.Range(0, roomgen.BossRed.Length);
        SpawnByInfluence(spawnPos.position, Quaternion.identity, false, true,roomgen.BossRed[x]);
        numberOfEnemies++;

        if (roomgen.mission.doubleMobs)
        {
            SpawnByInfluence(spawnPos.position + Vector3.forward, Quaternion.identity, false, true, roomgen.BossRed[x]);
            numberOfEnemies++;
        }
        CloseDoors();

        ClearTrigger();
    }

    private void ClearTrigger()
    {
        foreach (BoxCollider box in boxColliders) box.size = Vector3.zero;
    }

    private void CloseDoors()
    {
        foreach (GameObject door in doorMesh) door.SetActive(true);
    }

    private Vector3 RandomPointInBounds(Bounds rawBounds)
    {
        Bounds bounds = rawBounds;
        bounds.size /= 2;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    internal void IsEncounterDone()
    {
        numberOfEnemies--;

        if (numberOfEnemies == 0)
        {
            OpenDoors();
            TurnLightsRed();
            if (roomType == RoomType.Boss) SpawnExit();



            if (Random.Range(1f, 100f) > 100 - roomgen.mission.healthChanceEncounter) SpawnHeal();
            if (Random.Range(1f, 100f) > 100 - (roomgen.mirrorBoonChance  + 5))
            {
                Instantiate(roomgen.itemRoom, spawnPos.position + Vector3.back, spawnPos.rotation, transform);
            }

            gameMan.isInEncounter = false;
        }

        
    }

    private void SpawnExit()
    {
        Transform newPos = spawnPos;
        if (roomType == RoomType.Boss) newPos = transform.GetChild(1);


        Instantiate(roomgen.exit, newPos.position , newPos.rotation, transform);

    }

    internal IEnumerator UpdateSymbolMainType()
    {
        yield return new WaitForSecondsRealtime(1f);

        foreach (GameObject door in doorMesh)
        {
            door.SetActive(true);
            door.GetComponentInChildren<DoorSymbolManager>().UpdateSymbolMainType(mainType);
        }
    }
    internal IEnumerator UpdateSymbolRoomType()
    {
        yield return new WaitForSecondsRealtime(1f);

        foreach (GameObject door in doorMesh)
        {
            door.SetActive(true);
            door.GetComponentInChildren<DoorSymbolManager>().UpdateSymbolRoomType(roomType);
        }
    }

    internal void OpenDoors()
    {
        foreach (GameObject door in doorMesh) door.SetActive(false);
    }

    internal void TurnLightsRed()
    {

        switch (influcence)
        {
            case VoidBoon.BoonType.Blue:
                foreach (Light light in lightsComponent) light.color = Color.blue;
                break;

            case VoidBoon.BoonType.Red:
                foreach (Light light in lightsComponent) light.color = Color.red;
                break;
        }
    }
}
