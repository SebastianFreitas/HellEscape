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
    [SerializeField] Transform nextPosition;

    [SerializeField] internal GameObject mainBase;
    void Start()
    {
        if (PlayerPrefs.HasKey("PathLevel")) dificulty = PlayerPrefs.GetInt("PathLevel");
        
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

                var x = Instantiate(mainBase, nextPosition.position, nextPosition.rotation, transform);
                x.transform.parent = GetComponentInParent<Hub>().transform;

                var par = transform.parent;
                transform.parent = x.transform;
                Destroy(par.GetComponentInParent<StartHub>().gameObject);
            }
            else
            {
                Vector3 randomizer = new Vector3(Random.Range(-10f,10f), Random.Range(-5f, 5f), Random.Range(-2f, 10f));
                Vector3 randomizerAngle = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                Quaternion rotation;

                if (Random.Range(0, 100) > 80) rotation = Quaternion.identity;
                else  rotation = nextPosition.rotation * Quaternion.Euler(randomizerAngle);

                var nextPiece = Instantiate(pieces[Random.Range(0, pieces.Count)], nextPosition.position + randomizer, rotation); //Random.Range(0, pieces.Count -1)
                nextPiece.transform.parent = transform.parent;
                distance--;
                nextPiece.SetDistance(distance--);
            }

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
}
