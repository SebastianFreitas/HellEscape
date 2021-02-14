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
             StartCoroutine(waiterOn(timeOn, i));//if (Random.value > .7)
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator waiterOn(float time, int i)
    {
        time += Random.Range(-5f,5f);//time += Random.Range(-10f,10f);
        yield return new WaitForSeconds(time);
        allObjects[i].GetComponent<Light>().enabled = false;
        allObjects[i].SetActive(false);
        StartCoroutine(waiterOff(timeOff, i));
    }

    private IEnumerator waiterOff(float time,int i)
    {
        time += Random.Range(-.5f,.5f);
        yield return new WaitForSeconds(time);
        allObjects[i].GetComponent<Light>().enabled = true;
        allObjects[i].SetActive(true);
        StartCoroutine(waiterOn(timeOn, i));
    }
}
