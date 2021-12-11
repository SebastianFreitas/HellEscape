using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidDamage : MonoBehaviour
{
    public Room room;
    public Hub hub;
    public bool isHub = false;
    public bool isRoom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Dude"))
        {
            if (isRoom) room.VoidPlayer();
            if (isHub) hub.VoidPlayer();
        }
    }



}
