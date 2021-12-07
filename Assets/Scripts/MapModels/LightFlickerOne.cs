using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerOne : MonoBehaviour
{
    public GameObject lamp;
    public Light light_;
    public MeshRenderer meshON;
    public MeshRenderer meshOFF;

    //public Material brightness;

    public float waitTime;     // The total of seconds the flash wil last
    private float maxIntensity;     // The maximum intensity the flash will reach

    public float timeOff;
    public float timeOn;

    public bool oN = false;

    void Start()
    {
        maxIntensity = light_.intensity;
        //brightness = this.transform.GetComponentInChildren<Renderer>().materials[1];// = new Material(brightness);
        //brightness = new Material(brightness);
        if (Random.Range(1,10) > 7 || oN)   StartCoroutine(WaiterOn(timeOn));
          
    }

    public IEnumerator TurnOnSlowly()
    {
       // waitTime += Random.Range(0, 0.5f);

        while (light_.intensity < maxIntensity)
        {
            light_.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        yield return null;
    }

    private IEnumerator WaiterOn(float time)
    {
        time += Random.Range(-2f, 7f);
        yield return new WaitForSeconds(time);

        TurnOff();

        StartCoroutine(WaiterOff(timeOff));
    }

    private IEnumerator WaiterOff(float time)
    {
        time += Random.Range(-.8f, 2f);
        yield return new WaitForSeconds(time);
        if (Random.Range(1, 10) > 3) StartCoroutine(Blink());
        else TurnOnSlow();

        StartCoroutine(WaiterOn(timeOn));
    }

    public void TurnOff()
    {
        light_.enabled = false;
        SwitchMeshRender(false, true);
        //brightness.DisableKeyword("_EMISSION");
    }

    private void SwitchMeshRender(bool on, bool off)
    {
        meshON.enabled = on;
        meshOFF.enabled = off;
    }

    public void TurnOn()
    {
        light_.enabled = true;
        SwitchMeshRender(true, false);
        //brightness.EnableKeyword("_EMISSION");
    }

    private void TurnOnSlow()
    {
        light_.enabled = true;
        light_.intensity = 0;
        SwitchMeshRender(true, false);
        StartCoroutine(TurnOnSlowly());
    }


    public void GetShot()
    {
        TurnOff();
        StartCoroutine(GetShotWaiter(Random.Range(2f,7f)));
    }

    private IEnumerator  GetShotWaiter(float v)
    {
        TurnOff();
        yield return new WaitForSeconds(v);
        TurnOnSlow();
    }

    private IEnumerator Blink()
    {
        var time = 0.07f;
        for(int i = 0; i< Random.Range(2,6); i++)
        {
            time += Random.Range(-0.03f, 0.03f);
            TurnOff();
            yield return new WaitForSeconds(time);
            TurnOn();
            yield return new WaitForSeconds(time);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (Random.Range(1,10) > 6) GetShot();
            else StartCoroutine(Blink());
        }
        
    }


}
