using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonDrop : MonoBehaviour
{
    private VoidBoon boon;
    public TMPro.TextMeshPro text;

    [SerializeField] bool isInfluencePool = false;

    private void Awake()
    {
        boon = new VoidBoon(isInfluencePool, GetComponentInParent<RoomActivator>().influcence);
        boon.player = GameObject.FindGameObjectsWithTag("Dude")[0];
        boon.playerHP = boon.player.GetComponent<PlayerHpManager>();
        boon.playerInv = boon.player.GetComponent<PlayerInventory>();
        boon.playerMov = boon.player.GetComponent<PlayerBasicMovement>();

        text.text = boon.text;
    }

    internal void Accept()
    {
        boon.AcceptOrRemoveBoon(true);
        gameObject.SetActive(false);
    }

    internal void Deny()
    {
        boon.playerHP.TakeDamage(5);
        gameObject.SetActive(false);
    }
}
