using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public List<GameObject> allObjects;
    public float timeOff;
    public float timeOn;
    // Start is called before the first frame update


    void Start()
    {
        for(int i = 0; i<allObjects.Count;i++)
        {
            if(Random.value > 0.75) StartCoroutine(waiterOn(timeOn, i));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator waiterOn(float time, int i)
    {
        time += Random.Range(-10f,10f);
        yield return new WaitForSeconds(time);
        allObjects[i].GetComponent<Light>().enabled = false;
        StartCoroutine(waiterOff(timeOff, i));
    }

    private IEnumerator waiterOff(float time,int i)
    {
        time += Random.Range(-.1f,.1f);
        yield return new WaitForSeconds(time);
        allObjects[i].GetComponent<Light>().enabled = true;
        StartCoroutine(waiterOn(timeOn, i));
    }
}
