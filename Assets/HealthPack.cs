using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip collected;
    private float volume;

    private bool hasBeenCollected = false;


    void Start()
    {
        volume = PlayerPrefs.GetFloat("Volume");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !hasBeenCollected)
        {
            CollectItem(other);
            hasBeenCollected = true;
        }
    }

    private void CollectItem(Collider other)
    {
        other.GetComponent<PlayerHpManager>().TakeDamage(-50);
    }
}
