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

    internal enum RoomType
    {
        Boss,
        Encounter,
        Special,
        Main,
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
    internal AreaType influcence = AreaType.Blue;

    internal ModDataRoom.GeneratedMission mission;
    private void Start()
    {
        lightsComponent = lights.GetComponentsInChildren<Light>();

        foreach (GameObject door in doorMesh) door.SetActive(false);

        boxColliders = transform.GetComponents<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            switch (roomType)
            {
                case RoomType.Boss:
                {
                    SpawnBoss();
                    break;
                }

                case RoomType.Encounter:
                {
                    SpawnEncounter(4);
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
            }
        }
    }

    private void SpawnMain()
    {
        switch (mainType)
        {
            case MainType.choiceSpecial:

                break;

            case MainType.itemOrDrop:

                break;

            case MainType.redOrBlue:

                break;

            case MainType.Shop:

                break;

            case MainType.switchInfluence:

                break;
        }
    }

    private void SpawnSpecial()
    {
        spawnPos = transform.GetChild(0);

        SpecialType type = (SpecialType)Random.Range(0, System.Enum.GetValues(typeof(SpecialType)).Length);

        switch (type)
        {
            case SpecialType.Crafting:
                {
                    SpawnCraftingBench();
                    break;
                }

            case SpecialType.Elite:
                {
                    SpawnElite();
                    break;
                }

            case SpecialType.Heal:
                {
                    SpawnHeal();
                    break;
                }
            case SpecialType.MaxHP:
                {
                    SpawnMaxHP();
                    break;
                }
            case SpecialType.weapon:
                {
                    SpawnWeapon();
                    break;
                }
        }
    }
    private Monster SpawnByInfluence(Vector3 position, Quaternion rotation)
    {
        Monster currentMob;
        switch (influcence)
        {
            case AreaType.Blue:
                currentMob = Instantiate(monstersBlue[0], position, rotation, transform);
                return currentMob;

            case AreaType.Red:
                currentMob = Instantiate(monstersRed[0], position, rotation, transform);
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

    private void SpawnMaxHP()
    {
        Instantiate(healthPack, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    private void SpawnHeal()
    {
        Instantiate(healthPack, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    private void SpawnElite()
    {
        Monster mob = SpawnByInfluence(spawnPos.position,spawnPos.rotation);
        mob.TurnElite();
        mob = SpawnByInfluence(spawnPos.position, spawnPos.rotation);
        mob.TurnElite();
        numberOfEnemies++;
        CloseDoors();
        ClearTrigger();
    }

    private void SpawnCraftingBench()
    {
        Instantiate(CraftingBench, spawnPos.position, spawnPos.rotation, transform);
        TurnLightsRed();
    }

    private void SpawnEncounter(int totalMobs)
    {
        for (int i = 0; i < totalMobs; i++)
        {
            var chosenCollider = boxColliders[Random.Range(0, boxColliders.Length)];

            Vector3 randomPoint = RandomPointInBounds(chosenCollider.bounds);
            SpawnByInfluence(randomPoint, Quaternion.identity);
            numberOfEnemies++;
        }

        CloseDoors();
        ClearTrigger();
    }

    private void SpawnBoss()
    {
        SpawnByInfluence(spawnPos.position, Quaternion.identity);
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

    private void TurnLightsRed()
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
