using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTower : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] turnOffList;

    [SerializeField] GameObject door;
    [SerializeField] bool isFinal = false;

    bool hasBeenFound = false;
    private void OnTriggerEnter(Collider other)
    {
        if((other.CompareTag("Monster") || other.CompareTag("MonsterHead")) &&!hasBeenFound)
        {
            hasBeenFound = true;
            other.GetComponentInParent<TowerBoss>().StartCoroutine("Stop");

        }

        if ((other.CompareTag("Dude") ) && isFinal)
        {


            isFinal = false;
            foreach (var item in turnOffList)
            {
                item.gameObject.SetActive(false);
            }

           if(door) door.SetActive(true);
            
        }
    }
}
