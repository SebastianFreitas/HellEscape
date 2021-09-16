using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip openPortalSound;

    public Light[] lights;

    public CraftingDevice craftDevice;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            transform.parent.GetComponent<Room>().UseDoor();

        }        
    }

    void OnEnable()
    {
        craftDevice.portalOnline = true;
        craftDevice.offline.SetActive(true);

        foreach (var x in lights)
        {
            x.color = Color.red;
        }
        audioSource.PlayOneShot(openPortalSound);
    }

}
