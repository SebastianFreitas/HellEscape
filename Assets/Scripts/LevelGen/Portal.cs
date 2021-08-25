using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip openPortalSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude"))
        {
            transform.parent.GetComponent<Room>().UseDoor();

        }
            
    }

}
