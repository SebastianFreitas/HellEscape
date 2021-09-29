using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip openPortalSound;

    public Light[] lights;

    public CraftingDevice craftDevice;

    public ParticleSystem exp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            transform.parent.GetComponent<Room>().UseDoor();

        }        
    }

    void OnEnable()
    {
        if (exp != null)
        {
            exp.gameObject.SetActive(true);
            exp.Play();   
        }


        if ( craftDevice != null)
        {
            craftDevice.portalOnline = true;
            craftDevice.offline.SetActive(true);
        }


        foreach (var x in lights)
        {
            x.color = Color.red;
        }
        audioSource.PlayOneShot(openPortalSound);


    }

    void OnDisable()
    {
        Destroy(exp, exp.main.duration);
    }

}
