using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetManagement : MonoBehaviour
{
    public GameObject ceiling;
    public GameObject floor;

    void Start()
    {
        StartCoroutine(WaitCeiling());
        StartCoroutine(WaitFloor());
    }


    public IEnumerator WaitCeiling()
    {
        yield return new WaitForSeconds(Random.Range(1, 120));
        ceiling.SetActive(false);
    }

    public IEnumerator WaitFloor()
    {
        yield return new WaitForSeconds(Random.Range(1, 120));
        floor.SetActive(false);
    }
}
