using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoom : MonoBehaviour
{
    public Room room;

    public GameObject door;

    public GameObject[] symbols;

    public Transform doorPlace;

    public GameObject HealthPot;

    public GameObject weapon;

    public GameObject[] enemies;

    public Transform instantiatePos;

    private GameObject symbol;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(WaiterStart());
    }

    private void GenerateSpecialRoom()
    {
        var result = Random.Range(0, symbols.Length);
        result = 2;
        switch (result)
        {
            case 0:
                GenerateHealthRoom();
                break;

            case 1:
                GenerateGunRoom();
                break;

            case 2:
                GenerateBossRoom();
                break;

        }
    }

    private void GenerateBossRoom()
    {
        symbol = Instantiate(symbols[2], doorPlace.position, doorPlace.rotation, transform);
        var enemy = Instantiate(enemies[Random.Range(0,enemies.Length)], instantiatePos.position, transform.rotation, room.transform);
        var script = enemy.GetComponent<Monster>();
        script.player = room.player;
        script.level += 10;
        script.damage += 10;
        script.health += 250;
    }

    private void GenerateGunRoom()
    {
        symbol = Instantiate(symbols[1], doorPlace.position, doorPlace.rotation, transform);
        Instantiate(weapon, instantiatePos.position, transform.rotation, transform);
    }

    private void GenerateHealthRoom()
    {
        symbol = Instantiate(symbols[0], doorPlace.position, doorPlace.rotation, transform);
        Instantiate(HealthPot, instantiatePos.position, transform.rotation, transform);
    }

    internal void OpenDoor()
    {
        door.SetActive(false);
        symbol.SetActive(false);
    }

    IEnumerator WaiterStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (Random.Range(1, 100) + room.mission.increasedChanceSpecialRooms > 0) GenerateSpecialRoom();
    }
}
