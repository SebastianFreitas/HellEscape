using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTower : MonoBehaviour
{
    // Start is called before the first frame update
    bool hasBeenFound = false;
    private void OnTriggerEnter(Collider other)
    {
        if((other.CompareTag("Monster") || other.CompareTag("MonsterHead")) &&!hasBeenFound)
        {
            hasBeenFound = true;
            other.GetComponentInParent<TowerBoss>().StartCoroutine("Stop");
        }
    }
}
