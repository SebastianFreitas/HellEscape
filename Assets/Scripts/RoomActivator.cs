using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour
{
    [SerializeField] GameObject[] doorMesh;
    [SerializeField] Monster[] monstersRed;
    [SerializeField] Monster[] monstersBlue;
    [SerializeField] GameObject lights;

    [SerializeField] GameObject healthPack;
    [SerializeField] GameObject CraftingBench;
    [SerializeField] GameObject weaponDrop;

    internal Transform spawnPos;

    internal int numberOfEnemies = 0;
    private BoxCollider[] boxColliders;
    private Light[] lightsComponent;

    private RoomGenerator roomgen;
    internal enum RoomType
    {
        Boss,
        Encounter,
        Special,
        Main,
        Corridor,
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
        MaxHP,
        Elite,
        weapon,
        Hard,
        Extreme,
    }

    internal enum MainType
    {
        Shop,
        choiceSpecial,
        itemOrDrop,
        redOrBlue,
        switchInfluence,
    }

    internal RoomType roomType;
    internal MainType mainType;
    internal AreaType influcence = AreaType.Red;

    internal ModDataRoom.GeneratedMission mission;
    internal bool beenTrigered;

    private void Start()
    {
        if (transform.childCount > 0) spawnPos = transform.GetChild(0);
        roomgen = GetComponentInParent<RoomGenerator>();
        lightsComponent = lights.GetComponentsInChildren<Light>();

        foreach (GameObject door in doorMesh) door.SetActive(false);

        boxColliders = transform.GetComponents<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !beenTrigered)
        {
            beenTrigered = true;
            switch (roomType)
            {
                case RoomType.Boss:
                {
                    SpawnBoss();
                    break;
                }

                case RoomType.Encounter:
                {
                    SpawnEncounter(Random.Range(2,mission.encounterMobCount), false);
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
            }
        }
    }


    private void SpawnMain()
    {

        switch (mainType)
        {
            case MainType.choiceSpecial:
                Instantiate(roomgen.choseSpecial, spawnPos.position, Quaternion.identity, transform);
                Debug.Log("choicespecial");
                break;

            case MainType.itemOrDrop:
                Instantiate(roomgen.itemRoom, spawnPos.position, Quaternion.identity, transform);
                TurnLightsRed();
                Debug.Log("itemroom");
                break;

            case MainType.redOrBlue:
                // Instantiate(roomgen.changeInfluence, spawnPos.position, Quaternion.identity, transform);
                Debug.Log("red or blue drop");
                break;

            case MainType.Shop:
                //Instantiate(roomgen.changeInfluence, spawnPos.position, Quaternion.identity, transform);
                Debug.Log("shop");
                TurnLightsRed();
                break;

            case MainType.switchInfluence:
                Instantiate(roomgen.changeInfluence, spawnPos.position, Quaternion.identity, transform);
                Debug.Log("Switch influence");

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
 
                    Debug.Log("crafting");
                    break;
                }

            case SpecialType.Hard:
                {
                    SpawnEncounter(Random.Range(2, mission.encounterMobCount), true);

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
                    break;
                }
            case SpecialType.weapon:
                {
                    SpawnWeapon();
                    TurnLightsRed();
                    break;
                }
        }
    }
    private Monster SpawnByInfluence(Vector3 position, Quaternion rotation, bool elite)
    {
        Monster currentMob;
        switch (influcence)
        {
            case AreaType.Blue:
                currentMob = Instantiate(monstersBlue[0], position, rotation, transform);
                currentMob.isElite = elite;
                return currentMob;

            case AreaType.Red:
                currentMob = Instantiate(monstersRed[0], position, rotation, transform);
                currentMob.isElite = elite;
                return currentMob;
        }
        currentMob = Instantiate(monstersRed[0], position, rotation, transform);
        return currentMob;
    }


    private void SpawnWeapon()
    {

        Instantiate(weaponDrop, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    private void SpawnHeal()
    {
        Instantiate(healthPack, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    private void SpawnElite()
    {
        Monster mob = SpawnByInfluence(spawnPos.position,spawnPos.rotation, true);
        mob.TurnElite();
        mob = SpawnByInfluence(spawnPos.position, spawnPos.rotation, true);
        mob.TurnElite();

        CloseDoors();
        ClearTrigger();
    }

    internal void SpawnCraftingBench()
    {
        Instantiate(CraftingBench, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    internal void SpawnEncounter(int totalMobs, bool elite)
    {
        for (int i = 0; i < totalMobs; i++)
        {
            var chosenCollider = boxColliders[Random.Range(0, boxColliders.Length)];

            Vector3 randomPoint = RandomPointInBounds(chosenCollider.bounds);
            SpawnByInfluence(randomPoint, Quaternion.identity, elite);
            numberOfEnemies++;
        }

        CloseDoors();
        ClearTrigger();
    }

   

    private void SpawnBoss()
    {
        SpawnByInfluence(spawnPos.position, Quaternion.identity, false);
        numberOfEnemies++;
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
            foreach (GameObject door in doorMesh) door.SetActive(false);
            TurnLightsRed();
        }

    }

    internal void TurnLightsRed()
    {

        switch (influcence)
        {
            case AreaType.Blue:
                foreach (Light light in lightsComponent) light.color = Color.blue;
                break;

            case AreaType.Red:
                foreach (Light light in lightsComponent) light.color = Color.red;
                break;
        }
    }
}
