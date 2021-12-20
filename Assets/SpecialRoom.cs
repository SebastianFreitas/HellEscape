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

    public Transform instantiatePos;
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
        Instantiate(symbols[2], doorPlace.position, doorPlace.rotation, doorPlace);
        //Instantiate(null, instantiatePos.position, transform.rotation, transform);
    }

    private void GenerateGunRoom()
    {
        Instantiate(symbols[1], doorPlace.position, doorPlace.rotation, doorPlace);
        //Instantiate(null, instantiatePos.position, transform.rotation, transform);
    }

    private void GenerateHealthRoom()
    {
        Instantiate(symbols[0], doorPlace.position, doorPlace.rotation, doorPlace);
        //Instantiate(null, instantiatePos.position, transform.rotation, transform);
    }

    internal void OpenDoor()
    {
        door.SetActive(false);
    }

    IEnumerator WaiterStart()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (Random.Range(1, 100) + room.mission.increasedChanceSpecialRooms > 5) GenerateSpecialRoom();
    }
}
