using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public GameObject light;
    public float timeOff;
    public float timeOn;
    // Start is called before the first frame update


    void Start()
    {
        StartCoroutine(waiterOn(timeOn));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator waiterOn(float time)
    {
        time += Random.Range(-10f,10f);
        yield return new WaitForSeconds(time);
        light.GetComponent<Light>().enabled = false;
        StartCoroutine(waiterOff(timeOff));
    }

    private IEnumerator waiterOff(float time)
    {   
        time += Random.Range(-.1f,.1f);
        yield return new WaitForSeconds(time);
        light.GetComponent<Light>().enabled = true;
        StartCoroutine(waiterOn(timeOn));
    }
}
