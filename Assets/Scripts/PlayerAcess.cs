using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAcess : MonoBehaviour
{
    // Start is called before the first frame update
    public void Awake()
    {
        
        player = GameObject.FindGameObjectsWithTag("Dude")[0];
       playerHP = player.GetComponent<PlayerHpManager>();
       playerInv = player.GetComponent<PlayerInventory>();
       playerMov = player.GetComponent<PlayerBasicMovement>();

    }


    internal PlayerBasicMovement playerMov;
    internal PlayerHpManager playerHP;
    internal PlayerInventory playerInv;
    internal GameObject player;
}
