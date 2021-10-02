using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip dash;
    public AudioClip land;
    public AudioClip teleportSound;
    public AudioClip[] steps;
    public AudioClip[] hurts;
    
    public float volume = 0.5f;
    public float nextFootstep = 0;
    public float footstepDelay = .3f;


    public void PlayDashSound()
    {
        audioSource.PlayOneShot(dash, volume / 2);
    }

    public void PlayLandSound()
    {
        audioSource.PlayOneShot(land, volume + .25f);
    }

    public void PlayTakeDamageSound()
    {
        audioSource.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume + 1);
    }

    internal void PlayTeleportSound()
    {
        audioSource.PlayOneShot(teleportSound, volume*2);
    }

    public void PlayFootStepsSound(bool isGrounded, Vector3 moveRaw, bool isSideDashing)
    {
        if (isGrounded)
        {
            if (moveRaw != Vector3.zero && !isSideDashing)
            {
                nextFootstep -= Time.deltaTime;
                if (nextFootstep <= 0)
                {
                    audioSource.PlayOneShot(steps[Random.Range(0, steps.Length)], volume);
                    nextFootstep += footstepDelay;
                }
            }
        }
    }
}
