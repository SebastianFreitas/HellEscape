using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour
{
    [SerializeField] GameObject[] doorMesh;
    [SerializeField] Monster[] monsters;
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
    }

    internal enum SpecialType
    {
        Crafting,
        Heal,
        MaxHP,
        Elite,
        weapon,
    }

    internal RoomType roomType;

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
            }
        }
    }

    private void SpawnSpecial()
    {
        spawnPos = transform.GetChild(0);

        SpecialType type = (SpecialType)Random.Range(0, 5);

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

    private void SpawnWeapon()
    {
        Instantiate(weaponDrop, spawnPos.position, spawnPos.rotation, transform);
    }

    private void SpawnMaxHP()
    {
        Instantiate(healthPack, spawnPos.position, spawnPos.rotation, transform);
    }

    private void SpawnHeal()
    {
        Instantiate(healthPack, spawnPos.position, spawnPos.rotation, transform);
    }

    private void SpawnElite()
    {
        throw new System.NotImplementedException();
    }

    private void SpawnCraftingBench()
    {
        Instantiate(CraftingBench, spawnPos.position, spawnPos.rotation, transform);
    }

    private void SpawnEncounter(int totalMobs)
    {
        for (int i = 0; i < totalMobs; i++)
        {
            var chosenCollider = boxColliders[Random.Range(0, boxColliders.Length)];

            Vector3 randomPoint = RandomPointInBounds(chosenCollider.bounds);
            Instantiate(monsters[0], randomPoint, Quaternion.identity, transform);
            numberOfEnemies++;
        }

        CloseDoors();
        ClearTrigger();
    }

    private void SpawnBoss()
    {
        Instantiate(monsters[0], spawnPos.position, spawnPos.rotation, transform);
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
            foreach (Light light in lightsComponent) light.color = Color.red;
        }

    }
}
