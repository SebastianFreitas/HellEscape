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
    public GravityPuller gravPull;

    public MissionSelector selector;

    public bool isVoid = false;

    private bool passed = false;
    public bool ishub = false;

    internal bool isON = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dude") && !passed && isON)
        {
            passed = true;
            if (ishub)
            {
                selector.StartSelectedMission();
                Debug.Log("hee");
            }
            else
            {
                //other.transform.GetComponent<PlayerSounds>().PlayTeleportSound();
                var room = transform.parent.GetComponent<Room>();
                room.player.GetComponent<PlayerSounds>().PlayTeleportSound();
                room.UseDoor();
            }

            gameObject.SetActive(false);
        }        
    }

    void OnEnable()
    {
        if (isVoid)
        {
            gravPull.gameObject.SetActive(true);
        }

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
       


    }

    void OnDisable()
    {
        passed = false;
        Destroy(exp, exp.main.duration);
    }

}
