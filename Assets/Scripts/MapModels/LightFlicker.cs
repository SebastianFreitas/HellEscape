using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public List<GameObject> allObjects;
    public float timeOff;
    public float timeOn;
   


    void Start()
    {
        for(int i = 0; i<allObjects.Count;i++)
        {
            if (Random.value > .8) StartCoroutine(waiterOn(timeOn, i));
        }

        //add a omega flashing constanly light
    }

    private IEnumerator waiterOn(float time, int i)
    {
        var light = allObjects[i];

        time += Random.Range(-7f,7f);//time += Random.Range(-10f,10f);
        yield return new WaitForSeconds(time);

        light.SetActive(false);

        if (Random.Range(0, 1) > .6 && i - 1 > 0) StartCoroutine(WaiterNeighbor(1f, i - 1));

        StartCoroutine(waiterOff(timeOff, i));
    }

    private IEnumerator waiterOff(float time,int i)
    {
        var light = allObjects[i];

        time += Random.Range(-.5f,.5f);
        yield return new WaitForSeconds(time);

        light.SetActive(true);

        StartCoroutine(waiterOn(timeOn, i));
    }

    //theres a chance that when a ligh goes out another close to it might also
    private IEnumerator WaiterNeighbor(float time, int i)
    {
        var lightneighbor = allObjects[i];

        lightneighbor.SetActive(false);

        yield return new WaitForSeconds(time);

        lightneighbor.SetActive(true);
    }
}
