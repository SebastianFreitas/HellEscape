using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trial : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro price;
    [SerializeField] StartHub hub;

    private PlayerInventory playerInv;
    private void Awake()
    {
        playerInv = transform.root.GetComponent<GameMan>().player.GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        
        price.text = GetPrice().ToString();
    }
    internal bool StartTrial()
    {
        if (playerInv.gunParts >= GetPrice())
        {
            playerInv.UpdateGunParts(-GetPrice());
            GetComponentInParent<StartHub>().StartPath();
            gameObject.SetActive(false);

            return true;
        }
        else return false;
    }

    private int GetPrice()
    {
        if (transform.root.GetComponent<GameMan>().runsucess) return 0;
        
        var dificulty = 0;
        if (PlayerPrefs.HasKey("PathLevel")) dificulty = PlayerPrefs.GetInt("PathLevel");

        return 10 + dificulty;
    }
}
