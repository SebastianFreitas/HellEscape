using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonDrop : MonoBehaviour
{
    private VoidBoon boon;
    public TMPro.TextMeshPro text;

    [SerializeField] bool isInfluencePool = false;

    private int counter = 1;
    private PlayerInventory playerInv;
    [SerializeField] TMPro.TextMeshPro rerollPrice;

    private void Awake()
    {
        playerInv = transform.root.GetComponent<GameMan>().player.GetComponent<PlayerInventory>();


        boon = transform.root.GetComponent<GameMan>().boonManager;
        boon.RefreshBoon();

        text.text = boon.text;

        rerollPrice.text = GetRerollPrice().ToString();
    }

    internal void Accept()
    {
        boon.AcceptOrRemoveBoon(true);
        boon.RemoveBoon();

        gameObject.SetActive(false);

 

        //text.text = boon.text;
    }

    internal bool Reroll()
    {
        if(playerInv.gunParts >= GetRerollPrice())
        {
            boon.RefreshBoon();
            text.text = boon.text;

            counter++;
            rerollPrice.text = GetRerollPrice().ToString();

            return true;
        }
        return false;
    }

    private int GetRerollPrice()
    {
        return counter * 5;
    }
  
    internal void Deny()
    {
        boon.playerHP.TakeDamage(10);
        boon.RemoveBoon();
        gameObject.SetActive(false);
    }
}
