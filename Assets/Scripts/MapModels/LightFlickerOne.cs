using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickerOne : MonoBehaviour
{
    public GameObject lamp;
    public Light light_;
    public MeshRenderer mesh;

    //public Material brightness;

    public float waitTime;     // The total of seconds the flash wil last
    private float maxIntensity;     // The maximum intensity the flash will reach

    public float timeOff;
    public float timeOn;

    void Start()
    {
        maxIntensity = light_.intensity;
        //brightness = this.transform.GetComponentInChildren<Renderer>().materials[1];// = new Material(brightness);
        //brightness = new Material(brightness);
        if (Random.Range(1,10) > 7)   StartCoroutine(WaiterOn(timeOn));
          
    }

    public IEnumerator FlashNow()
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

        TurnOn();

        StartCoroutine(WaiterOn(timeOn));
    }

    private void TurnOff()
    {
        light_.enabled = false;
        mesh.enabled = false;
        //brightness.DisableKeyword("_EMISSION");
    }

    private void TurnOn()
    {
        light_.enabled = true;
        light_.intensity = 0;
        mesh.enabled = true;
        StartCoroutine(FlashNow());
        //brightness.EnableKeyword("_EMISSION");
    }

    private void Blink()
    {
        TurnOff();
        StartCoroutine(BlinkWaiter(5f));
    }

    private IEnumerator BlinkWaiter(float time)
    {
        time += Random.Range(-.5f, .5f);
        yield return new WaitForSeconds(time);

        TurnOn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Blink();
        }
        
    }


}
