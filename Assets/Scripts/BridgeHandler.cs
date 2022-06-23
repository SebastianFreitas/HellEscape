using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    private int dificulty = 0;
    private int distance = 10;
    private bool passed = false;

    [SerializeField] List<BridgeHandler> pieces;
    [SerializeField] GameObject meshPieces;
    [SerializeField] GameObject lights;
    [SerializeField] Transform nextPosition;

    [SerializeField] List<Monster> monsters;

    [SerializeField] internal GameObject mainBase;
    void Start()
    {
        if (PlayerPrefs.HasKey("PathLevel")) dificulty = PlayerPrefs.GetInt("PathLevel");
        else SaveDificulty();
        
        foreach(Transform child in meshPieces.transform)
        {
            child.gameObject.SetActive(true);
            if (Random.Range(1, 101) > 75) child.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dude") && !passed)
        {
            passed = true;

            if(distance <= 0)
            {
                transform.root.GetComponent<GameMan>().startedRun = true;
                dificulty++;
                SaveDificulty();

                var x = Instantiate(mainBase, nextPosition.position, Quaternion.identity, transform);
                x.transform.parent = GetComponentInParent<Hub>().transform;

                var par = transform.parent;
                transform.parent = x.transform;
                Destroy(par.GetComponentInParent<StartHub>().gameObject);
            }
            else
            {
                Vector3 randomizer = new Vector3(Random.Range(-10f,10f), Random.Range(-5f, 5f), Random.Range(-2f, 10f));
                Vector3 randomizerAngle = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f,20f), Random.Range(-20f, 20f));
                Quaternion rotation;

                if (Random.Range(0, 100) > 50)  rotation = Quaternion.identity * Quaternion.Euler(randomizerAngle);
                else                            rotation = nextPosition.rotation * Quaternion.Euler(randomizerAngle);

                var nextPiece = Instantiate(pieces[Random.Range(0, pieces.Count)], nextPosition.position + randomizer, rotation); //Random.Range(0, pieces.Count -1)
                nextPiece.transform.parent = transform.parent;
                distance--;
                nextPiece.SetDistance(distance--);
                

                if (IsDivisible(distance, 2))
                {
                    if (distance < dificulty) nextPiece.SpawnMonsters(EncounterType.elite);
                    else nextPiece.SpawnMonsters(EncounterType.normal);
                } 
                else if (distance == 1)
                {
                    nextPiece.SpawnMonsters(EncounterType.boss);
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        TurnLights(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        TurnLights(true);
    }

    private void TurnLights(bool value)
    {
        foreach (Transform child in lights.transform)
        {
            child.gameObject.SetActive(value);
        }
    }

    private void SaveDificulty()
    {
        PlayerPrefs.SetInt("PathLevel", dificulty);
    }

    internal void SetDistance(int distance)
    {
        this.distance = distance;
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("PathLevel"))
        {
            dificulty = PlayerPrefs.GetInt("PathLevel");
        }
        else dificulty = 0;

       
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
    public bool IsDivisible(int x, int n)
    {
        return (x % n) == 0;
    }

    internal enum EncounterType
    {
        normal,
        elite,
        boss
    }

    private GameObject player;
    internal void SpawnMonsters(EncounterType type)
    {
        player = transform.root.GetComponent<GameMan>().player;

        var total = 2;
        if (EncounterType.normal != type) total *= 2;

        for (int i = 0; i < total; i++)
        {
            var vector = RandomPointInBounds(GetComponent<BoxCollider>().bounds);
            var mob = Instantiate(monsters[Random.Range(0, monsters.Count)], vector, Quaternion.identity, transform);
            mob.player = player;
            if (type != EncounterType.normal) mob.TurnElite();
        }

        //if (type == EncounterType.boss)
        //{
        //    var vector = RandomPointInBounds(GetComponent<BoxCollider>().bounds);
        //    var mob = Instantiate(monsters[Random.Range(0, monsters.Count)], vector, Quaternion.identity, transform);
        //    mob.player = player;
        //    mob.TurnElite();

        //}
    }
}
