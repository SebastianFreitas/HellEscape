using UnityEngine;

internal class VoidBoon
{
    internal BoonType influence;

    internal enum BoonType
    {
        Blue,
        Red
    }

    internal enum tags
    {
        movement
    }

    internal PlayerBasicMovement playerMov;
    internal PlayerHpManager playerHP;
    internal PlayerInventory playerInv;
    internal GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Dude")[0];
        playerHP = player.GetComponent<PlayerHpManager>();
        playerInv = player.GetComponent<PlayerInventory>();
        playerMov = player.GetComponent<PlayerBasicMovement>();
    }

    internal void Maxhp1()
    {
        playerHP.ChangeMaxHP(10);
    }

    //red
    internal void Greed()
    {
        //Lose 50% of you max hp
        //deal +10 fire damage
    }
}