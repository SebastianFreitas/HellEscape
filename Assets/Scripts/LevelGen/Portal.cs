using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip openPortalSound;

    public Light[] lights;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            transform.parent.GetComponent<Room>().UseDoor();

        }
        foreach (var x in lights)
        {
            x.color = Color.red;
        }
            
    }

    void OnEnable()
    {
        audioSource.PlayOneShot(openPortalSound);
    }

}
