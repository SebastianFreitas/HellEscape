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

    internal void SpawnMonsters()
    {
        for (int i = 0; i < 2; i++)
        {
            var vector = RandomPointInBounds(GetComponent<BoxCollider>().bounds);
            Instantiate(monsters[Random.Range(0,monsters.Count)], vector, Quaternion.identity, transform);
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
                nextPiece.SpawnMonsters();
            }

            TurnOfLights();
        }
    }

    private void TurnOfLights()
    {
        foreach (Transform child in lights.transform)
        {
            child.gameObject.SetActive(false);
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

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Room"), false);
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
}
